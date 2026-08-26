using System;
using UnityEngine;

public class SelectionBar : MonoBehaviour
{
    private readonly int SkillSelectionAppearAnimationHash = Animator.StringToHash("Appear");
    private readonly int SkillSelectionDisappearAnimationHash = Animator.StringToHash("Disappear");
    [Header("Skill Board")]
    [SerializeField] private GameObject skillBoardRoot;
    [SerializeField] private GameObject skillBoardOther_I;
    [SerializeField] private GameObject skillBoardOthe_II;
    [SerializeField] private Animator animator;


    public void ActiveSkillBoard()
    {
        skillBoardOther_I.SetActive(false);
        skillBoardOthe_II.SetActive(false);
        skillBoardRoot.SetActive(true);
        animator.SetTrigger(SkillSelectionAppearAnimationHash);
    }
}
