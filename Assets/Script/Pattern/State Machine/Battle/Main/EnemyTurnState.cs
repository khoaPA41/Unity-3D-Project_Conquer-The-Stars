using System.Collections;
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
            battleStateMachine.EnemyStateMachine.SwitchIdle(); // Change Enemy State
            yield return new WaitForSecondsRealtime(3f);
            battleStateMachine.SwitchState(battleStateMachine.EnemyExecuted);
        }

        private void GetTarget()
        {
            var epsilon = 0.001f;
            var scoreList = battleStateMachine.EnemyStateMachine.EnemyEvaluation.Evaluate(battleStateMachine.TeamController.PlayerTeam);
            var score = 0f;
            for (int i = 0; i < scoreList.Count; i++)
            {
                score = Mathf.Max(score, scoreList[i]);
                Debug.Log(score);
            }
            Debug.Log("Final: " + score);
            var target = scoreList.FindIndex(target => Mathf.Abs(target - score) < epsilon);

            battleStateMachine.EnemyTargeter.currentIndex = target;
            // battleStateMachine.EnemyTargeter.GetTarget();
            battleStateMachine.EnemyTargeter.FirstSelected();
        }
    }
}
