using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        private readonly int IdleAnimationHash = Animator.StringToHash("Idle");
        public EnemyIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime(IdleAnimationHash, enemyStateMachine.AnimationCrossFade);
            // enemyStateMachine.IsFinished = false;
        }

        public override void Tick(float deltaTime)
        {
            MoveBack(deltaTime);
        }

        public override void Exit()
        {
        }
    }
}
