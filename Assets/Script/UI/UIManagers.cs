using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UIManagers : MonoBehaviour
{
    public static UIManagers Instance;

    [Header("Skill UI")]
    [SerializeField] private List<SkillSelectionElement> skillSelection;

    [field: Header("Skill Frame Action")]
    [field: SerializeField] public SkillActionFrame SkillActionFrame { get; set; }

    [field: Header("Status Panel")]
    [field: SerializeField] public RectTransform StatusPanel { get; private set; }

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

    // public void SetupSkillSelectionUI(List<Sprite> icons, List<string> inforList)
    // {
    //     for (int i = 0; i < skillSelection.Count; i++)
    //     {
    //         skillSelection[i].Icon.sprite = icons[i];
    //         skillSelection[i].Information.SetText(inforList[i]);
    //     }
    // }

    public void AppearSkillSelection()
    {
        // skilLSelectionPanelAnimator.SetTrigger(AppearTrigger);
    }

    public void DisappearSkillSelection()
    {
        // skilLSelectionPanelAnimator.SetTrigger(DisappearTrigger);

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
