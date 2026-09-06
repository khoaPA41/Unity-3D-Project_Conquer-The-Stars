using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTeam", menuName = "Scriptable Objects/EnemyTeam")]
public class EnemyTeam : ScriptableObject
{
    public List<string> enemyTeam;
    public int AreaIndex;
}
