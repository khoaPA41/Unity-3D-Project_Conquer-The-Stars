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
            // Prepare for player turn
            battleStateMachine.PlayerCombatStateMachine.HighlightCurrentTurn.Highlight();
            battleStateMachine.PlayerCombatStateMachine.ActiveCamera();
            battleStateMachine.PlayerCombatStateMachine.SwitchState(battleStateMachine.PlayerCombatStateMachine.PlayerIdleState);

            yield return new WaitUntil(() => battleStateMachine.IsWaitingCameraBlend == true);

            battleStateMachine.SwitchState(battleStateMachine.PlayerSelectSkillTurn);
        }
    }
}
