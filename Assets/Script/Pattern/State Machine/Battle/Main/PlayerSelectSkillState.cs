using System.Collections;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerSelectSkillState : BattleBaseState
    {
        public PlayerSelectSkillState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            battleStateMachine.PlayerCombatStateMachine.PlayerExecuteAction += PlayerExecutedAction;

            /*Setup Skill Selection UI*/
            battleStateMachine.PlayerCombatStateMachine.PlayerSetupSkillUI.AppearSkillUI();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            battleStateMachine.PlayerCombatStateMachine.PlayerExecuteAction -= PlayerExecutedAction;
        }

        private void PlayerExecutedAction(int index)
        {
            battleStateMachine.StartCoroutine(WaitABit());
        }

        private IEnumerator WaitABit()
        {
            battleStateMachine.PlayerCombatStateMachine.PlayerSetupSkillUI.DisappearSkillUI();
            yield return new WaitForSecondsRealtime(3f);
            battleStateMachine.SwitchState(battleStateMachine.PlayerSelectTargetTurn);
        }
    }
}
