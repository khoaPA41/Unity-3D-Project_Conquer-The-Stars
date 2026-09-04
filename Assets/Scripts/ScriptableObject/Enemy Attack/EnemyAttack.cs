using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "Scriptable Objects/EnemyAttack")]
public class EnemyAttack : ScriptableObject
{
    public List<string> AttackNameList;
}
