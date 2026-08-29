using System.Collections;
using System.Linq;
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
            // Skip Characte Death
            while (battleStateMachine.CharacterStats.Count > 0)
            {
                var next = battleStateMachine.CharacterStats.Dequeue();
                if (next == null) continue;

                if (!next.IsDeath)
                {
                    battleStateMachine.CurrentTurn = next;
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
                    battleStateMachine.PlayerTargeter.RemoveTarget();
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
