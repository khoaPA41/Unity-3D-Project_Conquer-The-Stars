using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagers : MonoBehaviour
{
    public static UIManagers Instance;
    private readonly int AppearTrigger = Animator.StringToHash("Appear");
    private readonly int DisappearTrigger = Animator.StringToHash("Disappear");

    [Header("Player UI")]
    [SerializeField] private List<GameObject> healthPanel;
    [SerializeField] private List<Image> healthFillList;

    [Header("Skill UI")]
    [SerializeField] private List<SkillSelectionElement> skillSelection;

    [field: Header("Skill Frame Action")]
    [field: SerializeField] public SkillActionFrame SkillActionFrame { get; set; }


    [SerializeField] private Animator skilLSelectionPanelAnimator;
    [SerializeField] private float timeToFill;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetupSkillSelectionUI(List<Sprite> icons, List<string> inforList)
    {
        for (int i = 0; i < skillSelection.Count; i++)
        {
            skillSelection[i].Icon.sprite = icons[i];
            skillSelection[i].Information.SetText(inforList[i]);
        }
    }

    public void AppearSkillSelection()
    {
        // skilLSelectionPanelAnimator.SetTrigger(AppearTrigger);
    }

    public void DisappearSkillSelection()
    {
        // skilLSelectionPanelAnimator.SetTrigger(DisappearTrigger);

    }

    public void SetHealth(int healthUiIndex, float health)
    {
        healthPanel[healthUiIndex].SetActive(true);
        StartCoroutine(HealthChangeCoroutine(healthFillList[healthUiIndex], health));
    }

    private IEnumerator HealthChangeCoroutine(Image healthFill, float targetHealth)
    {
        var currentHealth = healthFill.fillAmount;
        var elapsed = 0f;
        while (elapsed < timeToFill)
        {
            elapsed += Time.deltaTime;
            var percentageTime = Mathf.Clamp01(elapsed / timeToFill);

            var lerpHealth = Mathf.Lerp(currentHealth, targetHealth, percentageTime);
            healthFill.fillAmount = lerpHealth;
            yield return null;
        }
        healthFill.fillAmount = targetHealth;
    }

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
}
