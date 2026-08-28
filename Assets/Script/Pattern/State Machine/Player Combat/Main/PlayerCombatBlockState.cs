using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatBlockState : PlayerCombatBaseState
    {
        private readonly int BlockAnimationHash = Animator.StringToHash("Block");

        private readonly string BlockAnimationTag = "Block";

        private float normalizedTime;
        private float prevTime;
        public PlayerCombatBlockState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(BlockAnimationHash, playerCombatStateMachine.AnimationCrossFade);
            playerCombatStateMachine.CharacterStatsManagers.SetIsBlock(true);
        }

        public override void Tick(float deltaTime)
        {
            normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, BlockAnimationTag);

            if (normalizedTime > prevTime && normalizedTime > .3f)
            {
                playerCombatStateMachine.CharacterStatsManagers.SetIsBlock(false);
            }

            if (normalizedTime > prevTime && normalizedTime > .9f)
            {
                playerCombatStateMachine.ReturnCombatIdle();
            }

            normalizedTime = prevTime;
        }

        public override void Exit()
        {
        }
    }
}
