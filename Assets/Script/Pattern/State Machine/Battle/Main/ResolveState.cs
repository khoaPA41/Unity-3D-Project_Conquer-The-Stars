using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class ResolveState : BattleBaseState
    {
        private bool isFinished;
        // CharacterStatsManagers character;
        public ResolveState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            isFinished = false;
            CheckCurrentCharacter();
            CheckCurrentTurnList();
            if (battleStateMachine.TeamController.CheckBattleResult()) // check if either one team list is dead, end battle
            {
                Debug.Log("Is finished battle!");
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
