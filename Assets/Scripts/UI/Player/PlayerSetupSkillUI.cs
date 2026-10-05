using System.Collections.Generic;
using ConquerTheStars.Factory.Item;
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
        private PlayerTeam _subscribedTeam;

        private void OnEnable()
        {
            _mainCamera = Camera.main;
            _subscribedTeam = PlayerTeam.Instance;
            if (_playerCombatStateMachine != null && _subscribedTeam != null)
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

            if (_subscribedTeam != null)
                _subscribedTeam.UpdateItemQuantityAction += UpdateItemQuantity;
        }

        private void OnDisable()
        {
            if (_subscribedTeam != null)
                _subscribedTeam.UpdateItemQuantityAction -= UpdateItemQuantity;

            _subscribedTeam = null;
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
            var itemList = _subscribedTeam.GetItemList();
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

        public void UpdateItemQuantity(ItemType itemType, string quantity)
        {
            var itemList = _subscribedTeam.GetItemList();
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].ItemData.ItemType != itemType) continue;
                _itemElement[i].UpdateItemQuantity(quantity);
                break;
            }
        }

        public void RefreshItemQuanity()
        {
            if (_subscribedTeam == null) return;

            var itemList = _subscribedTeam.GetItemList();
            for (int i = 0; i < itemList.Count; i++)
            {
                _itemElement[i].UpdateItemQuantity(itemList[i].Quantity.ToString());
            }
        }

        private void SetupCamera()
        {
            _playerCanvas.worldCamera = _mainCamera;
        }

        public void AppearSkillUI()
        {
            RefreshItemQuanity();
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
