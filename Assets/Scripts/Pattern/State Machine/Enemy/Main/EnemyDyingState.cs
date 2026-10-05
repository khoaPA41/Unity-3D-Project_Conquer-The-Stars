using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyDyingState : EnemyBaseState
    {
        private readonly int _dyingAnimationHash = Animator.StringToHash("Dying");
        private readonly string _dyingTag = "Dying";
        private float _normalizedTime;
        private float _prevTime;

        public EnemyDyingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime(_dyingAnimationHash, enemyStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(enemyStateMachine.Animator, _dyingTag);
            if (_normalizedTime > _prevTime && _normalizedTime >= .9)
            {
                enemyStateMachine.PooledObject.Release();
            }

            _prevTime = _normalizedTime;
        }

        public override void Exit()
        {
        }
    }
}
