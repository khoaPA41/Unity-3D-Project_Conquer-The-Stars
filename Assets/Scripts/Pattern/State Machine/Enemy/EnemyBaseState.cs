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

            var distance = offset.magnitude;
            if (distance <= 0.1f) return;


            var step = Mathf.Min(enemyStateMachine.Speed * deltaTime, distance);
            enemyStateMachine.CharacterController.Move(offset / distance * step);
        }
    }
}
