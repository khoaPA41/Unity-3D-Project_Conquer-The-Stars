using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyDyingState : EnemyBaseState
    {
        private readonly int DyingAnimationHash = Animator.StringToHash("Dying");
        private readonly string DyingTag = "Dying";
        private float normalizedTime;
        private float prevTime;
        public EnemyDyingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {

            enemyStateMachine.Animator.CrossFadeInFixedTime(DyingAnimationHash, enemyStateMachine.AnimationCrossFade);

        }

        public override void Tick(float deltaTime)
        {
            normalizedTime = NormalizedTime(enemyStateMachine.Animator, DyingTag);
            if (normalizedTime > prevTime && normalizedTime >= .9)
            {
                // enemyStateMachine.IsFinished = true;
                enemyStateMachine.PooledObject.Release();
            }

            prevTime = normalizedTime;
        }

        public override void Exit()
        {
        }
    }
}
