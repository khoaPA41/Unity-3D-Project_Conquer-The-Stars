using System;
using System.Collections;
using System.Collections.Generic;
using ConquerTheStars.Pattern.Object_Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UIManagers : MonoBehaviour
{
    private readonly string mainSceneName = "Main";

    private readonly string killElementName = "KillElement";
    public static UIManagers Instance;

    [Header("Skill UI")]

    [field: Header("Skill Frame Action")]
    [field: SerializeField] public SkillActionFrame SkillActionFrame { get; set; }

    [field: Header("Status Panel")]
    [field: SerializeField] public RectTransform StatusPanel { get; private set; }

    [SerializeField] private float timeToFill;

    [Header("Result")]
    [SerializeField] private GameObject resultBoard;
    [SerializeField] private TextMeshProUGUI highestDamageText;
    [SerializeField] private TextMeshProUGUI damageDealtText;
    [SerializeField] private TextMeshProUGUI damageReceivedText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI successfulPariesText;
    [SerializeField] private TextMeshProUGUI successfulDodgesText;
    [SerializeField] private RectTransform Kills;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /**** Action Frame ****/
    public void ActiveActionFrame()
    {
        SkillActionFrame.gameObject.SetActive(true);
    }

    public void PausePerfectFrame()
    {
        SkillActionFrame.PauseActionFrame();
        SkillActionFrame.CallPausedAction();
    }

    public float GetActionFrameValue() => SkillActionFrame.ActionFrameValue;


    /**** Result Board ****/
    public void ActiveResultBoard()
    {
        resultBoard.SetActive(true);
    }

    public void SetResultText(string highestDamageText, string damageDealtText, string damageReceivedText,
    string timeText, string successfulPariesText, string successfulDodgesText)
    {
        this.highestDamageText.SetText(highestDamageText);
        this.damageDealtText.SetText(damageDealtText);
        this.damageReceivedText.SetText(damageReceivedText);
        this.timeText.SetText(timeText);
        this.successfulPariesText.SetText(successfulPariesText);
        this.successfulDodgesText.SetText(successfulDodgesText);
    }

    public void SpawnKillElement(Sprite enemyIcon)
    {
        var killElement = ObjectPoolingManagers.Instance.GetPooledObject(killElementName, Vector3.zero);
        killElement.GetComponent<RectTransform>().SetParent(Kills);
        killElement.GetComponent<EnemyKillElement>().SetIcon(enemyIcon);
    }

    /**** Result Board ****/
    public void ReturMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}
