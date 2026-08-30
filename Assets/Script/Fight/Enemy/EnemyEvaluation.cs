using System;
using System.Collections.Generic;
using ConquerTheStars.Stats;
using UnityEngine;

[RequireComponent(typeof(CharacterStatsManagers))]
public class EnemyEvaluation : MonoBehaviour
{
    [SerializeField] private StrategyEvaluation strategyEvaluation;
    private CharacterStatsManagers characterStatsManagers;

    private void OnEnable()
    {
        characterStatsManagers = GetComponent<CharacterStatsManagers>();
    }

    public List<float> Evaluate(List<CharacterStatsManagers> targets)
    {
        var listCharacterScore = new List<float>();

        for (int i = 0; i < targets.Count; i++)
        {
            var finalScore = EvaluateHP(targets[i]) * strategyEvaluation.hpWeight +
                        EvaluateThreat() * strategyEvaluation.threatWeight +
                        EvaluateDefense(targets[i]) * strategyEvaluation.defenseWeight +
                        EvaluateStatus(targets[i]) * strategyEvaluation.statusWeight;

            listCharacterScore.Add(finalScore);
        }
        return listCharacterScore;
    }

    private float EvaluateHP(CharacterStatsManagers target)
    {
        var healthScore = 1 - (target.CurrentHealth / target.maxHealth.GetFinalValue());
        return healthScore * strategyEvaluation.hpWeight;
    }
    private float EvaluateThreat()
    {
        return characterStatsManagers.DamageReceived * strategyEvaluation.threatWeight;
    }
    private float EvaluateDefense(CharacterStatsManagers target)
    {
        return target.defense.GetFinalValue() * strategyEvaluation.defenseWeight;
    }
    private float EvaluateStatus(CharacterStatsManagers target)
    {
        return 0;
    }
}
