using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _hp;

    private enum EnemyState
    {
        Dead,
        Alive
    }
    private EnemyState _currentEnemyState;

    private void Start()
    {
        _currentEnemyState = EnemyState.Alive;
    }

    public void RecieveDamage(float damage)
    {
        TakeDamage(damage);
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
        gameObject.SetActive(false);
    }

}
