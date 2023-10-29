using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageSender
{
    
    public static void SendDamage(GameObject damageReceiverObj, float damage, float stunCoeffiient)
    {
        if (damageReceiverObj.TryGetComponent<Enemy>(out Enemy damageReceiverEnemyClass))
        {
            damageReceiverEnemyClass.RecieveDamage(damage, stunCoeffiient);

        }
        else if (damageReceiverObj.TryGetComponent<Player>(out Player damageReceiverPlayerClass))
        {
            damageReceiverPlayerClass.RecieveDamage(damage, stunCoeffiient);
        }
    }

}
