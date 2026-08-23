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
            battleStateMachine.PlayerExecuteAction += PlayerExecutedAction; // Event click button

            /*Setup Skill Selection UI*/
            UIManagers.Instance.SetupSkillSelectionUI(battleStateMachine.PlayerCombatStateMachine.AttackData.AttackIcon, battleStateMachine.PlayerCombatStateMachine.AttackData.AttackInformation);
            UIManagers.Instance.AppearSkillSelection();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {

            battleStateMachine.PlayerExecuteAction -= PlayerExecutedAction; // Event click button

            UIManagers.Instance.DisappearSkillSelection();
        }

        private void PlayerExecutedAction(int index)
        {
            battleStateMachine.AttackIndexSelected = index;
            battleStateMachine.SwitchState(battleStateMachine.PlayerSelectTargetTurn);
        }
    }
}
