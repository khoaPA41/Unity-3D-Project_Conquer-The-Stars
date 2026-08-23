using ConquerTheStars.Pattern.StateMachine.Base;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.Enemy
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine enemyStateMachine;
        protected EnemyBaseState(EnemyStateMachine enemyStateMachine)
        {
            this.enemyStateMachine = enemyStateMachine;
        }


        protected bool MoveToTarget(float deltaTime)
        {
            var targetPos = enemyStateMachine.Target.transform.position;
            var currentPos = enemyStateMachine.transform.position;
            var offset = targetPos - currentPos;
            offset.y = 0f;

            if (offset.sqrMagnitude < 1f)
            {
                return true;
            }
            var dirToTarget = offset.normalized;

            enemyStateMachine.CharacterController.Move(deltaTime * enemyStateMachine.Speed * dirToTarget);
            return false;
        }

        protected void MoveBack(float deltaTime)
        {
            var targetPos = enemyStateMachine.EnemyStartPosition;
            var currentPos = enemyStateMachine.transform.position;
            var offset = targetPos - currentPos;
            offset.y = 0f;
            if (offset.sqrMagnitude <= 0.01f)
            {
                return;
            }
            var dirToTarget = offset.normalized;
            enemyStateMachine.CharacterController.Move(deltaTime * enemyStateMachine.Speed * dirToTarget);
        }
    }
}
