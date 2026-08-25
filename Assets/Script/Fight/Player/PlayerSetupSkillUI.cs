using System;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConquerTheStars.Fight.Player
{
    [RequireComponent(typeof(PlayerCombatStateMachine))]
    public class PlayerSetupSkillUI : MonoBehaviour
    {
        private readonly int SkillSelectionAppearAnimationHash = Animator.StringToHash("Appear");
        private readonly int SkillSelectionDisappearAnimationHash = Animator.StringToHash("Disappear");

        [Header("Canvas")]
        [SerializeField]
        private Canvas playerCanvas;

        [Header("Skill UI Icon")]
        [SerializeField]
        private Image Skill_I_Icon;
        [SerializeField] private Image Skill_II_Icon;
        [SerializeField] private Image Skill_III_Icon;

        [Header("Skill UI Text")]
        [SerializeField]
        private TextMeshProUGUI Skill_I_Text;
        [SerializeField] private TextMeshProUGUI Skill_II_Text;
        [SerializeField] private TextMeshProUGUI Skill_III_Text;

        [Header("Animator")]
        [SerializeField]
        private Animator skillAnimator;

        [Header("StateMachine")]
        [SerializeField] private PlayerCombatStateMachine playerCombatStateMachine;

        private Camera mainCamera;

        private void Start()
        {

        }

        private void OnEnable()
        {
            mainCamera = Camera.main;
            if (playerCombatStateMachine != null)
            {
                SetupSkillUiIcon();
                SetupSkillUiText();
                if (mainCamera != null)
                {
                    SetupCamera();
                }
            }
        }


        private void SetupSkillUiIcon()
        {
            Skill_I_Icon.sprite = playerCombatStateMachine.AttackData.AttackIcon[0];
            Skill_II_Icon.sprite = playerCombatStateMachine.AttackData.AttackIcon[1];
            Skill_III_Icon.sprite = playerCombatStateMachine.AttackData.AttackIcon[2];
        }


        private void SetupSkillUiText()
        {
            Skill_I_Text.SetText(playerCombatStateMachine.AttackData.AttackInformation[0]);
            Skill_II_Text.SetText(playerCombatStateMachine.AttackData.AttackInformation[1]);
            Skill_III_Text.SetText(playerCombatStateMachine.AttackData.AttackInformation[2]);
        }

        private void SetupCamera()
        {
            playerCanvas.worldCamera = mainCamera;
        }

        public void AppearSkillUI()
        {
            playerCanvas.gameObject.SetActive(true);
            skillAnimator.SetTrigger(SkillSelectionAppearAnimationHash);
        }
        public void DisappearSkillUI()
        {
            skillAnimator.SetTrigger(SkillSelectionDisappearAnimationHash);
            playerCanvas.gameObject.SetActive(false);
        }

        public void InActiveSkillUI()
        {
            playerCanvas.gameObject.SetActive(false);
        }
    }

}
