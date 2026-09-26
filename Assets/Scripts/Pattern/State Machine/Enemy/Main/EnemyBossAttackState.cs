using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyBossAttackState : EnemyBaseState
    {
        private readonly int _moveAnimationHash = Animator.StringToHash("Move");
        private readonly string _attackTagHash = "Attack";
        private string _attackName;
        private float _normalizedTime;
        private float _prevTime;
        private bool _isActiveAnimation;

        public EnemyBossAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {
            _isActiveAnimation = false;
            _prevTime = 0f;
            _normalizedTime = 0f;
            _attackName = enemyStateMachine.EnemyAttack.AttackNameList[Random.Range(0, enemyStateMachine.EnemyAttack.AttackNameList.Count)];
            enemyStateMachine.Animator.CrossFadeInFixedTime(_moveAnimationHash, enemyStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            if (_isActiveAnimation)
            {
                _normalizedTime = NormalizedTime(enemyStateMachine.Animator, _attackTagHash);
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
                    enemyStateMachine.Animator.CrossFadeInFixedTime(_attackName, enemyStateMachine.AnimationCrossFade);
                }
            }
        }

        public override void Exit()
        {
        }
    }
}