using System;
using System.Collections.Generic;
using ConquerTheStars.Stats;
using UnityEngine;


[Serializable]
public class ItemQuantity
{
    public ItemData ItemData;
    public int Quantity;
}

// [Serializable]
// public class TeamLevel
// {
//     public ItemData ItemData;
//     public int Quantity;
// }
public class PlayerTeam : MonoBehaviour
{
    public static PlayerTeam Instance { get; set; }

    [SerializeField] private List<ItemQuantity> itemDatas;
    public List<string> TeamNameList { get; set; } = new();

    public Vector3 CurrentPosition { get; private set; } = new Vector3(36f, 0f, 62f);

    public int TeamLevel { get; set; } = 1;

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
        AddTeamMate("Player_II");
        AddTeamMate("Player_III");
    }


    public void AddTeamMate(string name)
    {
        TeamNameList.Add(name);
    }

    // public void SavePos(Vector3 position)
    // {
    //     CurrentPosition = position;
    // }

    public List<ItemQuantity> GetItemList()
    {
        return itemDatas;
    }

    public ItemData GetItemData(int index)
    {
        return itemDatas[index].ItemData;
    }

    public void AddQuantity()
    {
        for (int i = 0; i < itemDatas.Count; i++)
        {

        }
    }
}
