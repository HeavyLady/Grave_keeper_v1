using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{   
    public static PlayerInput Instance { get; private set;} 
    private ControlMaps _controls;
    public event Action OnDashPressed;

    private void Awake()
    {
        Instance = this;
        _controls = new ControlMaps();
        _controls.Player.Enable();
        _controls.Player.Dash.performed += Dash_performed;
        //_controls.Player.InteractAlternative.performed += InteractAlternative_performed;
        //_controls.Player.Pause.performed += Pause_performed;
    }


    private void OnDestroy()
    {
        PlayerInput.Instance = null;
        _controls.Player.Dash.performed -= Dash_performed;
        //_controls.Player.Interact.performed -= OnInteract;
        //_controls.Player.InteractAlternative.performed -= InteractAlternative_performed;
        //_controls.Player.Pause.performed -= Pause_performed;
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
