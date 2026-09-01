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
            // battleStateMachine.PlayerTargeter.SetupTargetCamera(battleStateMachine.PlayerCombatStateMachine.CinemachineTargetGroup);
            battleStateMachine.PlayerCombatStateMachine.ActiveCamera();
            battleStateMachine.PlayerCombatStateMachine.SwitchState(battleStateMachine.PlayerCombatStateMachine.PlayerIdleState);

            yield return new WaitForSecondsRealtime(2f);

            battleStateMachine.SwitchState(battleStateMachine.PlayerSelectSkillTurn);
        }
    }
}
