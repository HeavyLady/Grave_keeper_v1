using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private GameObject AttackColliderGameObject;
    [SerializeField] protected float _hitTime;
    protected AttackCollider AttackCollider;
    protected Camera Cam;
    protected PlayerInput PlayerInput;


    protected const string ENEMY_TAG = "Enemy";
    

    private void Awake()
    {
        Cam = Camera.main;
        PlayerInput = GetComponent<PlayerInput>(); 
    }

    private void OnEnable()
    {   
        AttackCollider = AttackColliderGameObject.GetComponent<AttackCollider>();
    }


    protected virtual void ActivateAttack(){}
    public virtual void ExecuteSecondaryAttack(){}
    public virtual void ExecuteEscapeAttack(){}

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
