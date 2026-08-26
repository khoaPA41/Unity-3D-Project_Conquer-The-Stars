using System;
using ConquerTheStars.Pattern.Object_Pooling;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConquerTheStars.Fight.Player
{
    [RequireComponent(typeof(PlayerCombatStateMachine))]
    public class PlayerSetupSkillUI : MonoBehaviour
    {
        private readonly string SkillElementName = "Skill_Board_Element";
        private readonly int SkillSelectionAppearAnimationHash = Animator.StringToHash("Appear");
        private readonly int SkillSelectionDisappearAnimationHash = Animator.StringToHash("Disappear");

        [Header("Canvas")]
        [SerializeField]
        private Canvas playerCanvas;

        [Header("Selection Object")]
        [SerializeField]
        private RectTransform selection_I;
        [SerializeField] private RectTransform selection_II;
        [SerializeField] private RectTransform selection_III;

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
            if (playerCombatStateMachine != null && ObjectPoolingManagers.Instance.IsSetupFinished)
            {
                SetupAttackUI();
                SetupSkillUI();
                if (mainCamera != null)
                {
                    SetupCamera();
                }
            }
        }


        private void SetupAttackUI()
        {
            for (int i = 0; i < playerCombatStateMachine.AttackData.AttackIcon.Count; i++)
            {
                var skillUI = ObjectPoolingManagers.Instance.GetPooledObject(SkillElementName, Vector3.zero, Vector3.zero);
                var skillElement = skillUI.GetComponent<SkillElement>();
                var skillRectTransform = skillElement.GetComponent<RectTransform>();
                skillRectTransform.SetParent(selection_I, false);
                skillElement.SetupSkillElement(playerCombatStateMachine.AttackData.AttackIcon[i], playerCombatStateMachine.AttackData.AttackInformation[i]);

                //button
                var index = i;
                skillElement.Button.onClick.AddListener(() =>
                {
                    playerCombatStateMachine.GetIndexAction("Attack", index);
                });
            }
        }


        private void SetupSkillUI()
        {
            for (int i = 0; i < playerCombatStateMachine.AttackData.SkillIcon.Count; i++)
            {
                var skillElement = ObjectPoolingManagers.Instance.GetPooledObject(SkillElementName, Vector3.zero, Vector3.zero).GetComponent<SkillElement>();
                var skillRectTransform = skillElement.GetComponent<RectTransform>();
                skillRectTransform.SetParent(selection_II, false);
                skillElement.SetupSkillElement(playerCombatStateMachine.AttackData.SkillIcon[i], playerCombatStateMachine.AttackData.SkillInformation[i]);

                //button
                var index = i;
                skillElement.Button.onClick.AddListener(() =>
                {
                    playerCombatStateMachine.GetIndexAction("Skill", index);
                });
            }
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
