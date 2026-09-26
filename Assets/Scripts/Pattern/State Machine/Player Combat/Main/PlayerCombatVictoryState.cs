using System;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatVictoryState : PlayerCombatBaseState
    {
        private readonly int _victoryAnimationHash = Animator.StringToHash("Victory");
        public PlayerCombatVictoryState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.PlayerSetupUI.ReturnToPool();
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(_victoryAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}
