using UnityEngine;

namespace ConquerTheStars.InputController
{
    public class PlayerInputReader : MonoBehaviour, PlayerInput.IPlayerActions
    {
        public Vector2 Movement { get; set; }
        public bool Interact { get; set; }
        public bool Attack { get; set; }

        private PlayerInput playerInput;

        private void Awake()
        {
            playerInput = new PlayerInput();
            playerInput.Player.SetCallbacks(this);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable() => playerInput.Enable();

        private void OnDisable() => playerInput.Disable();

        public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Movement = context.ReadValue<Vector2>();
        }

        public void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
            {
                Attack = false;
                return;
            }
            Attack = true;
        }

        public void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
            {
                Interact = false;
                return;
            }
            Interact = true;
        }

        public void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

        }

    }

}