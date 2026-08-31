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
            battleStateMachine.PlayerCombatStateMachine.PlayerExecuteAction += PlayerExecutedAction; // Attack
            battleStateMachine.PlayerCombatStateMachine.PlayerUseItem += PlayerUseItem; // Item

            /*Setup Skill Selection UI*/
            battleStateMachine.PlayerCombatStateMachine.PlayerSetupSkillUI.AppearSkillUI();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            battleStateMachine.PlayerCombatStateMachine.PlayerExecuteAction -= PlayerExecutedAction;
            battleStateMachine.PlayerCombatStateMachine.PlayerUseItem -= PlayerUseItem; // Item

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

            battleStateMachine.StartCoroutine(WaitABit());
        }

        private void PlayerUseItem(ItemType itemType, int index)
        {
            battleStateMachine.PlayerCombatStateMachine.SwitchUseItem();
        }

        private IEnumerator WaitABit()
        {
            battleStateMachine.PlayerCombatStateMachine.PlayerSetupSkillUI.DisappearSkillUI();
            yield return new WaitForSecondsRealtime(3f);
            battleStateMachine.SwitchState(battleStateMachine.PlayerSelectTargetTurn);
        }
    }
}
