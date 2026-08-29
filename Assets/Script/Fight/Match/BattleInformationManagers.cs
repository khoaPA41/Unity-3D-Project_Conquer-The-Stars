using System.Collections.Generic;
using UnityEngine;


public class BattleInformationManagers : MonoBehaviour
{
    public static BattleInformationManagers Instance;
    public EnemyTeam AreaInformation { get; set; }

    public PlayerTeam PlayerTeam { get; set; }

    public List<GameObject> area;
    public List<bool> isAvtive;
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

    private void OnEnable()
    {
        if (area.Count > 0)
            for (int i = 0; i < area.Count; i++)
            {
                area[i].SetActive(isAvtive[i]);
            }
    }

    public void SetArea(EnemyTeam enemyTeam)
    {
        AreaInformation = enemyTeam;
        // area.Add(enemyTeam.gameObject);
        // isAvtive.Add(false);
    }
}
