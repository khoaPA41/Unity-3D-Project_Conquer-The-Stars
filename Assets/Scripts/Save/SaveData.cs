using System;
using System.Collections.Generic;
using ConquerTheStars.Fight;

/// <summary>
/// This all data need to save in once playing
/// This class need to mark Serializable to JsonUtility convert JSON
/// </summary>
[Serializable]
public class SaveData
{
    // Checkpoint
    public string SceneName;
    public float XPosition;
    public float YPosition;
    public float ZPosition;

    // Level
    public int TeamLevel;
    public int Exp;
    public int CurrentNeededExp;

    //Battle
    public List<string> BattleCompletedIds = new();

    //Item
    public List<ItemInfoForSave> ItemInfo = new();
}
