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
        private bool isUseItem;
        public PlayerCombatUseItemState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            _normalizedTime = 0f;
            isUseItem = false;
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(_useItemAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            if (isUseItem) return;

            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, _useItemAnimationTag);

            if (_normalizedTime < .9f) return;

            isUseItem = true;
            UseItem();

            playerCombatStateMachine.IsFinished = true;
            playerCombatStateMachine.ReturnCombatIdle();
        }

        public override void Exit()
        {
        }

        private void UseItem()
        {
            playerCombatStateMachine.BuffManager.ApplyBuff();
        }
    }
}