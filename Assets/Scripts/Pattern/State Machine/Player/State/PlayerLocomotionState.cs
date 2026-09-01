using ConquerTheStars.Pattern.StateMachine.Player;
using UnityEngine;

public class PlayerLocomotionState : PlayerBaseState
{
    private readonly int LomocotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int MovementParam = Animator.StringToHash("Movement");

    public PlayerLocomotionState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(LomocotionBlendTreeHash, playerStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 motion = CalculateDir();
        Move(motion * playerStateMachine.Speed, deltaTime);
        FaceDir(motion, deltaTime);
        UpdateAnimation(deltaTime);
    }

    public override void Exit()
    {

    }


    private void UpdateAnimation(float deltaTime)
    {
        if (playerStateMachine.InputReader.Movement == Vector2.zero)
        {
            playerStateMachine.Animator.SetFloat(MovementParam, 0, playerStateMachine.AnimationCrossFade, deltaTime);
            if (playerStateMachine.Animator.GetFloat(MovementParam) <= 0.001f)
            {
                playerStateMachine.Animator.SetFloat(MovementParam, 0);
            }
            return;
        }

        playerStateMachine.Animator.SetFloat(MovementParam, 1, playerStateMachine.AnimationCrossFade, deltaTime);
    }


}
