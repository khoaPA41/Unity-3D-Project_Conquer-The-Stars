using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatBlockState : PlayerCombatBaseState
    {
        private readonly int _blockAnimationHash = Animator.StringToHash("Block");
        private readonly string _blockAnimationTag = "Block";
        private float _normalizedTime;
        private float _prevTime;

        public PlayerCombatBlockState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            _prevTime = 0f;
            _normalizedTime = 0f;
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(_blockAnimationHash, playerCombatStateMachine.AnimationCrossFade);
            playerCombatStateMachine.CharacterStatsManagers.SetIsBlock(true);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, _blockAnimationTag);

            if (_normalizedTime > _prevTime && _normalizedTime > .3f)
            {
                playerCombatStateMachine.CharacterStatsManagers.SetIsBlock(false);
            }

            if (_normalizedTime > _prevTime && _normalizedTime > .9f && _normalizedTime <= 1f)
            {
                playerCombatStateMachine.ReturnCombatIdle();
            }

            _prevTime = _normalizedTime;
        }

        public override void Exit()
        {
        }
    }
}
