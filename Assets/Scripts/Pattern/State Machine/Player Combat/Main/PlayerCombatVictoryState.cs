using System;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatVictoryState : PlayerCombatBaseState
    {
        private readonly int VictoryAnimationHash = Animator.StringToHash("Victory");
        public PlayerCombatVictoryState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.PlayerSetupUI.InactiveCharacterHUD();
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(VictoryAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}
