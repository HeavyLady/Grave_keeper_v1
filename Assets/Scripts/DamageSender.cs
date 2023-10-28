using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageSender
{
    
    public static void SendDamage(GameObject damageReceiverObj, float damage)
    {
        Enemy damageReceiverClass;
        if (damageReceiverObj.TryGetComponent<Enemy>(out damageReceiverClass))
        {
            damageReceiverClass.RecieveDamage(damage);
        }
    }

}
