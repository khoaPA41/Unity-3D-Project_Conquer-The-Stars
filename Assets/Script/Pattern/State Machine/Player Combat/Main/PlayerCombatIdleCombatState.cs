using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatIdleCombatState : PlayerCombatBaseState
    {
        private readonly int CombatIdleAnimationHash = Animator.StringToHash("Combat_Idle");

        public PlayerCombatIdleCombatState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(CombatIdleAnimationHash, playerCombatStateMachine.AnimationCrossFade);
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
