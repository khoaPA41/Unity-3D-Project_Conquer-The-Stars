using System.Collections.Generic;
using UnityEngine;


public class BattleInformationManagers : MonoBehaviour
{
    public static BattleInformationManagers Instance;
    public EnemyTeam AreaInformation { get; set; }

    public PlayerTeam PlayerTeam { get; set; }

    public List<GameObject> area;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetArea(EnemyTeam enemyTeam)
    {
        AreaInformation = enemyTeam;
    }
}
