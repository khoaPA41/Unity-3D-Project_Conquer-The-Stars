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
    [SerializeField] private GameObject _soundSettings;
    [SerializeField] private GameObject _exitChoose;


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
        _exitChoose.SetActive(false);
        _soundSettings.SetActive(true);
    }

    public void ActiveExitChoose()
    {
        _soundSettings.SetActive(false);
        _exitChoose.SetActive(true);
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
