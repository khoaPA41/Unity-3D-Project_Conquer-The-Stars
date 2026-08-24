using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public class EnemyGetHitState : EnemyBaseState
    {
        private readonly int GetHitAnimationHash = Animator.StringToHash("GetHit");
        private readonly string GetHitTag = "GetHit";
        private float normalizedTime;
        private float prevTime;
        public EnemyGetHitState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime(GetHitAnimationHash, enemyStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            normalizedTime = NormalizedTime(enemyStateMachine.Animator, GetHitTag);
            if (normalizedTime > prevTime && normalizedTime >= .9)
            {
                // enemyStateMachine.IsFinished = true;
                // CheckHealth();
                enemyStateMachine.SwitchIdle();
            }

            prevTime = normalizedTime;
        }

        public override void Exit()
        {
            // enemyStateMachine.IsFinished = false;
        }

        private void CheckHealth()
        {
            if (enemyStateMachine.CharacterStatsManagers.IsDeath)
            {
                enemyStateMachine.SwitchDyingState();
                return;
            }
            enemyStateMachine.SwitchIdle();
        }
    }
}
