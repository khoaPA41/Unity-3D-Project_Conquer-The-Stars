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

    }
}
