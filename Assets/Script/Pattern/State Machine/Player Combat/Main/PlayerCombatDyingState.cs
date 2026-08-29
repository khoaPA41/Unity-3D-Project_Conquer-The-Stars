using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatDyingState : PlayerCombatBaseState
    {
        private readonly int DyingAnimationHash = Animator.StringToHash("Dying");
        private readonly string DyingTag = "Dying";
        private float normalizedTime;
        private float prevTime;
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
            normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, DyingTag);
            if (normalizedTime > prevTime && normalizedTime >= .9 && normalizedTime <= 1f)
            {
                playerCombatStateMachine.IsFinished = true;
                playerCombatStateMachine.PooledObject.Release();
            }

            prevTime = normalizedTime;
        }

        public override void Exit()
        {
        }
    }
}
