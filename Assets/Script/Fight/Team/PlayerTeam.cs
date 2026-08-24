using System.Collections.Generic;
using ConquerTheStars.Stats;
using UnityEngine;

public class PlayerTeam : MonoBehaviour
{
    public static PlayerTeam Instance { get; set; }
    public List<string> TeamNameList { get; set; } = new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        AddTeamMate("Player_I");
    }


    public void AddTeamMate(string name)
    {
        TeamNameList.Add(name);
    }
}
