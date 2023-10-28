using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public event Action<Collider> OnAttackCollided; 
    private void OnTriggerEnter(Collider other)
    {
        OnAttackCollided?.Invoke(other);
    }
}
