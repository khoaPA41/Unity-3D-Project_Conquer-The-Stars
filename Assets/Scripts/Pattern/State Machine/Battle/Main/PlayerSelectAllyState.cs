using System.Collections;
using ConquerTheStars.Fight.Target;
using ConquerTheStars.InputController;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerSelectAllyState : BattleBaseState
    {
        private PlayerCombatStateMachine _usingItemPlayer;
        private Targeter _allyTargeterList;
        private BattleInputReader _inputReader;
        private PlayerCombatStateMachine _targetAlly;

        public PlayerSelectAllyState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            _usingItemPlayer = battleStateMachine.PlayerCombatStateMachine;
            _allyTargeterList = battleStateMachine.AllyTargeter;
            _inputReader = battleStateMachine.InputReader;

            battleStateMachine.ActiveSelectUI(true, false);

            _usingItemPlayer.HighlightCurrentTurn.InactiveHighlight();
            _usingItemPlayer.IsFinished = false;

            /*Select default ally target*/
            _allyTargeterList.FirstSelected();
            _allyTargeterList.GetTarget();
            Highlight();
            _targetAlly = _allyTargeterList.CurrentTarget.GetComponent<PlayerCombatStateMachine>();

            /*Listen input event to choose target*/
            _inputReader.NextTargetAction += HighlightNextTarget;
            _inputReader.PreviousTargetAction += HighlightPrevTarget;

            // Selected
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

            battleStateMachine.ActiveSelectUI(false, false);

            _allyTargeterList = null;
            _usingItemPlayer = null;
            _inputReader = null;
        }

        private IEnumerator WaitToEndAnimation()
        {
            var target = _targetAlly;
            _usingItemPlayer.PlayerSetupSkillUI.DisappearSkillUI();
            target.IsFinished = false;

            target.BuffManager.SetItemToUse(_usingItemPlayer.ItemData);

            target.HighlightSelectedByAlly.InactiveHighlight();
            target.SwitchUseItem();

            //Wait until player use item animation done
            yield return new WaitUntil(() => target.IsFinished == true);

            _usingItemPlayer.InactiveCamera();
            battleStateMachine.SwitchResolve();
        }

        private void OnConfirm()
        {
            if (_targetAlly.CharacterStatsManagers.IsDeath && _usingItemPlayer.ItemData.ItemType != Factory.Item.ItemType.Revive) return;
            if (!_targetAlly.CharacterStatsManagers.IsDeath && _usingItemPlayer.ItemData.ItemType == Factory.Item.ItemType.Revive) return;

            _inputReader.EnterTargetAction -= OnConfirm;

            _inputReader.NextTargetAction -= HighlightNextTarget;
            _inputReader.PreviousTargetAction -= HighlightPrevTarget;

            battleStateMachine.ActiveSelectUI(false, false);
            battleStateMachine.StartCoroutine(WaitToEndAnimation());
        }

        private void HighlightNextTarget()
        {
            _allyTargeterList.CurrentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.InactiveHighlight();
            _allyTargeterList.ChooseNextTarget();
            _allyTargeterList.CurrentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.Highlight();
            _targetAlly = _allyTargeterList.CurrentTarget.GetComponent<PlayerCombatStateMachine>();
        }

        private void HighlightPrevTarget()
        {
            _allyTargeterList.CurrentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.InactiveHighlight();
            _allyTargeterList.ChoosePrevTarget();
            _allyTargeterList.CurrentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.Highlight();
            _targetAlly = _allyTargeterList.CurrentTarget.GetComponent<PlayerCombatStateMachine>();
        }

        private void Highlight()
        {
            _allyTargeterList.CurrentTarget.GetComponent<PlayerCombatStateMachine>().HighlightSelectedByAlly.Highlight();
        }
    }
}

