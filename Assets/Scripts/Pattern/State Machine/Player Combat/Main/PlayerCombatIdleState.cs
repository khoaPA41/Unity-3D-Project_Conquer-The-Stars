using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatIdleState : PlayerCombatBaseState
    {
        private readonly int _idleAnimationHash = Animator.StringToHash("Idle");

        public PlayerCombatIdleState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(_idleAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            MoveBack(deltaTime);
        }

        public override void Exit()
        {
        }
    }
}
