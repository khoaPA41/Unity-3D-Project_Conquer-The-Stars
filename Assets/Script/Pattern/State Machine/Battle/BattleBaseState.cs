using ConquerTheStars.Pattern.StateMachine.Base;
using ConquerTheStars.Pattern.StateMachine.PlayerCombat;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public abstract class BattleBaseState : State
    {
        protected BattleStateMachine battleStateMachine;
        protected BattleBaseState(BattleStateMachine battleStateMachine)
        {
            this.battleStateMachine = battleStateMachine;
        }

        protected void SelectedTarget()
        {

        }
    }
}
