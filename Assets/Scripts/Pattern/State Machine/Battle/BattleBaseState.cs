using ConquerTheStars.Pattern.StateMachine.Base;

namespace ConquerTheStars.Pattern.StateMachine.Battle
{
    public abstract class BattleBaseState : State
    {
        protected BattleStateMachine battleStateMachine;
        protected BattleBaseState(BattleStateMachine battleStateMachine)
        {
            this.battleStateMachine = battleStateMachine;
        }
    }
}
