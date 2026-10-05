using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatGetHitState : PlayerCombatBaseState
    {
        private readonly int _getHitAnimationHash = Animator.StringToHash("GetHit");
        private readonly string _getHitAnimationTag = "GetHit";
        private float _normalizedTime;
        private float _prevTime;

        public PlayerCombatGetHitState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            _prevTime = 0f;
            _normalizedTime = 0f;
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(_getHitAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, _getHitAnimationTag);
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
