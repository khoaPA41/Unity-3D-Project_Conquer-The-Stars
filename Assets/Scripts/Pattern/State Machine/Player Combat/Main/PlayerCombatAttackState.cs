using ConquerTheStars.UI.Player;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatAttackState : PlayerCombatBaseState
    {
        private readonly int JumpAnimationHash = Animator.StringToHash("Jump");

        private readonly string AttackAnimationTag = "Attack";

        private bool _isActiveAnimation;
        private float _normalizedTime;
        private float _prevTime;
        string _animationName;
        public PlayerCombatAttackState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
        }

        public override void Enter()
        {
            _isActiveAnimation = false;
            _prevTime = 0f;
            _normalizedTime = 0f;
            playerCombatStateMachine.IsWatingCameraBlendFinished = false;

            UIManagers.Instance.SkillActionFrame.PauseSkillActionFrame += playerCombatStateMachine.ReturnAttackSpeed;
            _animationName = playerCombatStateMachine.AttackNameList == "Attack" ?
            playerCombatStateMachine.AttackData.AttackName[playerCombatStateMachine.AttackIndexSelected] :
            playerCombatStateMachine.AttackData.SkillName[playerCombatStateMachine.AttackIndexSelected];

            playerCombatStateMachine.Animator.CrossFadeInFixedTime(JumpAnimationHash, playerCombatStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            if (_isActiveAnimation)
            {
                _normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, AttackAnimationTag);

                if (_normalizedTime > _prevTime && _normalizedTime >= .9f && _normalizedTime <= 1f)
                {
                    playerCombatStateMachine.IsFinished = true;
                    playerCombatStateMachine.ReturnCombatIdle();
                }
                _prevTime = _normalizedTime;
                return;
            }


            if (MoveToTarget(deltaTime))
            {
                playerCombatStateMachine.ActiveCamera();
                if (!playerCombatStateMachine.IsWatingCameraBlendFinished) return;

                if (playerCombatStateMachine.CinemachineBrain.IsBlending) return;
                if (!_isActiveAnimation)
                {
                    _isActiveAnimation = true;

                    playerCombatStateMachine.Animator.CrossFadeInFixedTime(_animationName, playerCombatStateMachine.AnimationCrossFade);
                }
            }

            playerCombatStateMachine.RotateToEnemy(playerCombatStateMachine.Target.transform);
        }

        public override void Exit()
        {
            UIManagers.Instance.SkillActionFrame.PauseSkillActionFrame -= playerCombatStateMachine.ReturnAttackSpeed;
            RotateRoot();
            playerCombatStateMachine.InactiveCamera();
        }
    }
}
