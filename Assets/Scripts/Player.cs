using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _hp;
    private PlayerMovement _movement;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
    }
    public void RecieveDamage(float damage, float stunCoeffiient)
    {
        _hp -= damage;
        _movement.ReactOnDamage(stunCoeffiient);
    }
}
