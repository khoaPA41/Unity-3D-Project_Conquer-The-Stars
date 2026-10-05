using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatDyingState : PlayerCombatBaseState
    {
        private readonly int _dyingAnimationHash = Animator.StringToHash("Dying");
        private readonly string _dyingTag = "Dying";
        private float _normalizedTime;
        private float _prevTime;
        public PlayerCombatDyingState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            playerCombatStateMachine.PlayerSetupUI.ReturnToPool();
            playerCombatStateMachine.Animator.CrossFadeInFixedTime(_dyingAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, _dyingTag);
            if (_normalizedTime > _prevTime && _normalizedTime >= .9 && _normalizedTime <= 1f)
            {
                playerCombatStateMachine.IsFinished = true;
                playerCombatStateMachine.Model.SetActive(false);
            }

            _prevTime = _normalizedTime;
        }

        public override void Exit()
        {
        }
    }
}
