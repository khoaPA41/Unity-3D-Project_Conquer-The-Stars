using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatDodgeState : PlayerCombatBaseState
    {
        private readonly int DodgeAnimationHash = Animator.StringToHash("Dodge");
        private readonly string DodgeAnimationTag = "Dodge";

        private float normalizedTime;
        private float prevTime;
        public PlayerCombatDodgeState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(DodgeAnimationHash, playerCombatStateMachine.AnimationCrossFade);
            playerCombatStateMachine.CharacterStatsManagers.SetImmortal(true);
        }

        public override void Tick(float deltaTime)
        {
            normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, DodgeAnimationTag);

            if (normalizedTime > prevTime && normalizedTime > .3f)
            {
                playerCombatStateMachine.CharacterStatsManagers.SetImmortal(false);

            }

            if (normalizedTime > prevTime && normalizedTime > .9f)
            {
                playerCombatStateMachine.ReturnIdle();
            }

            normalizedTime = prevTime;
        }

        public override void Exit()
        {
        }
    }
}
