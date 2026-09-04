using System;
using System.Collections.Generic;
using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Stats;
using UnityEngine;
[Serializable]

public class BossPhaseInformation
{
    public EnemyAttack EnemyAttack;
    public float healthThreshold;
}
public class BossPhase : MonoBehaviour
{
    [field: SerializeField] public List<BossPhaseInformation> Attacks;

    [SerializeField] private CharacterStatsManagers bossStatsManagers;

    [SerializeField] private EnemyStateMachine enemyStateMachine;

    public EnemyAttack EnemyAttack;



    private void OnEnable()
    {
        bossStatsManagers.HealthUpdateAction += GetAttackByPhase;
    }


    private void OnDisable()
    {
        bossStatsManagers.HealthUpdateAction -= GetAttackByPhase;
    }

    public void GetAttackByPhase(float healthThreshold)
    {
        foreach (var phase in Attacks)
        {
            Debug.Log(healthThreshold);
            if (healthThreshold <= phase.healthThreshold)
            {
                enemyStateMachine.EnemyAttack = phase.EnemyAttack;
                Debug.Log(enemyStateMachine.EnemyAttack);
                EnemyAttack = phase.EnemyAttack;
            }
        }
    }
}
