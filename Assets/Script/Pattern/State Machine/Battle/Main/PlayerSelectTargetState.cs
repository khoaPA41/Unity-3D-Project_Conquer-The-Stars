using System.Threading.Tasks;

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

            Selected();

            /*Listen input event to choose target*/
            battleStateMachine.InputReader.NextTargetAction += battleStateMachine.PlayerTargeter.ChooseNextTarget;
            battleStateMachine.InputReader.PreviousTargetAction += battleStateMachine.PlayerTargeter.ChoosePrevTarget;
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            battleStateMachine.InputReader.NextTargetAction -= battleStateMachine.PlayerTargeter.ChooseNextTarget;
            battleStateMachine.InputReader.PreviousTargetAction -= battleStateMachine.PlayerTargeter.ChoosePrevTarget;
        }

        private async void Selected()
        {
            // battleStateMachine.PlayerCombatStateMachine.RotateToEnemy(battleStateMachine.PlayerTargeter.currentTarget.transform);
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
    }
}