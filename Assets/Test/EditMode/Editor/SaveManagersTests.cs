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
            sceneName = "Main",
            xPosition = 10f,
            yPosition = 0f,
            zPosition = 20f,
            teamLevel = 5
        };

        _saveManagers.SaveGame(saveData);
        var loadedData = _saveManagers.LoadSaveData();

        Assert.IsNotNull(loadedData);
        Assert.AreEqual(saveData.sceneName, loadedData.sceneName);
        Assert.AreEqual(saveData.xPosition, loadedData.xPosition, 0.001f);
        Assert.AreEqual(saveData.yPosition, loadedData.yPosition, 0.001f);
        Assert.AreEqual(saveData.zPosition, loadedData.zPosition, 0.001f);
        Assert.AreEqual(saveData.teamLevel, loadedData.teamLevel);
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
            sceneName = "Main",
            xPosition = 10f,
            yPosition = 0f,
            zPosition = 20f,
            teamLevel = 5
        };

        var secondSaveData = new SaveData
        {
            sceneName = "Main",
            xPosition = 100f,
            yPosition = 7f,
            zPosition = 200f,
            teamLevel = 3
        };

        _saveManagers.SaveGame(firstSaveData);
        _saveManagers.SaveGame(secondSaveData);

        var savePathBackup = Path.Combine(Application.persistentDataPath, "CTS_Backup.json");
        Assert.IsTrue(File.Exists(savePathBackup));

        var backupJson = File.ReadAllText(savePathBackup);
        var backupData = JsonUtility.FromJson<SaveData>(backupJson);

        Assert.AreEqual(firstSaveData.sceneName, backupData.sceneName);
        Assert.AreEqual(firstSaveData.xPosition, backupData.xPosition, 0.001f);
        Assert.AreEqual(firstSaveData.yPosition, backupData.yPosition, 0.001f);
        Assert.AreEqual(firstSaveData.zPosition, backupData.zPosition, 0.001f);
        Assert.AreEqual(firstSaveData.teamLevel, backupData.teamLevel);
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
