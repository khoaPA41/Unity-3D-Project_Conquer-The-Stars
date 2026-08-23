using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly int AttackAnimationHash = Animator.StringToHash("Attack");
        private readonly string AttackTagHash = "Attack";

        private float normalizedTime;
        public EnemyAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime(AttackAnimationHash, enemyStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            normalizedTime = NormalizedTime(enemyStateMachine.Animator, AttackTagHash);
            if (normalizedTime >= .9)
            {
                enemyStateMachine.IsFinished = true;
                enemyStateMachine.SwitchIdle();
            }
            return;
        }

        public override void Exit()
        {
            enemyStateMachine.IsFinished = false;

        }
    }
}
