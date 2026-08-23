using ConquerTheStars.Pattern.StateMachine.Enemy;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using ConquerTheStars.Stats;
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
            battleStateMachine.CurrentTurn = battleStateMachine.CharacterStats.Dequeue();

            switch (battleStateMachine.CurrentTurn.characterType)
            {
                case CharacterType.Player:
                    battleStateMachine.PlayerCombatStateMachine = battleStateMachine.CurrentTurn.GetComponent<PlayerCombatStateMachine>();
                    battleStateMachine.SwitchState(battleStateMachine.PlayerTurn);
                    break;
                case CharacterType.Enemy:
                    battleStateMachine.EnemyStateMachine = battleStateMachine.CurrentTurn.GetComponent<EnemyStateMachine>();
                    battleStateMachine.SwitchState(battleStateMachine.EnemyTurn);
                    break;
            }
        }
    }
}
