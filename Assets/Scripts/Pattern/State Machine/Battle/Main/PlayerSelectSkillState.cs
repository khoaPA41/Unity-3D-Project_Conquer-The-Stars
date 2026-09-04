using System.Collections;
using ConquerTheStars.Factory.Item;
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
            battleStateMachine.IsWaitingCameraBlend = false;


            // Listen attack and use item event
            battleStateMachine.PlayerCombatStateMachine.PlayerExecuteAction += PlayerExecutedAction;
            battleStateMachine.PlayerCombatStateMachine.PlayerUseItem += PlayerUseItem;

            /*Show Skill Selection UI*/
            battleStateMachine.PlayerCombatStateMachine.PlayerSetupSkillUI.AppearSkillUI();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            battleStateMachine.PlayerCombatStateMachine.PlayerExecuteAction -= PlayerExecutedAction;
            battleStateMachine.PlayerCombatStateMachine.PlayerUseItem -= PlayerUseItem;
        }

        private void PlayerExecutedAction(string listName, int index)
        {
            if (listName == "Skill")
            {
                var currentMana = battleStateMachine.CurrentTurn.CurrentMana;
                var manaRequired = battleStateMachine.PlayerCombatStateMachine.GetManaRequired(listName, index);

                if (currentMana < manaRequired)
                {
                    return;
                }
                battleStateMachine.CurrentTurn.SubtractMana(manaRequired);
            }
            battleStateMachine.StartCoroutine(WaitForCameraBlendFinishedForSelectTarget());
        }

        private void PlayerUseItem(ItemType itemType, int index)
        {
            battleStateMachine.StartCoroutine(WaitForCameraBlendFinishedForSelectAlly());
        }

        private IEnumerator WaitForCameraBlendFinishedForSelectAlly()
        {
            battleStateMachine.PlayerCombatStateMachine.PlayerSetupSkillUI.DisappearSkillUI();
            battleStateMachine.PlayerCombatStateMachine.InactiveCamera();

            yield return new WaitUntil(() => battleStateMachine.IsWaitingCameraBlend == true);

            battleStateMachine.SwitchSelectAlly();
        }

        private IEnumerator WaitForCameraBlendFinishedForSelectTarget()
        {
            battleStateMachine.PlayerCombatStateMachine.PlayerSetupSkillUI.DisappearSkillUI();
            battleStateMachine.PlayerCombatStateMachine.InactiveCamera();

            yield return new WaitUntil(() => battleStateMachine.IsWaitingCameraBlend == true);

            battleStateMachine.SwitchState(battleStateMachine.PlayerSelectTargetTurn);
        }
    }
}
