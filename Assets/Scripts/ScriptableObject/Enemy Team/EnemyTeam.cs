using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reward
{
    public int ExpReward;
}
[CreateAssetMenu(fileName = "EnemyTeam", menuName = "Scriptable Objects/EnemyTeam")]

public class EnemyTeam : ScriptableObject
{
    public List<PooledObjectId> enemyTeam;
    public int AreaIndex;
    public Reward Reward;
}
