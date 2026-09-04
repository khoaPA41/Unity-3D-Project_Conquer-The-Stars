using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using ConquerTheStars.UI.Player;

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
            UIManagers.Instance.InactiveTurnOrderHighlight();

            if (battleStateMachine.IsTurnOrderChange)
            {
                UIManagers.Instance.ResetTurnOrder();
                UIManagers.Instance.SetTurnOrder(battleStateMachine.CharacterStats);
            }
            UIManagers.Instance.ActiveTurnOrderHighlight();
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
            // Get next character and remove if next character death
            foreach (var next in battleStateMachine.CharacterStats)
            {
                if (next == null) continue;

                if (!next.IsDeath)
                {
                    battleStateMachine.CurrentTurn = next;
                    battleStateMachine.CharacterStats.Remove(next);
                    break;
                }
            }

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
