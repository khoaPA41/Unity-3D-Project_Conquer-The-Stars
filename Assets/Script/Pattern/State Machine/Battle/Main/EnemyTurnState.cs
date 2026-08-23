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
            battleStateMachine.EnemyStateMachine.SwitchIdle(); // Change Enemy State
            yield return new WaitForSecondsRealtime(3f);
            battleStateMachine.SwitchState(battleStateMachine.EnemyExecuted);
        }
    }
}
