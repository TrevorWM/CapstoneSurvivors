using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnHitProjectile : ProjectileBase
{
    /// <summary>
    /// Override for the ProjectileBase virtual function. Allows us to change the
    /// logic of OnTriggerEnter without having to override the shared collision detection
    /// behaviour.
    /// </summary>
    protected override void OnTriggerEnterLogic()
    {
        Debug.Log("Hit something!");
        if (abilityBase != null)
        {
            Debug.Log("Ability Base found");
            abilityBase.SpawnOnHitEffect(attackPayload, this.transform);
        }    
    }
}
