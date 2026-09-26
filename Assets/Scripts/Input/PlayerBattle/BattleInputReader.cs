using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ConquerTheStars.InputController
{
    public class BattleInputReader : MonoBehaviour, PlayerBattleInput.IPlayerActions
    {
        public event Action NextTargetAction = delegate { };
        public event Action PreviousTargetAction = delegate { };
        public event Action EnterTargetAction = delegate { };
        public event Action BlockAction = delegate { };
        public event Action SettingUiAction = delegate { };
        private PlayerBattleInput _inputActions;

        private void Awake()
        {
            _inputActions = new PlayerBattleInput();
            _inputActions.Player.SetCallbacks(this);
            _inputActions.Enable();
        }

        private void OnEnable() => _inputActions?.Enable();

        private void OnDisable() => _inputActions?.Disable();

        public void OnDodge(InputAction.CallbackContext context)
        {
        }

        public void OnParry(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                BlockAction?.Invoke();
            }
        }

        public void OnSkill_I(InputAction.CallbackContext context)
        {
        }

        public void OnSkill_II(InputAction.CallbackContext context)
        {
        }

        public void OnSkill_III(InputAction.CallbackContext context)
        {
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                NextTargetAction?.Invoke();
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                PreviousTargetAction?.Invoke();
            }
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                EnterTargetAction?.Invoke();
            }
        }

        public void OnTab(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                SettingUiAction?.Invoke();
            }
        }

        public void OnNextButton()
        {
            NextTargetAction?.Invoke();
        }

        public void OnPreviousButton()
        {
            PreviousTargetAction?.Invoke();
        }

        public void OnEnterButton()
        {
            EnterTargetAction?.Invoke();
        }
    }
}