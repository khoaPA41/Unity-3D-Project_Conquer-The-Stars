using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyDyingState : EnemyBaseState
    {
        private readonly int DyingAnimationHash = Animator.StringToHash("Dying");
        private readonly string DyingTag = "Dying";
        private float _normalizedTime;
        private float _prevTime;
        public EnemyDyingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime(DyingAnimationHash, enemyStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(enemyStateMachine.Animator, DyingTag);
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
