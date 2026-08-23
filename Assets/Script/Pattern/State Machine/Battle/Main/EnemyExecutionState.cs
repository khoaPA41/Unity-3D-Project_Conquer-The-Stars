using System.Collections;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class EnemyExecutionState : BattleBaseState
    {
        public EnemyExecutionState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            EnterTarget();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        private void EnterTarget()
        {
            battleStateMachine.StartCoroutine(WaitToEndAttack());
        }

        private IEnumerator WaitToEndAttack()
        {
            battleStateMachine.EnemyStateMachine.SwitchAttackState();
            yield return new WaitUntil(() => battleStateMachine.EnemyStateMachine.IsFinished = true);
            battleStateMachine.SwitchResolve();
        }


    }
}
