using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyTeam : MonoBehaviour
{
    [field: Header("Team Trasnform")]
    [field: SerializeField] public List<string> enemyTeam;
    [field: SerializeField] public int AreaIndex;
}
