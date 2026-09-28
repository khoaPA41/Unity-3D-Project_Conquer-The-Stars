using System.Collections;
using ConquerTheStars.Fight.Target;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class EnemyTurnState : BattleBaseState
    {
        private static WaitForSecondsRealtime _waitToEnemySetup = new WaitForSecondsRealtime(3f);

        public EnemyTurnState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            // Xuat hien hieu ung
            battleStateMachine.StartCoroutine(WaitToEnemySetup());
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {

        }

        private IEnumerator WaitToEnemySetup()
        {
            GetTarget();
            battleStateMachine.EnemyStateMachine.HighlightCurrentTurn.Highlight();
            battleStateMachine.EnemyStateMachine.Target = battleStateMachine.EnemyTargeter.CurrentTarget; // Get Current Target form select Target state
            battleStateMachine.EnemyStateMachine.SwitchIdle();
            battleStateMachine.EnemyTargeter.CurrentTarget.GetComponent<PlayerCombatStateMachine>().ReturnDefenseIdle();

            yield return _waitToEnemySetup;
            battleStateMachine.SwitchState(battleStateMachine.EnemyExecuted);
        }

        private void GetTarget()
        {
            var selected = battleStateMachine.EnemyStateMachine.EnemyEvaluation.GetBestTarget(battleStateMachine.TeamController.PlayerTeam);
            battleStateMachine.EnemyTargeter.CurrentTarget = selected.GetComponent<Target>();

            // var selected = evaluation.GetBestTarget(playerTeam);

#if UNITY_EDITOR
var details = battleStateMachine.EnemyStateMachine.EnemyEvaluation.EvaluateDetailed(battleStateMachine.TeamController.PlayerTeam);
foreach (var d in details)
{
    string mark = (d.Target == selected) ? " << SELECTED" : "";
    Debug.Log($"[AI] {d.Target.name}  HP:{d.Hp:F2} Thr:{d.Threat:F2} Def:{d.Defense:F2} Total:{d.Total:F2}{mark}");
}
#endif
        }
    }
}
