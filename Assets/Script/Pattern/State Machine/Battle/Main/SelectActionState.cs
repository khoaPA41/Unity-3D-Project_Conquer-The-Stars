using System.Threading.Tasks;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class SelectActionState : BattleBaseState
    {
        public SelectActionState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            battleStateMachine.PlayerExecuteAction += PlayerExecutedAction; // Event click button
            battleStateMachine.InputReader.NextTargetAction += battleStateMachine.PlayerTargeter.ChooseNextTarget;
            battleStateMachine.InputReader.PreviousTargetAction += battleStateMachine.PlayerTargeter.ChoosePrevTarget;
            battleStateMachine.PlayerCombatStateMachine.SwitchState(battleStateMachine.PlayerCombatStateMachine.PlayerIdleState); // Change Player State

            /*Setup Skill Selection UI*/
            UIManagers.Instance.SetupSkillSelectionUI(battleStateMachine.PlayerCombatStateMachine.AttackData.AttackIcon, battleStateMachine.PlayerCombatStateMachine.AttackData.AttackInformation);
            UIManagers.Instance.AppearSkillSelection();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            battleStateMachine.PlayerExecuteAction -= PlayerExecutedAction;
            battleStateMachine.InputReader.NextTargetAction -= battleStateMachine.PlayerTargeter.ChooseNextTarget;
            battleStateMachine.InputReader.PreviousTargetAction -= battleStateMachine.PlayerTargeter.ChoosePrevTarget;
            UIManagers.Instance.DisappearSkillSelection();
        }
    }
}
