using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatAttackState : PlayerCombatBaseState
    {
        private readonly string AttackAnimationTagHash = "Attack";
        private readonly int attackIndex;

        private bool isActiveAnimation;
        private float normalizedTime;
        public PlayerCombatAttackState(PlayerCombatStateMachine playerCombatStateMachine, int index) : base(playerCombatStateMachine)
        {
            attackIndex = index;
        }

        public override void Enter()
        {
            // Debug.Log("Attack");

        }

        public override void Tick(float deltaTime)
        {
            if (isActiveAnimation)
            {
                normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, AttackAnimationTagHash);
                if (normalizedTime >= .9)
                {
                    playerCombatStateMachine.IsFinished = true;
                    playerCombatStateMachine.ReturnIdle();
                }
                return;
            }


            if (MoveToTarget(deltaTime))
            {
                if (!isActiveAnimation)
                {
                    isActiveAnimation = true;
                    playerCombatStateMachine.Animator.CrossFadeInFixedTime(playerCombatStateMachine.AttackData.AttackName[attackIndex], playerCombatStateMachine.AnimationCrossFade);
                }
                return;
            }
        }

        public override void Exit()
        {
            // playerCombatStateMachine.IsFinished = false;
        }
    }
}
