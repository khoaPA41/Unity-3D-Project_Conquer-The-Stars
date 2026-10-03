using ConquerTheStars.InputController;
using ConquerTheStars.Pattern.StateMachine.Enemy;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerSelectTargetState : BattleBaseState
    {
        private BattleInputReader _inputReader;
        public PlayerSelectTargetState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            _inputReader = battleStateMachine.InputReader;
            // battleStateMachine.SelectUi.SetActive(true);
            battleStateMachine.ActiveSelectUI(true, true);
            /*Change Combat Idle State*/
            battleStateMachine.PlayerCombatStateMachine.SwitchState(battleStateMachine.PlayerCombatStateMachine.PlayerCombatIdleState);

            /*Select default target*/
            battleStateMachine.PlayerTargeter.FirstSelected();
            battleStateMachine.PlayerTargeter.GetTarget();
            Highlight();

            // battleStateMachine.PlayerCombatStateMachine.InactiveCamera();

            /*Listen input event to choose target*/
            _inputReader.NextTargetAction += HighlightNextTarget;
            _inputReader.PreviousTargetAction += HighlightPrevTarget;
            _inputReader.EnterTargetAction += OnConfirm;
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            if (_inputReader != null)
            {
                _inputReader.EnterTargetAction -= OnConfirm;
                _inputReader.NextTargetAction -= HighlightNextTarget;
                _inputReader.PreviousTargetAction -= HighlightPrevTarget;
            }

            battleStateMachine.ActiveSelectUI(false, true);
        }

        private void OnConfirm()
        {
            _inputReader.EnterTargetAction -= OnConfirm;

            _inputReader.NextTargetAction -= HighlightNextTarget;
            _inputReader.PreviousTargetAction -= HighlightPrevTarget;

            battleStateMachine.SwitchState(battleStateMachine.PlayerExecuted);
        }

        private void HighlightNextTarget()
        {
            battleStateMachine.PlayerTargeter.CurrentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.InactiveHighlight();
            battleStateMachine.PlayerTargeter.ChooseNextTarget();
            battleStateMachine.PlayerTargeter.CurrentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.Highlight();
        }

        private void HighlightPrevTarget()
        {
            battleStateMachine.PlayerTargeter.CurrentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.InactiveHighlight();
            battleStateMachine.PlayerTargeter.ChoosePrevTarget();
            battleStateMachine.PlayerTargeter.CurrentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.Highlight();
        }

        private void Highlight()
        {
            battleStateMachine.PlayerTargeter.CurrentTarget.GetComponent<EnemyStateMachine>().HighlightTarget.Highlight();
        }
    }
}