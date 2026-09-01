using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatDodgeState : PlayerCombatBaseState
    {
        private readonly int DodgeAnimationHash = Animator.StringToHash("Dodge");
        private readonly string DodgeAnimationTag = "Dodge";

        private float _normalizedTime;
        private float _prevTime;
        public PlayerCombatDodgeState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(DodgeAnimationHash, playerCombatStateMachine.AnimationCrossFade);
            playerCombatStateMachine.CharacterStatsManagers.SetIsDodge(true);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, DodgeAnimationTag);

            if (_normalizedTime > _prevTime && _normalizedTime > .3f)
            {
                playerCombatStateMachine.CharacterStatsManagers.SetIsDodge(false);

            }

            if (_normalizedTime > _prevTime && _normalizedTime > .9f && _normalizedTime <= 1f)
            {
                playerCombatStateMachine.ReturnCombatIdle();
            }

            _prevTime = _normalizedTime;
        }

        public override void Exit()
        {
        }
    }
}
