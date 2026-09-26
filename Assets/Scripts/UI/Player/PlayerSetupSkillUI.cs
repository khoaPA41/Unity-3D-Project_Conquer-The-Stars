using System.Collections.Generic;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.UI.Player;
using UnityEngine;

namespace ConquerTheStars.Fight.Player
{
    [RequireComponent(typeof(PlayerCombatStateMachine))]
    public class PlayerSetupSkillUI : MonoBehaviour
    {
        private readonly int _skillSelectionAppearAnimationHash = Animator.StringToHash("Appear");
        private readonly int _skillSelectionDisappearAnimationHash = Animator.StringToHash("Disappear");

        [Header("Canvas")]
        [SerializeField]
        private Canvas _playerCanvas;

        [Header("Selection Object")]
        [SerializeField]
        private RectTransform _selection_I;
        [SerializeField] private RectTransform _selection_II;
        [SerializeField] private RectTransform _selection_III;

        [Header("Skill Selection")]
        [SerializeField]
        private List<SkillElement> _attackElement;
        [SerializeField] private List<SkillElement> _skillElement;
        [SerializeField] private List<SkillElement> _itemElement;

        [Header("Animator")]
        [SerializeField]
        private Animator _skillAnimator;

        [Header("StateMachine")]
        [SerializeField] private PlayerCombatStateMachine _playerCombatStateMachine;

        private Camera _mainCamera;

        private bool _isInitialized;

        private void OnEnable()
        {
            _mainCamera = Camera.main;
            if (_playerCombatStateMachine != null && PlayerTeam.Instance != null)
            {
                if (!_isInitialized)
                {
                    SetupAttackUI();
                    SetupSkillUI();
                    SetupItemUI();
                    _isInitialized = true;
                }

                if (_mainCamera != null)
                {
                    SetupCamera();
                }
            }
        }


        private void SetupAttackUI()
        {
            for (int i = 0; i < _playerCombatStateMachine.AttackData.AttackIcon.Count; i++)
            {
                // Setup attack element base AttackData
                _attackElement[i].SetupSkillElement(_playerCombatStateMachine.AttackData.AttackIcon[i],
                _playerCombatStateMachine.AttackData.Attack[i],
                _playerCombatStateMachine.AttackData.AttackInformation[i]
                );

                //Attach event
                var index = i;
                _attackElement[i].Button.onClick.AddListener(() =>
                {
                    _playerCombatStateMachine.GetIndexAction("Attack", index);
                });
            }
        }


        private void SetupSkillUI()
        {
            for (int i = 0; i < _playerCombatStateMachine.AttackData.SkillIcon.Count; i++)
            {
                // Setup skill element base AttackData

                _skillElement[i].SetupSkillElement(_playerCombatStateMachine.AttackData.SkillIcon[i],
                _playerCombatStateMachine.AttackData.Skill[i],
                _playerCombatStateMachine.AttackData.SkillInformation[i]);

                //Attach event
                var index = i;
                _skillElement[i].Button.onClick.AddListener(() =>
                {
                    _playerCombatStateMachine.GetIndexAction("Skill", index);
                });
            }
        }

        private void SetupItemUI()
        {
            var itemList = PlayerTeam.Instance.GetItemList();
            for (int i = 0; i < itemList.Count; i++)
            {
                // Setup item element base Item List
                _itemElement[i].SetupSkillElement(itemList[i].ItemData.ItemIcon, itemList[i].Quantity.ToString(), itemList[i].ItemData.ItemInformation);

                //Attach event
                var index = i;
                _itemElement[i].Button.onClick.AddListener(() =>
                {
                    _playerCombatStateMachine.GetItemIndex(itemList[index].ItemData);
                });
            }
        }

        private void SetupCamera()
        {
            _playerCanvas.worldCamera = _mainCamera;
        }

        public void AppearSkillUI()
        {
            _playerCanvas.gameObject.SetActive(true);
            _skillAnimator.SetTrigger(_skillSelectionAppearAnimationHash);
        }
        public void DisappearSkillUI()
        {
            _skillAnimator.SetTrigger(_skillSelectionDisappearAnimationHash);
            _playerCanvas.gameObject.SetActive(false);
        }

        public void InActiveSkillUI()
        {
            _playerCanvas.gameObject.SetActive(false);
        }
    }

}
