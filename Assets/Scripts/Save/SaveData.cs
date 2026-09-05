using UnityEngine;


/// <summary>
/// This all data need to save in once playing
/// This class need to mark Serializable to JsonUtility convert JSON
/// </summary>
public class SaveData
{
    // Checkpoint
    public string sceneName;
    public float xPosition;
    public float yPosition;
    public float zPosition;

    // Stats
    public int teamLevel;


    // Sound Settings
    public float masterVolume;
    public float BGMVolume;
    public float SFXVolume;
    public float UIVolume;


    // Graphic Settings
    public int resolutionIndex;
    public int displayModeIndex;
    public bool vsync;
    public int fps;
    public int qualityPresentIndex;
    public bool shadow;
    public int antiAliasingIndex;
    public int textureQualityIndex;
    public bool bloom;
    public bool motionBlur;
    public bool ambientOcclusion;
}
