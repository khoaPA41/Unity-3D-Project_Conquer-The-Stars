using System;
using System.IO;
using UnityEngine;

public class SaveManagers : MonoBehaviour
{
    public static SaveManagers Instance { get; private set; }

    public SaveData CurrentSaveData;

    private string savePath => Path.Combine(Application.persistentDataPath, "CTS.json");
    private string savePathBackup => Path.Combine(Application.persistentDataPath, "CTS_Backup.json");

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
        try
        {
            var dataJson = JsonUtility.ToJson(saveData, true);
            if (HasSaveData())
            {
                File.Copy(savePath, savePathBackup, true);
            }
            File.WriteAllText(savePath, dataJson);

            CurrentSaveData = saveData;
            Debug.Log("[SaveManagers] Saved Game" + savePath);
        }
        catch (Exception exception)
        {
            Debug.LogError($"[SaveManagers] Failed to save: {exception.Message}");
        }

    }

    public SaveData LoadSaveData()
    {
        if (!HasSaveData())
        {
            Debug.LogWarning("[SaveManagers] Don't have save data");
            return null;
        }

        try
        {
            var dataJson = File.ReadAllText(savePath);
            var readingSaveData = JsonUtility.FromJson<SaveData>(dataJson);
            if (readingSaveData == null)
            {
                Debug.LogWarning("[SaveManagers] Invalid save data");
                return null;
            }

            CurrentSaveData = readingSaveData;
            return CurrentSaveData;
        }
        catch (Exception exception)
        {
            Debug.LogError($"[SaveManagers] Failed to load save: {exception.Message}");
            return null;
        }

    }
}
