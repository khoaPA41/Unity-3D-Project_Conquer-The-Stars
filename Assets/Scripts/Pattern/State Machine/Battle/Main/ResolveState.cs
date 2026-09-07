
using System.Linq;
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

            CheckBuffRemaining();
            InactiveCamera();
            CheckCurrentCharacter();
            CheckCurrentTurnList();
            StealTurn();

            // Check if either one team list is dead, end battle
            if (battleStateMachine.TeamController.CheckBattleResult())
            {
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

        private void CheckBuffRemaining()
        {
            if (battleStateMachine.PlayerCombatStateMachine != null)
                battleStateMachine.PlayerCombatStateMachine.BuffManager.CheckRemainingBuff();
        }

        private void CheckCurrentCharacter()
        {
            battleStateMachine.CharacterStats.Add(battleStateMachine.CurrentTurn);
        }


        private void CheckCurrentTurnList()
        {
            //Call Death event if character isDeath
            foreach (var character in battleStateMachine.CharacterStats)
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

                battleStateMachine.IsTurnOrderChange = true; // Update Turn Order UI if character die

                character.CallDyingEvent();
            }

            // Remove all character isDeath 
            battleStateMachine.CharacterStats.RemoveAll(character => character.IsDeath);
        }

        private void InactiveCamera()
        {
            battleStateMachine.PlayerCombatStateMachine?.InactiveCamera();
        }

        private void StealTurn()
        {
            var candidates = battleStateMachine.CharacterStats.ToList();

            foreach (var character in candidates)
            {
                if (character.CurrentSpeed <= battleStateMachine.SpeedAverage) continue;

                var stealTurnRate = Mathf.Clamp(character.CurrentLuck * (character.CurrentSpeed - battleStateMachine.SpeedAverage) / 100f, 0f, .95f);
                if (!IsStealSuccess(stealTurnRate)) continue;

                battleStateMachine.IsTurnOrderChange = true;

                battleStateMachine.CharacterStats.Remove(character);

                battleStateMachine.CharacterStats.Insert(0, character);
            }
        }

        private bool IsStealSuccess(float rate)
        {
            return Random.value < rate;
        }
    }
}
