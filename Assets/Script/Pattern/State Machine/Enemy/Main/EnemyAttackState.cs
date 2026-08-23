using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly int AttackAnimationHash = Animator.StringToHash("Attack");
        private readonly string AttackTagHash = "Attack";

        private float normalizedTime;
        private float prevTime;
        private bool isActiveAnimation;
        public EnemyAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            if (isActiveAnimation)
            {
                normalizedTime = NormalizedTime(enemyStateMachine.Animator, AttackTagHash);
                if (normalizedTime > prevTime && normalizedTime >= .9)
                {
                    enemyStateMachine.IsFinished = true;
                    enemyStateMachine.SwitchIdle();
                }
                prevTime = normalizedTime;
                return;
            }

            if (MoveToTarget(deltaTime))
            {
                if (!isActiveAnimation)
                {
                    isActiveAnimation = true;
                    enemyStateMachine.Animator.CrossFadeInFixedTime(AttackAnimationHash, enemyStateMachine.AnimationCrossFade);
                }
            }
        }

        public override void Exit()
        {
            isActiveAnimation = false;
        }
    }
}
