
using ConquerTheStars.Fight.Target;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class ResolveState : BattleBaseState
    {
        private bool isFinished;
        public ResolveState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            isFinished = false;

            CheckCurrentTurnList();
            CheckCurrentCharacter();

            // Check if either one team list is dead, end battle
            if (battleStateMachine.TeamController.CheckBattleResult())
            {
                Debug.Log("Is finished battle!");
                battleStateMachine.SwitchState(battleStateMachine.Result);
                return;
            }

            isFinished = true;
        }

        public override void Tick(float deltaTime)
        {
            if (isFinished)
            {
                isFinished = false;
                battleStateMachine.SwitchStartTurn();
            }
        }

        public override void Exit()
        {
            battleStateMachine.CurrentTurn = null;
            battleStateMachine.PlayerCombatStateMachine = null;
            battleStateMachine.EnemyStateMachine = null;
        }

        private void CheckCurrentCharacter()
        {
            battleStateMachine.CharacterStats.Enqueue(battleStateMachine.CurrentTurn);
        }


        private void CheckCurrentTurnList()
        {
            foreach (var character in battleStateMachine.CharacterStats)
            {
                if (character.IsDeath)
                {
                    character.CallDyingEvent();
                }
            }
        }
    }

}
