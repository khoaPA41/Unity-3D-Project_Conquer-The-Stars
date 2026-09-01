using System.Collections;
using ConquerTheStars.Fight.Target;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class EnemyTurnState : BattleBaseState
    {
        public EnemyTurnState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            // Chuyen camera ve binh thuong
            // Logic xac dinh muc tieu

            // GetTarget();
            // Xuat hien hieu ung
            battleStateMachine.StartCoroutine(Wait());
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {

        }

        private IEnumerator Wait()
        {
            GetTarget();
            battleStateMachine.EnemyStateMachine.Target = battleStateMachine.EnemyTargeter.currentTarget; // Get Current Target form select Target state

            battleStateMachine.EnemyStateMachine.SwitchIdle();
            battleStateMachine.EnemyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>().ReturnDefenseIdle();

            yield return new WaitForSecondsRealtime(3f);
            battleStateMachine.SwitchState(battleStateMachine.EnemyExecuted);
        }

        private void GetTarget()
        {
            battleStateMachine.EnemyTargeter.currentTarget = battleStateMachine.EnemyStateMachine.EnemyEvaluation.GetBestTarget(battleStateMachine.TeamController.PlayerTeam).GetComponent<Target>();
        }
    }
}
