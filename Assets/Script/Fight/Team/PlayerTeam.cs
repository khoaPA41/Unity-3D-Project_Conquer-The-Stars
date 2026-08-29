using System.Collections.Generic;
using ConquerTheStars.Stats;
using UnityEngine;

public class PlayerTeam : MonoBehaviour
{
    public static PlayerTeam Instance { get; set; }
    public List<string> TeamNameList { get; set; } = new();

    public Vector3 CurrentPosition { get; private set; } = new Vector3(36f, 0f, 62f);

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

    public void SavePos(Vector3 position)
    {
        CurrentPosition = position;
    }
}
