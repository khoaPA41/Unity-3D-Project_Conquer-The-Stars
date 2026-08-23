using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleInputReader : MonoBehaviour, PlayerBattleInput.IPlayerActions
{
    public event Action NextTargetAction = delegate { };
    public event Action PreviousTargetAction = delegate { };
    public event Action EnterTargetAction = delegate { };

    private PlayerBattleInput inputActions;


    private void Start()
    {
        inputActions = new PlayerBattleInput();
        inputActions.Player.SetCallbacks(this);
        inputActions.Enable();
    }

    // private void OnEnable() => inputActions.Enable();


    private void OnDisable() => inputActions.Disable();

    public void OnDodge(InputAction.CallbackContext context)
    {
    }

    public void OnParry(InputAction.CallbackContext context)
    {
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
}
