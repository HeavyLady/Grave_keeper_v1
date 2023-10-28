using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] protected GameObject AttackColliderGameObject;
    [SerializeField] protected float _hitTime;
    protected AttackCollider AttackCollider;
    protected Camera Cam;
    protected PlayerInput PlayerInput;
    protected PlayerMovement PlayerMovement;

    public event Action<Vector3> OnAttackExecute;


    protected const string ENEMY_TAG = "Enemy";
    

    private void Awake()
    {
        Cam = Camera.main;
        PlayerInput = GetComponent<PlayerInput>(); 
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {   
        AttackCollider = AttackColliderGameObject.GetComponent<AttackCollider>();
    }


    protected virtual void ActivateAttack(){}
    protected virtual void ActivateSecondaryAttack(){}
    protected virtual void ActivateEscapeAttack(){}

    protected Vector3 GetMousePosition()
    {
        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    protected void ActivateAttackCollider()
    {   

        AttackColliderGameObject.SetActive(true);
       
    }
    protected void DeactivateAttackCollider()
    {
        AttackColliderGameObject.SetActive(false);
        
    }
}
