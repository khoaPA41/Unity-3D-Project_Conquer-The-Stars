using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatGetHitState : PlayerCombatBaseState
    {
        private readonly int GetHitAnimationHash = Animator.StringToHash("GetHit");
        private readonly string GetHitAnimationTag = "GetHit";

        private float normalizeTime;
        private float prevTime;

        public PlayerCombatGetHitState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(GetHitAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            normalizeTime = NormalizedTime(playerCombatStateMachine.Animator, GetHitAnimationTag);
            if (normalizeTime > prevTime && normalizeTime > .8f)
            {
                playerCombatStateMachine.ReturnIdle();
            }
            prevTime = normalizeTime;
        }

        public override void Exit()
        {
        }
    }
}
