using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAttack : CombatSystem
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackStun;
    private bool _isAttackPerfoming = false;
    private float _attackTimer = 0;
    private Player Player;




    


    private void Start()
    {
        PlayerInput.OnMainAttackPressed += PlayerInput_OnMainAttackPressed;
        AttackCollider.OnAttackCollided += AttackCollider_OnAttackCollided;
        Player = GetComponent<Player>();


        DeactivateAttackCollider();
    }


    private void OnDisable()
    {
        PlayerInput.OnMainAttackPressed -= PlayerInput_OnMainAttackPressed;
        AttackCollider.OnAttackCollided -= AttackCollider_OnAttackCollided;
    }

    private void Update()
    {
        if (_isAttackPerfoming)
        {
            if (_attackTimer > 0)
            {
                _attackTimer -= Time.deltaTime;
            } else
            {
                _isAttackPerfoming = false;
                DeactivateAttackCollider();
            }
        }
    }

    private void PlayerInput_OnMainAttackPressed()
    {   
        if (!GameControl.Instance.IsFighting())
        {
            ActivateDig();
        } else
        {
            ActivateAttack();
        }
    }
    private void AttackCollider_OnAttackCollided(Collider collider)
    {
        if (collider.CompareTag(ENEMY_TAG)) {
            ExexuteAttack(collider.gameObject);
        }
    }

    private void ActivateDig()
    {

        _isAttackPerfoming = true;
        _attackTimer = _hitTime;
        Player._animator.SetTrigger("Digging");
        ActivateAttackCollider();
        PlayerMovement.RactOnAttack(GetMousePosition(), _hitTime);
    }

    protected override void ActivateAttack()
    {
        
        _isAttackPerfoming = true;
        _attackTimer = _hitTime;
        Player._animator.SetTrigger("MainAttack");
        ActivateAttackCollider();
        PlayerMovement.RactOnAttack(GetMousePosition(), _hitTime);


    }



    private void ExexuteAttack(GameObject target)
    {
        if (!GameControl.Instance.IsFighting())
        {
            DamageSender.SendDig(target);
            
        }
        else
        {
            DamageSender.SendDamage(target, _damage, _attackStun);
        }
        
    }
}
