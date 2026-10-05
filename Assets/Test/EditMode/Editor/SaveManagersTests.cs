using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class SaveManagersTests
{
    private GameObject _gameObject;
    private SaveManagers _saveManagers;


    [SetUp]
    public void SetUp()
    {
        _gameObject = new GameObject("SaveManager");
        _saveManagers = _gameObject.AddComponent<SaveManagers>();
    }

    [Test]
    public void SaveAndLoad_ReturnsSameData()
    {
        var saveData = new SaveData
        {
            SceneName = "Main",
            XPosition = 10f,
            YPosition = 0f,
            ZPosition = 20f,
            TeamLevel = 5
        };

        _saveManagers.SaveGame(saveData);
        var loadedData = _saveManagers.LoadSaveData();

        Assert.IsNotNull(loadedData);
        Assert.AreEqual(saveData.SceneName, loadedData.SceneName);
        Assert.AreEqual(saveData.XPosition, loadedData.XPosition, 0.001f);
        Assert.AreEqual(saveData.YPosition, loadedData.YPosition, 0.001f);
        Assert.AreEqual(saveData.ZPosition, loadedData.ZPosition, 0.001f);
        Assert.AreEqual(saveData.TeamLevel, loadedData.TeamLevel);
    }

    [Test]
    public void LoadSaveData_NoSaveFile_ReturnsNull()
    {
        var result = _saveManagers.LoadSaveData();
        Assert.IsNull(result);
    }

    [Test]
    public void LoadSaveData_ConrruptedFile_ReturnsNull()
    {
        var savePath = Path.Combine(Application.persistentDataPath, "CTS.json");
        File.WriteAllText(savePath, "{ Invalid");


        LogAssert.Expect(
            LogType.Error,
            "[SaveManagers] Failed to load save: JSON parse error: Missing a name for object member."
        );
        var result = _saveManagers.LoadSaveData();
        Assert.IsNull(result);
    }


    [Test]
    public void SaveGame_CreatesBackupOfPreviousSave()
    {
        var firstSaveData = new SaveData
        {
            SceneName = "Main",
            XPosition = 10f,
            YPosition = 0f,
            ZPosition = 20f,
            TeamLevel = 5
        };

        var secondSaveData = new SaveData
        {
            SceneName = "Main",
            XPosition = 100f,
            YPosition = 7f,
            ZPosition = 200f,
            TeamLevel = 3
        };

        _saveManagers.SaveGame(firstSaveData);
        _saveManagers.SaveGame(secondSaveData);

        var savePathBackup = Path.Combine(Application.persistentDataPath, "CTS_Backup.json");
        Assert.IsTrue(File.Exists(savePathBackup));

        var backupJson = File.ReadAllText(savePathBackup);
        var backupData = JsonUtility.FromJson<SaveData>(backupJson);

        Assert.AreEqual(firstSaveData.SceneName, backupData.SceneName);
        Assert.AreEqual(firstSaveData.XPosition, backupData.XPosition, 0.001f);
        Assert.AreEqual(firstSaveData.YPosition, backupData.YPosition, 0.001f);
        Assert.AreEqual(firstSaveData.ZPosition, backupData.ZPosition, 0.001f);
        Assert.AreEqual(firstSaveData.TeamLevel, backupData.TeamLevel);
    }

    [TearDown]
    public void TearDown()
    {
        var savePath = Path.Combine(Application.persistentDataPath, "CTS.json");
        var savePathBackup = Path.Combine(Application.persistentDataPath, "CTS_Backup.json");

        if (File.Exists(savePath))
            File.Delete(savePath);

        if (File.Exists(savePathBackup))
            File.Delete(savePathBackup);

        Object.DestroyImmediate(_gameObject);
    }
}
