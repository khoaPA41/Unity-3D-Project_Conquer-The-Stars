using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly int MoveAnimationHash = Animator.StringToHash("Move");

        private readonly int AttackAnimationHash = Animator.StringToHash("Attack");
        private readonly string AttackTagHash = "Attack";

        private float _normalizedTime;
        private float _prevTime;
        private bool _isActiveAnimation;
        public EnemyAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {
            _isActiveAnimation = false;
            _prevTime = 0f;
            _normalizedTime = 0f;
            enemyStateMachine.Animator.CrossFadeInFixedTime(MoveAnimationHash, enemyStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            if (_isActiveAnimation)
            {
                _normalizedTime = NormalizedTime(enemyStateMachine.Animator, AttackTagHash);
                if (_normalizedTime > _prevTime && _normalizedTime >= .9)
                {
                    enemyStateMachine.IsFinished = true;
                    enemyStateMachine.SwitchIdle();
                }
                _prevTime = _normalizedTime;
                return;
            }

            if (MoveToTarget(deltaTime))
            {
                if (!_isActiveAnimation)
                {
                    _isActiveAnimation = true;
                    enemyStateMachine.Animator.CrossFadeInFixedTime(AttackAnimationHash, enemyStateMachine.AnimationCrossFade);
                }
            }
        }

        public override void Exit()
        {
            _isActiveAnimation = false;
        }
    }
}
