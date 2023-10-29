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
    private NavMeshAgent _agent;

    private enum EnemyState
    {
        Dead,
        Alive
    }
    private EnemyState _currentEnemyState;

    private void Start()
    {
        _currentEnemyState = EnemyState.Alive;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _agent.SetDestination(_player.transform.position);
    }

    public void RecieveDamage(float damage)
    {   
        if (_currentEnemyState == EnemyState.Alive)
        {
            TakeDamage(damage);
        }
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
        _currentEnemyState = EnemyState.Dead;
        _deadVisual.SetActive(true);
        _aliveVisual.SetActive(false);
    }

}
