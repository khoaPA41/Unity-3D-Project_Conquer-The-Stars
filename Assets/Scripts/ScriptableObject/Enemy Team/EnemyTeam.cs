using System;
using System.Collections.Generic;
using ConquerTheStars.Fight;
using UnityEngine;

[Serializable]
public class Reward
{
    public int ExpReward;
    public ItemAttachData ItemAttachReward;
    public List<ItemInfoForSave> ItemReward;
}

[CreateAssetMenu(fileName = "EnemyTeam", menuName = "Scriptable Objects/EnemyTeam")]

public class EnemyTeam : ScriptableObject
{
    public List<PooledObjectId> enemyTeam;
    public int AreaIndex;
    public Reward Reward;
}
