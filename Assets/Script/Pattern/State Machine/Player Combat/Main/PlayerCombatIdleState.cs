using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatIdleState : PlayerCombatBaseState
    {
        private readonly int CombatIdleAnimationHash = Animator.StringToHash("Combat_Idle");

        public PlayerCombatIdleState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            // Debug.Log("Idle");
            // playerCombatStateMachine.PlayerStartPosition = playerCombatStateMachine.transform.position;
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
