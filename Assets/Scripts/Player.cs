using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float _hp;
    private PlayerMovement _movement;
    public Animator _animator;
    

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_hp <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
    public void RecieveDamage(float damage, float stunCoeffiient)
    {
        _hp -= damage;
        _movement.ReactOnDamage(stunCoeffiient);
    }
}
