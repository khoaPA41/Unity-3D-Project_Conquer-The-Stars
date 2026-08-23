using System.Linq;
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
            GetTargetDealDamage();
            CheckCurrentCharacter();
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

        private void GetTargetDealDamage()
        {
            if (battleStateMachine.CurrentTurn.characterType == CharacterType.Player)
            {
                battleStateMachine.PlayerTargeter.currentTarget.GetComponent<CharacterStatsManagers>().TakeDamage(20f);
            }
            else if (battleStateMachine.CurrentTurn.characterType == CharacterType.Enemy)
            {
                battleStateMachine.EnemyTargeter.currentTarget.GetComponent<CharacterStatsManagers>().TakeDamage(20f);
            }
        }

        private void CheckCurrentCharacter()
        {
            battleStateMachine.CharacterStats.Enqueue(battleStateMachine.CurrentTurn);
        }
    }
}
