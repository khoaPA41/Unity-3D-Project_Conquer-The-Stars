using ConquerTheStars.Factory.Item;
using ConquerTheStars.Fight;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatUseItemState : PlayerCombatBaseState
    {
        private readonly int _useItemAnimationHash = Animator.StringToHash("UseItem");
        private readonly string _useItemAnimationTag = "UseItem";

        private float _normalizedTime;
        private float _prevTime;
        private bool isUseItem;
        public PlayerCombatUseItemState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            _prevTime = 0f;
            _normalizedTime = 0f;
            isUseItem = false;
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(_useItemAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, _useItemAnimationTag);

            if (_normalizedTime > _prevTime && _normalizedTime >= .9f && _normalizedTime <= 1f)
            {
                playerCombatStateMachine.IsFinished = true;
                if (!isUseItem)
                {
                    UseItem();
                    isUseItem = true;
                }
                playerCombatStateMachine.ReturnCombatIdle();
            }
            _prevTime = _normalizedTime;
        }

        public override void Exit()
        {
        }

        private void UseItem()
        {
            // IItem item = ItemFactory.CreateItem(playerCombatStateMachine.ItemType);
            // item.Use(playerCombatStateMachine, PlayerTeam.Instance.GetItemData(playerCombatStateMachine.ItemIndex));
            playerCombatStateMachine.BuffManager.ApplyBuff();
        }
    }
}