using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private AnimationCurve _stopSmoth;
    private float easeTimer = 0f;
    private Vector3 _previousDirection = Vector3.zero;

    //DASH
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashCooldown;
    private float _dashCooldownTimer;
    private float _dashTimer;
    private Vector3 _dashingVector;
    //

    // Rotation
    [SerializeField] private float _rotationSpeed;
    //


    private enum PlayerMoveState
    {   
        Idle,
        Moving,
        Dashing
    }

    private PlayerMoveState _currentMoveState;


    private PlayerInput PlayerInput;
    private CharacterController CharacterController;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        CharacterController = GetComponent<CharacterController>();
        _currentMoveState = PlayerMoveState.Idle;
    }

    private void Start()
    {
        PlayerInput.OnDashPressed += PlayerInput_OnDashPressed;
    }

    private void OnDisable()
    {
        PlayerInput.OnDashPressed -= PlayerInput_OnDashPressed;
    }

    private void PlayerInput_OnDashPressed()
    {   
        if (_dashCooldownTimer > 0) { return; }
        DashSetup();
    }

    private void Update()
    {
        CalculateDashCooldown();
        if (_currentMoveState != PlayerMoveState.Dashing) {
            Move();
            Rotate();
        }

        if (_currentMoveState == PlayerMoveState.Dashing) Dashing();
    }

    private void Move()
    {
        Vector2 inputDirection = PlayerInput.GetInputMovementDirectionsNormalized();

        Vector3 inputDirection3d = new Vector3(inputDirection.x, 0, inputDirection.y);

        if (inputDirection == Vector2.zero)
        {
            easeTimer += Time.deltaTime;
        } else
        {
            easeTimer = 0;
            _previousDirection = inputDirection3d;
        }

        Vector3 targetPosition = _previousDirection * _moveSpeed * Time.deltaTime;

        CharacterController.Move(targetPosition * _stopSmoth.Evaluate(easeTimer));
    }

    private void Rotate()
    {
        transform.forward = Vector3.Slerp(transform.forward, _previousDirection, _rotationSpeed * Time.deltaTime);
    }

    private void DashSetup()
    {
        Vector2 inputDirection = PlayerInput.GetInputMovementDirectionsNormalized();

        Vector3 inputDirection3d = new Vector3(inputDirection.x, 0, inputDirection.y);

        if (inputDirection == Vector2.zero)
        {
            inputDirection3d = _previousDirection;
        }

        _dashingVector = inputDirection3d;
        _dashTimer = _dashTime;
        _dashCooldownTimer = _dashCooldown;
        _currentMoveState = PlayerMoveState.Dashing;

    }

    private void Dashing()
    {
       
        CalculateDashTime();
        if (_dashTimer == 0)
        {
            _currentMoveState = PlayerMoveState.Idle;
            return;
        }
        Vector3 targetPosition = _dashingVector * _dashSpeed * Time.deltaTime;
        CharacterController.Move(targetPosition);

    }

    private void CalculateDashCooldown()
    {   
        if (_dashCooldownTimer == 0)
        {
            return;

        } else if (_dashCooldownTimer <= 0)
        {
            _dashCooldownTimer = 0;
        } else
        {
            _dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void CalculateDashTime()
    {
        if (_dashTimer <= 0)
        {
            _dashTimer = 0;
            return;
        } else
        {
            _dashTimer -= Time.deltaTime;
        }
    }
}
