using ConquerTheStars.Factory.Item;
using ConquerTheStars.Fight;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatUseItemState : PlayerCombatBaseState
    {
        private readonly int UseItemAnimationHash = Animator.StringToHash("UseItem");
        private readonly string UseItemAnimationTag = "UseItem";

        private float _normalizedTime;
        private float _prevTime;
        public PlayerCombatUseItemState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            _prevTime = 0f;
            _normalizedTime = 0f;
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(UseItemAnimationHash, playerCombatStateMachine.AnimationCrossFade);
            UseItem();
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, UseItemAnimationTag);

            if (_normalizedTime > _prevTime && _normalizedTime >= .9f && _normalizedTime <= 1f)
            {
                playerCombatStateMachine.IsFinished = true;
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