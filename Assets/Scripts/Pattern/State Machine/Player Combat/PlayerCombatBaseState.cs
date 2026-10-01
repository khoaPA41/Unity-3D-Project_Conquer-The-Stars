using ConquerTheStars.Pattern.StateMachine.Base;
using UnityEngine;
namespace ConquerTheStars.Pattern.StateMachine.PlayerCombat
{
    public abstract class PlayerCombatBaseState : State
    {
        protected PlayerCombatStateMachine playerCombatStateMachine;

        protected PlayerCombatBaseState(PlayerCombatStateMachine playerCombatStateMachine)
        {
            this.playerCombatStateMachine = playerCombatStateMachine;
        }

        protected bool MoveToTarget(float deltaTime)
        {
            var targetPos = playerCombatStateMachine.Target.transform.position;
            var currentPos = playerCombatStateMachine.transform.position;
            var offset = targetPos - currentPos;
            offset.y = 0f;

            if (playerCombatStateMachine.IsNotMove)
            {
                return true;
            }

            if (offset.sqrMagnitude < 1f)
            {
                return true;
            }
            var dirToTarget = offset.normalized;

            playerCombatStateMachine.CharacterController.Move(deltaTime * playerCombatStateMachine.Speed * dirToTarget);
            return false;
        }

        protected void MoveBack(float deltaTime)
        {
            var targetPos = playerCombatStateMachine.PlayerStartPosition;
            var currentPos = playerCombatStateMachine.transform.position;
            var offset = targetPos - currentPos;
            offset.y = 0f;

            var distacne = offset.magnitude;
            if (distacne <= 0.1f) return;

            var step = Mathf.Min(playerCombatStateMachine.Speed * deltaTime, distacne);

            playerCombatStateMachine.CharacterController.Move(offset / distacne * step);
        }

        protected void RotateRoot()
        {
            playerCombatStateMachine.transform.eulerAngles = new Vector3(0f, -90f, 0f);
        }
    }
}
