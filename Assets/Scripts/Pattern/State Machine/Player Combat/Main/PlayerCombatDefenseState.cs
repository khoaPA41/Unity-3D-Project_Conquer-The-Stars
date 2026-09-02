using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatDefenseState : PlayerCombatBaseState
    {
        private readonly int DefenseIdleAnimationHash = Animator.StringToHash("Defense_Idle");

        public PlayerCombatDefenseState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(DefenseIdleAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            MoveBack(deltaTime);
        }

        public override void Exit()
        {
            playerCombatStateMachine.InactiveCamera();
        }
    }
}
