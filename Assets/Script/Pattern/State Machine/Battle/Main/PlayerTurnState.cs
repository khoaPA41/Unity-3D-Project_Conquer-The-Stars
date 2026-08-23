using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerTurnState : BattleBaseState
    {
        public PlayerTurnState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            // Camera ///

            battleStateMachine.StartCoroutine(WaitBitTime());

        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {

        }

        private IEnumerator WaitBitTime()
        {
            battleStateMachine.PlayerCombatStateMachine.SwitchState(battleStateMachine.PlayerCombatStateMachine.PlayerIdleState); // Change Player State

            yield return new WaitForSecondsRealtime(2f);

            battleStateMachine.SwitchState(battleStateMachine.PlayerSelectSkillTurn);
        }
    }
}
