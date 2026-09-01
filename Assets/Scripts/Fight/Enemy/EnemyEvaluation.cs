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
        [SerializeField] private StrategyEvaluation strategyEvaluation;

        public List<float> Evaluate(List<CharacterStatsManagers> targets)
        {
            var listCharacterScore = new List<float>();

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].IsDeath)
                {
                    continue;
                }
                var finalScore = EvaluateHP(targets[i]) * strategyEvaluation.hpWeight +
                                EvaluateThreat(targets[i]) * strategyEvaluation.threatWeight +
                                EvaluateDefense(targets[i]) * strategyEvaluation.defenseWeight +
                                EvaluateStatus(targets[i]) * strategyEvaluation.statusWeight;

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
        private float EvaluateStatus(CharacterStatsManagers target)
        {
            return 0;
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