using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatGetHitState : PlayerCombatBaseState
    {
        private readonly int GetHitAnimationHash = Animator.StringToHash("GetHit");
        private readonly string GetHitAnimationTag = "GetHit";

        private float _normalizedTime;
        private float _prevTime;

        public PlayerCombatGetHitState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            _prevTime = 0f;
            _normalizedTime = 0f;
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(GetHitAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, GetHitAnimationTag);
            if (_normalizedTime > _prevTime && _normalizedTime > .8f && _normalizedTime <= 1f)
            {
                playerCombatStateMachine.IsFinished = true;
                playerCombatStateMachine.ReturnIdle();
            }
            _prevTime = _normalizedTime;
        }

        public override void Exit()
        {
            playerCombatStateMachine.IsFinished = false;
        }
    }
}
