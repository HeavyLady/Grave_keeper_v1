using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryAttack : CombatSystem
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackStun;
    [SerializeField] private int _attackHitAmount;
    [SerializeField] private float _pauseBetweenHits;
    private bool _isAttackPerfoming = false;
    private float _attackTimer = 0;
    private float _pauseTimer;
    private int _hitCounter;
    private Vector3 _hitPosition;
    

    private void Start()
    {
        PlayerInput.OnSecondaryAttackPressed += PlayerInput_OnSecondaryAttackPressed;
        AttackCollider.OnAttackCollided += AttackCollider_OnAttackCollided;

        DeactivateAttackCollider();
    }


    private void OnDisable()
    {
        PlayerInput.OnSecondaryAttackPressed -= PlayerInput_OnSecondaryAttackPressed;
        AttackCollider.OnAttackCollided -= AttackCollider_OnAttackCollided;
    }

    private void Update()
    {

        if (_pauseTimer > 0)
        {
            _pauseTimer -= Time.deltaTime;
            DeactivateAttackCollider();
            return;
        }

        if (_isAttackPerfoming)
        {
            

            if (_attackTimer > 0)
            {
                ActivateAttackCollider();
                _attackTimer -= Time.deltaTime;
            }
            else
            {   
                if (_hitCounter - 1 > 0)
                {
                    _hitCounter--;
                    _attackTimer = _hitTime;
 

                } else
                {
                    
                    _isAttackPerfoming = false;
                    DeactivateAttackCollider();
                    AttackColliderGameObject.transform.parent = gameObject.transform;
                }

                _pauseTimer = _pauseBetweenHits;
            }
        }
    }
    private void PlayerInput_OnSecondaryAttackPressed()
    {
        ActivateSecondaryAttack();
    }
    private void AttackCollider_OnAttackCollided(Collider collider)
    {
        if (collider.CompareTag(ENEMY_TAG))
        {
            ExexuteAttack(collider.gameObject);
        }
    }

    protected override void ActivateSecondaryAttack()
    {
        var mouseposition = GetMousePosition();
        _hitPosition = mouseposition;
        _isAttackPerfoming = true;
        _attackTimer = _hitTime;
        _pauseTimer = _pauseBetweenHits;
        _hitCounter = _attackHitAmount;
        PlayerMovement.RactOnAttack(mouseposition, _hitTime);
        AttackColliderGameObject.transform.parent = null;
        AttackColliderGameObject.transform.position = _hitPosition;



    }

    private void ExexuteAttack(GameObject target)
    {
        DamageSender.SendDamage(target, _damage, _attackStun);
    }
}
