using System.Linq;
using ConquerTheStars.Pattern.StateMachine.Enemy;
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
            // GetTargetDealDamage();
            isFinished = true;

        }

        public override void Tick(float deltaTime)
        {
            if (isFinished)
            {
                isFinished = false;
                CheckCurrentCharacter();
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
            Debug.Log("Enqueue: " + battleStateMachine.CurrentTurn);

            battleStateMachine.CharacterStats.Enqueue(battleStateMachine.CurrentTurn);
        }
    }
}
