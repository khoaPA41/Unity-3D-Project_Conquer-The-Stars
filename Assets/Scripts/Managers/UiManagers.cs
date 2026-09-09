using System;
using ConquerTheStars.Managers;
using ConquerTheStars.Pattern.StateMachine.Battle;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManagers : MonoBehaviour
{
    private readonly string StartMenuScene = "Start";
    private readonly string CommbatScene = "Battle";

    public static UiManagers Instance;
    [field: SerializeField] public GameObject settingsPanel;
    [SerializeField] private GameObject soundSettings;
    [SerializeField] private GameObject exitChoose;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ActiveSettingsPanel(bool active)
    {
        settingsPanel.SetActive(active);
    }

    public void ActiveSoundSettings()
    {
        exitChoose.SetActive(false);
        soundSettings.SetActive(true);
    }

    public void ActiveExitChoose()
    {
        soundSettings.SetActive(false);
        exitChoose.SetActive(true);
    }


    public void ExitGame()
    {
        GameManager.Instance.Exit();
    }

    public void ExitTitle()
    {

        if (SceneManager.GetActiveScene().name == CommbatScene)
        {
            if (GameObject.FindGameObjectWithTag("BattleStateMachine").TryGetComponent<BattleStateMachine>(out var battle))
                battle.ReleaseAllTeam();
        }

        if (SceneManager.GetActiveScene().name == StartMenuScene)
        {
            ActiveSettingsPanel(false);
            return;
        }
        ActiveSettingsPanel(false);
        GameManager.Instance.ExitToTitle();
    }
}
