using System;
using System.Collections.Generic;
using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Stats;
using UnityEngine;

[Serializable]
public class BossPhaseInformation
{
    public EnemyAttack EnemyAttack;
    public float HealthThreshold;
}

public class BossPhase : MonoBehaviour
{
    [field: SerializeField] public List<BossPhaseInformation> Attacks { get; private set; }

    [SerializeField] private CharacterStatsManagers _bossStatsManagers;

    [SerializeField] private EnemyStateMachine _enemyStateMachine;

    public EnemyAttack EnemyAttack;


    private void OnEnable()
    {
        _bossStatsManagers.HealthUpdateAction += GetAttackByPhase;
    }

    private void OnDisable()
    {
        _bossStatsManagers.HealthUpdateAction -= GetAttackByPhase;
    }

    public void GetAttackByPhase(float healthThreshold)
    {
        BossPhaseInformation bestPhase = null;

        foreach (var phase in Attacks)
        {
            if (healthThreshold <= phase.HealthThreshold)
            {
                if (bestPhase == null || phase.HealthThreshold < bestPhase.HealthThreshold)
                {
                    bestPhase = phase;
                }
            }
        }

        if (bestPhase != null)
        {
            _enemyStateMachine.EnemyAttack = bestPhase.EnemyAttack;
            EnemyAttack = bestPhase.EnemyAttack;
        }
    }
}
