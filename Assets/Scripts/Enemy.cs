using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _hp;
    [SerializeField] private GameObject _deadVisual;
    [SerializeField] private GameObject _aliveVisual;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<Grave> _graveList;
    private Grave _clostestGrave;
    [SerializeField] private AttackCollider AttackCollider;
    [SerializeField] private bool _isAimToDig = false;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackStun;

    [SerializeField] private AudioClip _damageClip;
    [SerializeField] private AudioClip _metalClip;
    [SerializeField] private AudioClip _dieClip;

    private Animator _animator;

    private float updateDestinationTime = .5f;
    private float updateDestinationTimer;    
    
    private float AttackCooldown = 1f;
    private float AttackCooldownTimer;    
    
    private float attackTime = .3f;
    private float attackTimer;
    private bool isAttacking = false;

    private float diggingTime = 3f;
    private float digginTimer;

    private float attackDistance = 3f;

    private CapsuleCollider _aliveCollider;
    private BoxCollider _deadCollider;

    private NavMeshAgent _agent;

    [SerializeField] private float _moveSpeed;
    private float _maxSpeed;
    private enum EnemyState
    {
        Dead,
        Alive
    }
    private EnemyState _currentEnemyState;
    private bool isDigging;
    private const string PLAYER_TAG = "Player";

    private void Start()
    {
        GameControl.Instance._aliveEnemies++;

        if (_player == null)
        {
            _player = FindAnyObjectByType<Player>().gameObject;
        }



        _aliveCollider = GetComponent<CapsuleCollider>();
        _deadCollider = GetComponent<BoxCollider>();

        _aliveCollider.enabled = true;
        _deadCollider.enabled = false;

        _animator = GetComponentInChildren<Animator>();
        
        _currentEnemyState = EnemyState.Alive;
        _agent = GetComponent<NavMeshAgent>();
        AttackCollider.OnAttackCollided += AttackCollider_OnAttackCollided;

        _maxSpeed = _moveSpeed;
        _agent.speed = _moveSpeed;

        if (_isAimToDig)
        {
            _clostestGrave = GetClosestGraveFromList();
            _agent.SetDestination(_clostestGrave.transform.position);
            
        }
    }

    private void OnDisable()
    {
        AttackCollider.OnAttackCollided -= AttackCollider_OnAttackCollided;
    }



    private void Update()
    {
        if (_currentEnemyState == EnemyState.Dead) return;

        if (_isAimToDig && !isDigging)
        {
            if (_agent.remainingDistance < 0.1)
            {
                StartDigging();
            }
        } else if (_isAimToDig && isDigging)
        {
            Digging();
        }
        CalculateSpeed();

        if (_isAimToDig)
        {
            return;
        }
        
        if (AttackCooldownTimer > 0)
        {
            AttackCooldownTimer -= Time.deltaTime;
        }

        if (isAttacking)
        {
            if (attackTimer <= 0)
            {
                attackTimer = 0;
                isAttacking = false;
                DectivateAttackCollider();
            } else
            {
                attackTimer -= Time.deltaTime;
            }
        }



        if (attackDistance >= Vector3.Distance(transform.position, _player.transform.position))
        {
            Attack();
        } else
        {
            FollowPlayer();
        }

    }

    private void Digging()
    {
        if (digginTimer <= 0)
        {
            digginTimer = 0;
            isDigging = false;
            Digged();
        }
        else
        {
            digginTimer -= Time.deltaTime;
        }
    }

    private void Digged()
    {
        _clostestGrave.SpawnNewEnemy();
        _graveList.Remove(_clostestGrave);
        _clostestGrave = GetClosestGraveFromList();
        _animator.SetBool("Digging", false);
        if (_clostestGrave != null)
        {
            _agent.SetDestination(_clostestGrave.transform.position);
        } else
        {
            _isAimToDig = false;
        }
    }

    private void StartDigging()
    {
        isDigging = true;
        digginTimer = diggingTime;
        _animator.SetBool("Digging", true);


    }

    private Grave GetClosestGraveFromList()
    {
        bool firstIteration = true;
        float closestDistance = 0;
        Grave closestGrave = null;

        foreach (var item in _graveList)
        {   
            if (firstIteration)
            {
                closestGrave = item;
                closestDistance = Vector3.Distance(item.transform.position, transform.position);
                firstIteration = false;
                continue;
            }

            var distance = Vector3.Distance(item.transform.position, transform.position); ;
            
            if (distance < closestDistance)
            {
                closestGrave = item;
                closestDistance = distance;
            }
        }

        return closestGrave;
    }

    private void CalculateSpeed()
    {
        if (_moveSpeed > _maxSpeed)
        {
            _moveSpeed = _maxSpeed;
        }
        else if (_moveSpeed < _maxSpeed)
        {
            _moveSpeed += (Time.deltaTime * 2f);
        }

        _agent.speed = _moveSpeed;
    }

    private void FollowPlayer()
    {   
        if (updateDestinationTimer >= updateDestinationTime)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_player.transform.position);
            updateDestinationTimer = 0f;
        } else
        {
            updateDestinationTimer += Time.deltaTime;
        }
    }

    public void RecieveDamage(float damage, float stunCoefficient)
    {   
        if (_currentEnemyState == EnemyState.Alive)
        {
            TakeDamage(damage);
            ReactOnDamage(stunCoefficient);
        }
    }

    public void ReciveDig()
    {
        Destroy(gameObject);
    }

    private void TakeDamage(float damage)
    {
        _hp -= damage;
        if (CheckDeath())
        {
            Die();
        }
    }

    private bool CheckDeath()
    {
        return _hp <= 0;
    }

    private void Die()
    {
        GameControl.Instance._aliveEnemies--;
        AudioSource.PlayClipAtPoint(_dieClip, transform.position);
        _currentEnemyState = EnemyState.Dead;
        _agent.isStopped = true;
        _animator.SetTrigger("Death");
        _animator.SetBool("Digging", false);
        _aliveCollider.enabled = false;
        _deadCollider.enabled = true;
    }

    private void Attack()
    {
        if (AttackCooldownTimer <= 0)
        {
            AttackCooldownTimer = AttackCooldown;
            _agent.isStopped = true;
            transform.LookAt(_player.transform.position);
            ActivateAttackCollider();
            _animator.SetTrigger("Attack");
            isAttacking = true;
            attackTimer = attackTime;
        }

    }

    private void ActivateAttackCollider()
    {
        AttackCollider.gameObject.SetActive(true);
    }    
    private void DectivateAttackCollider()
    {
        AttackCollider.gameObject.SetActive(false);
    }

    private void AttackCollider_OnAttackCollided(Collider collider)
    {
        if (collider.CompareTag(PLAYER_TAG))
        {
            DamageSender.SendDamage(collider.gameObject, _damage, _attackStun);
 
        }
    }

    private void ReactOnDamage(float stunCoefficient)
    {
        _moveSpeed *= stunCoefficient;
        AudioSource.PlayClipAtPoint(_damageClip, transform.position);
        AudioSource.PlayClipAtPoint(_metalClip, transform.position);
        _animator.SetTrigger("Hit");
    }

}
