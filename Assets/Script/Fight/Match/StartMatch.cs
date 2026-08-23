using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyTransformList // This list will constain transform enemy will be spawn
{
    public List<Transform> enemyTransformList;
}

[Serializable]
public class PlayerTransformList // This list will constain transform player will be spawn
{
    public List<Transform> playerTransformList;
}

public class StartMatch : MonoBehaviour
{
    [Header("Spawn Area")]
    [field: SerializeField]
    public List<EnemyTransformList> enemyTransformList;
    [field: SerializeField] public List<PlayerTransformList> playerTransformList;
}
