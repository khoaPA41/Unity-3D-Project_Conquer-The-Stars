using System;
using UnityEngine;


/// <summary>
/// This all data need to save in once playing
/// This class need to mark Serializable to JsonUtility convert JSON
/// </summary>
[Serializable]
public class SaveData
{
    // Checkpoint
    public string sceneName;
    public float xPosition;
    public float yPosition;
    public float zPosition;

    // Stats
    public int teamLevel;
}
