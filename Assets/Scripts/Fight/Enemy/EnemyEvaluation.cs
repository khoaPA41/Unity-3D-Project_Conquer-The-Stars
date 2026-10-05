using System.Collections.Generic;
using System.Linq;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Fight.Enemy
{
    public readonly struct TargetScoreBreakdown
    {
        public readonly CharacterStatsManagers Target;
        public readonly float Hp;
        public readonly float Threat;
        public readonly float Defense;
        public readonly float Total;

        public TargetScoreBreakdown(
            CharacterStatsManagers target, float hp, float threat, float defense, float total)
        {
            Target = target;
            Hp = hp;
            Threat = threat;
            Defense = defense;
            Total = total;
        }
    }


    [RequireComponent(typeof(CharacterStatsManagers))]
    public class EnemyEvaluation : MonoBehaviour
    {
        [SerializeField] private StrategyEvaluation _strategyEvaluation;
        private readonly Dictionary<CharacterStatsManagers, float> _targetByScore = new();

        public void Initialize(StrategyEvaluation strategyEvaluation)
        {
            _strategyEvaluation = strategyEvaluation;
        }

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
            var player = targets.GetComponent<PlayerCombatStateMachine>();

            if (player == null) return 0f;

            BattleStatistics battleStatistics = player.BattleStatistics;
            if (battleStatistics == null) return 0f;


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


        public List<TargetScoreBreakdown> EvaluateDetailed(List<CharacterStatsManagers> targets)
        {
            var list = new List<TargetScoreBreakdown>();
            if (targets == null || targets.Count == 0) return list;

            for (int i = 0; i < targets.Count; i++)
            {
                var t = targets[i];
                if (t == null || t.IsDeath) continue;

                float hp = EvaluateHP(t);
                float threat = EvaluateThreat(t);
                float def = EvaluateDefense(t);
                float total = hp * _strategyEvaluation.hpWeight
                            + threat * _strategyEvaluation.threatWeight
                            + def * _strategyEvaluation.defenseWeight;

                list.Add(new TargetScoreBreakdown(t, hp, threat, def, total));
            }
            return list;
        }
    }
}