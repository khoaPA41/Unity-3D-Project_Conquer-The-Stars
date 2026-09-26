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

        public List<float> Evaluate(List<CharacterStatsManagers> targets)
        {
            var listCharacterScore = new List<float>();

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].IsDeath)
                {
                    listCharacterScore.Add(0f);
                    continue;
                }
                var finalScore = EvaluateHP(targets[i]) * _strategyEvaluation.hpWeight +
                                EvaluateThreat(targets[i]) * _strategyEvaluation.threatWeight +
                                EvaluateDefense(targets[i]) * _strategyEvaluation.defenseWeight;

                listCharacterScore.Add(finalScore);
            }
            return listCharacterScore;
        }

        private float EvaluateHP(CharacterStatsManagers target)
        {
            var healthNomalized = 1 - (target.CurrentHealth / target.maxHealth.GetFinalValue());
            return healthNomalized;
        }
        private float EvaluateThreat(CharacterStatsManagers targets)
        {
            BattleStatistics battleStatistics = targets.GetComponent<PlayerCombatStateMachine>().BattleStatistics;
            if (battleStatistics.DamageHistories.Count == 0) return 0f;
            var damageNomalized = battleStatistics.DamageHistories.Average() / targets.attack.GetFinalValue();
            return damageNomalized;
        }
        private float EvaluateDefense(CharacterStatsManagers target)
        {
            var defenseNomalized = 1 - (target.defense.GetFinalValue() / 100f);
            return defenseNomalized;
        }

        public CharacterStatsManagers GetBestTarget(List<CharacterStatsManagers> targets)
        {
            var scoredList = Evaluate(targets);

            int bestIndex = 0;

            for (int i = 0; i < scoredList.Count; i++)
            {
                if (scoredList[i] > scoredList[bestIndex])
                {
                    bestIndex = i;
                }
            }

            return targets[bestIndex];
        }
    }
}