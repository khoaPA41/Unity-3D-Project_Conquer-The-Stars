using System.IO;
using UnityEngine;

public class SaveManagers : MonoBehaviour
{
    public static SaveManagers Instance { get; private set; }

    public SaveData CurrentSaveData;

    private string savePath => Path.Combine(Application.persistentDataPath, "CTS.json");

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

    public bool HasSaveData()
    {
        return File.Exists(savePath);
    }

    public void CreateNewSaveData()
    {
        CurrentSaveData = new SaveData();
        Debug.Log("[SaveManagers] Created Save Game" + savePath);
    }

    public void DeleteSaveData()
    {
        if (HasSaveData())
        {
            File.Delete(savePath);
        }
        CurrentSaveData = null;
    }

    public void SaveGame(SaveData saveData)
    {
        var dataJson = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, dataJson);

        CurrentSaveData = saveData;
        Debug.Log("[SaveManagers] Saved Game" + savePath);
    }

    public SaveData LoadSaveData()
    {
        if (!HasSaveData())
        {
            Debug.LogWarning("[SaveManagers] Don't have save data]");
            return null;
        }
        var dataJson = File.ReadAllText(savePath);
        CurrentSaveData = JsonUtility.FromJson<SaveData>(dataJson);
        return CurrentSaveData;
    }
}
