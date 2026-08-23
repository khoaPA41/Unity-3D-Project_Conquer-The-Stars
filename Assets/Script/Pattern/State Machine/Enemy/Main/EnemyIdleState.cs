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
            // Debug.Log("Enemy Idle");

            enemyStateMachine.Animator.CrossFadeInFixedTime(IdleAnimationHash, enemyStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}
