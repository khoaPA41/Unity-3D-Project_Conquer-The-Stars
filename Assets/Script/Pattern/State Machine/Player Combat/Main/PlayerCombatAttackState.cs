using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public class PlayerCombatAttackState : PlayerCombatBaseState
    {
        private readonly int AttackSpeedParams = Animator.StringToHash("AttackSpeed");

        private readonly string AttackAnimationTag = "Attack";

        // private readonly int attackIndex;

        private bool isActiveAnimation;
        private float normalizedTime;
        private float prevTime;
        public PlayerCombatAttackState(PlayerCombatStateMachine playerCombatStateMachine) : base(playerCombatStateMachine)
        {
            // attackIndex = index;
        }

        public override void Enter()
        {
            isActiveAnimation = false;
            UIManagers.Instance.SkillActionFrame.PauseSkillActionFrame += playerCombatStateMachine.ReturnAttackSpeed;
        }

        public override void Tick(float deltaTime)
        {
            if (isActiveAnimation)
            {
                normalizedTime = NormalizedTime(playerCombatStateMachine.Animator, AttackAnimationTag);

                if (normalizedTime > prevTime && normalizedTime >= .9)
                {
                    playerCombatStateMachine.IsFinished = true;
                    playerCombatStateMachine.ReturnIdle();
                }
                prevTime = normalizedTime;
                return;
            }


            if (MoveToTarget(deltaTime))
            {
                if (!isActiveAnimation)
                {
                    isActiveAnimation = true;
                    UIManagers.Instance.ActiveActionFrame();
                    playerCombatStateMachine.Animator.CrossFadeInFixedTime(playerCombatStateMachine.AttackData.AttackName[playerCombatStateMachine.AttackIndexSelected], playerCombatStateMachine.AnimationCrossFade);
                }
            }
        }

        public override void Exit()
        {
            UIManagers.Instance.SkillActionFrame.PauseSkillActionFrame -= playerCombatStateMachine.ReturnAttackSpeed;
        }
    }
}
