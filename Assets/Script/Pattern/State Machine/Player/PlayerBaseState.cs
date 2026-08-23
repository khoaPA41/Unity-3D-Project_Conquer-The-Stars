using ConquerTheStars.Pattern.StateMachine.Base;
using UnityEngine;

namespace ConquerTheStars.Pattern.StateMachine.Player
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine playerStateMachine;

        protected PlayerBaseState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            playerStateMachine.CharacterController.Move((playerStateMachine.ForceReceiver.Movement + motion) * deltaTime);
        }

        protected void FaceDir(Vector3 direction, float deltaTime)
        {
            if (direction == Vector3.zero) return;
            playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation,
            Quaternion.LookRotation(direction), playerStateMachine.RotationDamping * deltaTime);
        }

        protected Vector3 CalculateDir()
        {
            Vector3 forwardCam = playerStateMachine.MainCamera.transform.forward;
            Vector3 rightCam = playerStateMachine.MainCamera.transform.right;
            forwardCam.y = 0f;
            rightCam.y = 0;
            forwardCam.Normalize();
            rightCam.Normalize();

            return forwardCam * playerStateMachine.InputReader.Movement.y +
            rightCam * playerStateMachine.InputReader.Movement.x;
        }


    }
}
