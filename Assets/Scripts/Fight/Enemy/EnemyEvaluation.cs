using System.Collections.Generic;
using System.Linq;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Fight.Enemy
{
    [RequireComponent(typeof(CharacterStatsManagers))]
    public class EnemyEvaluation : MonoBehaviour
    {
        [SerializeField] private StrategyEvaluation _strategyEvaluation;
        private readonly Dictionary<CharacterStatsManagers, float> _targetByScore = new();

        public Dictionary<CharacterStatsManagers, float> Evaluate(List<CharacterStatsManagers> targets)
        {
            _targetByScore.Clear();
            if (targets == null || targets.Count == 0) return null;

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].IsDeath) continue;
                var finalScore = EvaluateHP(targets[i]) * _strategyEvaluation.hpWeight +
                                EvaluateThreat(targets[i]) * _strategyEvaluation.threatWeight +
                                EvaluateDefense(targets[i]) * _strategyEvaluation.defenseWeight;

                _targetByScore.Add(targets[i], finalScore);
            }
            return _targetByScore;
        }

        private float EvaluateHP(CharacterStatsManagers target)
        {
            var maxHp = target.maxHealth.GetFinalValue();
            if (maxHp <= 0) return 0f;
            var healthNormalized = 1 - (target.CurrentHealth / maxHp);
            return healthNormalized;
        }

        private float EvaluateThreat(CharacterStatsManagers targets)
        {
            BattleStatistics battleStatistics = targets.GetComponent<PlayerCombatStateMachine>().BattleStatistics;
            if (battleStatistics.DamageHistories.Count == 0) return 0f;
            var damageNormalized = battleStatistics.DamageHistories.Average() / targets.attack.GetFinalValue();
            return damageNormalized;
        }

        private float EvaluateDefense(CharacterStatsManagers target)
        {
            var defenseNormalized = 1 - (target.defense.GetFinalValue() / 100f);
            return Mathf.Clamp01(defenseNormalized);
        }

        public CharacterStatsManagers GetBestTarget(List<CharacterStatsManagers> targets)
        {
            var scoredList = Evaluate(targets);
            if (scoredList == null || scoredList.Count == 0) return null;

            var highest = scoredList.OrderByDescending(target => target.Value).First();

            _targetByScore.Clear();
            return highest.Key;
        }
    }
}