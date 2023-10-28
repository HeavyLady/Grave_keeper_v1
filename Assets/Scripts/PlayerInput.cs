using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{   
    public static PlayerInput Instance { get; private set;} 
    private ControlMaps _controls;
    public event Action OnDashPressed;
    public event Action OnMainAttackPressed;
    public event Action OnSecondaryAttackPressed;
    public event Action OnEscapeAttackPressed;

    private void Awake()
    {
        Instance = this;
        _controls = new ControlMaps();
        _controls.Player.Enable();
        _controls.Player.Dash.performed += Dash_performed;
        _controls.Player.AttackMain.performed += AttackMain_performed; ;
        _controls.Player.AttackSecondary.performed += AttackSecondary_performed; ;
        _controls.Player.AttackEscape.performed += AttackEscape_performed;
    }


    private void OnDestroy()
    {
        PlayerInput.Instance = null;
        _controls.Player.Dash.performed -= Dash_performed;
        _controls.Player.AttackMain.performed -= AttackMain_performed; ;
        _controls.Player.AttackSecondary.performed -= AttackSecondary_performed; ;
        _controls.Player.AttackEscape.performed -= AttackEscape_performed;
    }
    private void AttackEscape_performed(InputAction.CallbackContext obj)
    {
        OnEscapeAttackPressed?.Invoke();
    }

    private void AttackSecondary_performed(InputAction.CallbackContext obj)
    {
        OnSecondaryAttackPressed?.Invoke();
    }

    private void AttackMain_performed(InputAction.CallbackContext obj)
    {
        OnMainAttackPressed?.Invoke();
    }


    private void Dash_performed(InputAction.CallbackContext obj)
    {
        OnDashPressed?.Invoke();
    }

    public Vector2 GetInputMovementDirectionsNormalized()
    {
        Vector2 inputDirection = _controls.Player.Move.ReadValue<Vector2>();
        inputDirection.Normalize();

        return inputDirection;
    }



    
}
