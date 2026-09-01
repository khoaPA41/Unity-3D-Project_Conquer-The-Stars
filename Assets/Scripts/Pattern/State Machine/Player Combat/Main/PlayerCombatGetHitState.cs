using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatGetHitState : PlayerCombatBaseState
    {
        private readonly int GetHitAnimationHash = Animator.StringToHash("GetHit");
        private readonly string GetHitAnimationTag = "GetHit";

        private float _normalizeTime;
        private float _prevTime;

        public PlayerCombatGetHitState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(GetHitAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            _normalizeTime = NormalizedTime(playerCombatStateMachine.Animator, GetHitAnimationTag);
            if (_normalizeTime > _prevTime && _normalizeTime > .8f && _normalizeTime <= 1f)
            {
                playerCombatStateMachine.IsFinished = true;
                playerCombatStateMachine.ReturnIdle();
            }
            _prevTime = _normalizeTime;
        }

        public override void Exit()
        {
            playerCombatStateMachine.IsFinished = false;
        }
    }
}
