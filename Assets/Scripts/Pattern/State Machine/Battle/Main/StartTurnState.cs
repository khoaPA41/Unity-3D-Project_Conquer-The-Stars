using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using ConquerTheStars.UI.Player;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class StartTurnState : BattleBaseState
    {
        public StartTurnState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            battleStateMachine.IsWaitingCameraBlend = false;
            UICombatManagers.Instance.InactiveTurnOrderHighlight();

            if (battleStateMachine.IsTurnOrderChange)
            {
                UICombatManagers.Instance.ResetTurnOrder();
                UICombatManagers.Instance.SetTurnOrder(battleStateMachine.TurnOrderService.CharacterList);
            }

            UICombatManagers.Instance.ActiveTurnOrderHighlight();
            battleStateMachine.IsTurnOrderChange = false;
            SwitchTurnByType();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        private void SwitchTurnByType()
        {
            battleStateMachine.CurrentTurn = battleStateMachine.TurnOrderService.DequeueNextAlive();

            if (battleStateMachine.CurrentTurn == null)
            {
                battleStateMachine.SwitchStartTurn();
                return;
            }

            switch (battleStateMachine.CurrentTurn.characterType)
            {
                case CharacterType.Player:
                    battleStateMachine.PlayerTargeter.ResetTarget();
                    battleStateMachine.PlayerCombatStateMachine = battleStateMachine.CurrentTurn.GetComponent<PlayerCombatStateMachine>();
                    battleStateMachine.SwitchState(battleStateMachine.PlayerTurn);
                    break;
                case CharacterType.Enemy:
                    battleStateMachine.EnemyTargeter.ResetTarget();
                    battleStateMachine.EnemyStateMachine = battleStateMachine.CurrentTurn.GetComponent<EnemyStateMachine>();
                    battleStateMachine.SwitchState(battleStateMachine.EnemyTurn);
                    break;
            }
        }
    }
}
