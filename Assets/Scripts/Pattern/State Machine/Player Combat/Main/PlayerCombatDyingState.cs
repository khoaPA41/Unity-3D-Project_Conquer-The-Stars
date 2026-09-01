using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatDyingState : PlayerCombatBaseState
    {
        private readonly int DyingAnimationHash = Animator.StringToHash("Dying");
        private readonly string DyingTag = "Dying";
        private float _normalizedTime;
        private float _prevTime;
        public PlayerCombatDyingState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.PlayerSetupUI.InactiveCharacterHUD();
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(DyingAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, DyingTag);
            if (_normalizedTime > _prevTime && _normalizedTime >= .9 && _normalizedTime <= 1f)
            {
                playerCombatStateMachine.IsFinished = true;
                playerCombatStateMachine.PooledObject.Release();
            }

            _prevTime = _normalizedTime;
        }

        public override void Exit()
        {
        }
    }
}
