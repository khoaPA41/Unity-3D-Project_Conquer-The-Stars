using System.Threading.Tasks;
using ConquerTheStars.Pattern.StateMachine.Enemy;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerSelectTargetState : BattleBaseState
    {
        public PlayerSelectTargetState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            /*Change Combat Idle State*/
            battleStateMachine.PlayerCombatStateMachine.SwitchState(battleStateMachine.PlayerCombatStateMachine.PlayerCombatIdleState);

            /*Select default target*/
            battleStateMachine.PlayerTargeter.FirstSelected();
            battleStateMachine.PlayerTargeter.GetTarget();
            Highlight();

            Selected();
            // battleStateMachine.PlayerCombatStateMachine.InactiveCamera();

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
            battleStateMachine.SwitchState(battleStateMachine.Playerexecuted);
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
            battleStateMachine.PlayerTargeter.currentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.InactiveHighlight();
            battleStateMachine.PlayerTargeter.ChooseNextTarget();
            battleStateMachine.PlayerTargeter.currentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.Highlight();
        }

        private void HighlightPrevTarget()
        {
            battleStateMachine.PlayerTargeter.currentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.InactiveHighlight();
            battleStateMachine.PlayerTargeter.ChoosePrevTarget();
            battleStateMachine.PlayerTargeter.currentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.Highlight();
        }

        private void Highlight()
        {
            battleStateMachine.PlayerTargeter.currentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.Highlight();
        }
    }
}