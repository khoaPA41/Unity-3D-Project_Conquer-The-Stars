using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyGetHitState : EnemyBaseState
    {
        private readonly int _getHitAnimationHash = Animator.StringToHash("GetHit");
        private readonly string _getHitTag = "GetHit";
        private float _normalizedTime;
        private float _prevTime;

        public EnemyGetHitState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime(_getHitAnimationHash, enemyStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(enemyStateMachine.Animator, _getHitTag);
            if (_normalizedTime > _prevTime && _normalizedTime >= .9)
            {
                enemyStateMachine.SwitchIdle();
            }

            _prevTime = _normalizedTime;
        }

        public override void Exit()
        {
        }
    }
}
