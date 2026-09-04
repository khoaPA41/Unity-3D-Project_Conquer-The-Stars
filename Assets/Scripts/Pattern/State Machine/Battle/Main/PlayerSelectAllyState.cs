using System.Collections;
using System.Threading.Tasks;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerSelectAllyState : BattleBaseState
    {
        public PlayerSelectAllyState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            battleStateMachine.PlayerCombatStateMachine.HighlightCurrentTurn.InactiveHighlight();

            /*Select default ally target*/
            battleStateMachine.AllyTargeter.FirstSelected();
            battleStateMachine.AllyTargeter.GetTarget();
            Highlight();

            Selected();

            /*Listen input event to choose target*/
            battleStateMachine.InputReader.NextTargetAction += HighlightNextTarget;
            battleStateMachine.InputReader.PreviousTargetAction += HighlightPrevTarget;

        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            battleStateMachine.InputReader.NextTargetAction -= HighlightNextTarget;
            battleStateMachine.InputReader.PreviousTargetAction -= HighlightPrevTarget;
        }

        private async void Selected()
        {
            await WaitForConfirm();
            battleStateMachine.StartCoroutine(WaitToEndAnimation());
        }

        private IEnumerator WaitToEndAnimation()
        {
            var targetAlly = battleStateMachine.AllyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>();
            battleStateMachine.PlayerCombatStateMachine.PlayerSetupSkillUI.DisappearSkillUI();
            targetAlly.ItemIndex = battleStateMachine.PlayerCombatStateMachine.ItemIndex;
            targetAlly.ItemType = battleStateMachine.PlayerCombatStateMachine.ItemType;

            targetAlly.HighlightSelectedByAlly.InactiveHighlight();
            targetAlly.SwitchUseItem();

            //Wait until player use item animation done
            yield return new WaitUntil(() => targetAlly.IsFinished == true);

            battleStateMachine.PlayerCombatStateMachine.InactiveCamera();
            battleStateMachine.SwitchResolve();
        }

        private Task WaitForConfirm()
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            void OnConfirm()
            {
                taskCompletionSource.TrySetResult(true);
                battleStateMachine.InputReader.EnterTargetAction -= OnConfirm;
            }

            battleStateMachine.InputReader.EnterTargetAction += OnConfirm;
            return taskCompletionSource.Task;
        }

        private void HighlightNextTarget()
        {
            battleStateMachine.AllyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.InactiveHighlight();
            battleStateMachine.AllyTargeter.ChooseNextTarget();
            battleStateMachine.AllyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.Highlight();
        }

        private void HighlightPrevTarget()
        {
            battleStateMachine.AllyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.InactiveHighlight();
            battleStateMachine.AllyTargeter.ChoosePrevTarget();
            battleStateMachine.AllyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.Highlight();
        }

        private void Highlight()
        {
            battleStateMachine.AllyTargeter.currentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.Highlight();
        }
    }
}

