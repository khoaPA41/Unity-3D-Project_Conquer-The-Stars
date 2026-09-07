using System.Collections.Generic;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.UI.Player;
using UnityEngine;

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

        [Header("Selection Object")]
        [SerializeField]
        private RectTransform selection_I;
        [SerializeField] private RectTransform selection_II;
        [SerializeField] private RectTransform selection_III;

        [Header("Skill Selection")]
        [SerializeField]
        private List<SkillElement> attackElement;
        [SerializeField] private List<SkillElement> skillElement;
        [SerializeField] private List<SkillElement> itemElement;

        [Header("Animator")]
        [SerializeField]
        private Animator skillAnimator;

        [Header("StateMachine")]
        [SerializeField] private PlayerCombatStateMachine playerCombatStateMachine;

        private Camera mainCamera;

        private bool isInitialized;

        private void OnEnable()
        {
            mainCamera = Camera.main;
            if (playerCombatStateMachine != null && PlayerTeam.Instance != null)
            {
                if (!isInitialized)
                {
                    SetupAttackUI();
                    SetupSkillUI();
                    SetupItemUI();
                    isInitialized = true;
                }

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
                // Setup attack element base AttackData
                attackElement[i].SetupSkillElement(playerCombatStateMachine.AttackData.AttackIcon[i],
                playerCombatStateMachine.AttackData.Attack[i],
                playerCombatStateMachine.AttackData.AttackInformation[i]
                );

                //Attach event
                var index = i;
                attackElement[i].Button.onClick.AddListener(() =>
                {
                    playerCombatStateMachine.GetIndexAction("Attack", index);
                });
            }
        }


        private void SetupSkillUI()
        {
            for (int i = 0; i < playerCombatStateMachine.AttackData.SkillIcon.Count; i++)
            {
                // Setup skill element base AttackData

                skillElement[i].SetupSkillElement(playerCombatStateMachine.AttackData.SkillIcon[i],
                playerCombatStateMachine.AttackData.Skill[i],
                playerCombatStateMachine.AttackData.SkillInformation[i]);

                //Attach event
                var index = i;
                skillElement[i].Button.onClick.AddListener(() =>
                {
                    playerCombatStateMachine.GetIndexAction("Skill", index);
                });
            }
        }

        private void SetupItemUI()
        {
            var itemList = PlayerTeam.Instance.GetItemList();
            for (int i = 0; i < itemList.Count; i++)
            {
                // Setup item element base Item List
                itemElement[i].SetupSkillElement(itemList[i].ItemData.ItemIcon, itemList[i].Quantity.ToString(), itemList[i].ItemData.ItemInformation);

                //Attach event
                var index = i;
                itemElement[i].Button.onClick.AddListener(() =>
                {
                    playerCombatStateMachine.GetItemIndex(itemList[index].ItemData);
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
