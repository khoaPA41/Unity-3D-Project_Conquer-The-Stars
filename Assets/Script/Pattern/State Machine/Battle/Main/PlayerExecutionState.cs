using System.Collections;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerExecutionState : BattleBaseState
    {

        public PlayerExecutionState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            // Debug.Log("Selected");
            battleStateMachine.InputReader.EnterTargetAction += EnterTarget;
            battleStateMachine.PlayerCombatStateMachine.IsFinished = false;
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            battleStateMachine.InputReader.EnterTargetAction -= EnterTarget;
        }

        private void EnterTarget()
        {
            battleStateMachine.StartCoroutine(WaitToEndAttack());

        }

        private IEnumerator WaitToEndAttack()
        {
            battleStateMachine.PlayerCombatStateMachine.Target = battleStateMachine.PlayerTargeter.currentTarget;
            battleStateMachine.PlayerCombatStateMachine.SwitchAttackState(battleStateMachine.AttackIndexSelected);

            yield return new WaitUntil(() => battleStateMachine.PlayerCombatStateMachine.IsFinished == true);
            battleStateMachine.SwitchResolve();
        }

    }
}
