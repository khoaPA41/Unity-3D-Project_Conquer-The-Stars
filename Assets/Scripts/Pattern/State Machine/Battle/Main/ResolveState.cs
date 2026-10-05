
using ConquerTheStars.Stats;
using ConquerTheStars.UI.Player;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class ResolveState : BattleBaseState
    {
        private bool _isFinished;
        public ResolveState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            _isFinished = false;

            CheckBuffRemaining();
            InactiveCamera();
            battleStateMachine.TurnOrderService.EnqueueBack(battleStateMachine.CurrentTurn); // Put Current Character Back To List
            ResolveDeaths();
            StealTurn();

            // Check if either one team list is dead, end battle
            if (battleStateMachine.TeamController.CheckBattleResult())
            {
                battleStateMachine.SwitchState(battleStateMachine.Result);
                return;
            }

            _isFinished = true;
        }

        public override void Tick(float deltaTime)
        {
            if (_isFinished)
            {
                _isFinished = false;
                battleStateMachine.SwitchStartTurn();
            }
        }

        public override void Exit()
        {
            battleStateMachine.CurrentTurn = null;
            battleStateMachine.PlayerCombatStateMachine = null;
            battleStateMachine.EnemyStateMachine = null;
        }

        private void CheckBuffRemaining()
        {
            battleStateMachine.PlayerCombatStateMachine?.BuffManager?.CheckRemainingBuff();
        }

        private void ResolveDeaths()
        {
            //Call Death event if character isDeath
            foreach (var character in battleStateMachine.TurnOrderService.CharacterList)
            {
                if (!character.IsDeath) continue;

                if (character.characterType == CharacterType.Enemy)
                {
                    battleStateMachine.PlayerTargeter.RemoveTarget();
                }

                if (character.characterType == CharacterType.Player)
                {
                    battleStateMachine.EnemyTargeter.RemoveTarget();
                }

                character.CallDyingEvent();
            }

            // Remove all character isDeath 
            battleStateMachine.TurnOrderService.RemoveDead();
            UICombatManagers.Instance.ResetTurnOrder();
            UICombatManagers.Instance.SetTurnOrder(battleStateMachine.TurnOrderService.CharacterList);
        }

        private void InactiveCamera()
        {
            battleStateMachine.PlayerCombatStateMachine?.InactiveCamera();
        }

        private void StealTurn()
        {
            battleStateMachine.IsTurnOrderChange = battleStateMachine.TurnOrderService.StealTurn();
        }
    }
}
