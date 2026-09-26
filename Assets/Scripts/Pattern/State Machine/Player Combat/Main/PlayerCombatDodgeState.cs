using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatDodgeState : PlayerCombatBaseState
    {
        private readonly int _dodgeAnimationHash = Animator.StringToHash("Dodge");
        private readonly string _dodgeAnimationTag = "Dodge";
        private float _normalizedTime;
        private float _prevTime;

        public PlayerCombatDodgeState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            _prevTime = 0f;
            _normalizedTime = 0f;
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(_dodgeAnimationHash, playerCombatStateMachine.AnimationCrossFade);
            playerCombatStateMachine.CharacterStatsManagers.SetIsDodge(true);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, _dodgeAnimationTag);

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
