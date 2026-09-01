using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public class PlayerSelectAllyState : BattleBaseState
    {
        public PlayerSelectAllyState(BattleStateMachine battleStateMachine) : base(battleStateMachine)
        {
        }

        public override void Enter()
        {
            foreach (var ally in battleStateMachine.TeamController.ReturnAllyDeath())
            {
                Debug.Log(ally.name);

            }
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}

