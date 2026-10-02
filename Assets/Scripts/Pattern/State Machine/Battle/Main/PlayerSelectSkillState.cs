using System.Collections;
using ConquerTheStars.Factory.Item;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerSelectSkillState : BattleBaseState
    {
        private PlayerCombatStateMachine _selectPlayer;
        public PlayerSelectSkillState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            _selectPlayer = battleStateMachine.PlayerCombatStateMachine;
            battleStateMachine.IsWaitingCameraBlend = false;


            // Listen attack and use item event
            _selectPlayer.PlayerExecuteAction += PlayerExecutedAction;
            _selectPlayer.PlayerUseItem += PlayerUseItem;

            /*Show Skill Selection UI*/
            _selectPlayer.PlayerSetupSkillUI.AppearSkillUI();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            if (_selectPlayer != null)
            {
                _selectPlayer.PlayerExecuteAction -= PlayerExecutedAction;
                _selectPlayer.PlayerUseItem -= PlayerUseItem;
            }

            _selectPlayer = null;
        }

        private void PlayerExecutedAction(string listName, int index)
        {
            if (listName == "Skill")
            {
                var currentMana = battleStateMachine.CurrentTurn.CurrentMana;
                var manaRequired = _selectPlayer.GetManaRequired(listName, index);

                if (currentMana < manaRequired)
                {
                    return;
                }
                battleStateMachine.CurrentTurn.SubtractMana(manaRequired);
            }
            battleStateMachine.StartCoroutine(WaitForCameraBlendFinishedForSelectTarget());
        }

        private void PlayerUseItem()
        {
            if (_selectPlayer.ItemData.ItemType == ItemType.Revive && !battleStateMachine.TeamController.IsSomeOneInPlayerDead()) return;
            battleStateMachine.StartCoroutine(WaitForCameraBlendFinishedForSelectAlly());
            //Will appear UI warning for player know no one die, can't use rivie item
        }

        private IEnumerator WaitForCameraBlendFinishedForSelectAlly()
        {
            _selectPlayer.PlayerSetupSkillUI.DisappearSkillUI();
            _selectPlayer.InactiveCamera();

            yield return new WaitUntil(() => battleStateMachine.IsWaitingCameraBlend == true);

            battleStateMachine.SwitchSelectAlly();
        }

        private IEnumerator WaitForCameraBlendFinishedForSelectTarget()
        {
            _selectPlayer.PlayerSetupSkillUI.DisappearSkillUI();
            _selectPlayer.InactiveCamera();

            yield return new WaitUntil(() => battleStateMachine.IsWaitingCameraBlend == true);

            battleStateMachine.SwitchState(battleStateMachine.PlayerSelectTargetTurn);
        }
    }
}
