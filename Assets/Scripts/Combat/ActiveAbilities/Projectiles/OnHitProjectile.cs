using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnHitProjectile : ProjectileBase
{
    /// <summary>
    /// When the projectile is disabled it will ask the ability base
    /// to spawn the on hit effect at the projectiles hit location.
    /// </summary>
    protected override void OnDisableLogic()
    {
        if (abilityBase != null)
        {
            abilityBase.SpawnOnHitEffect(attackPayload, this.transform);
        }
    }

    protected override void DamageColliderLogic(Collider2D collision)
    {
        if (abilityBase != null)
        {
            abilityBase.ApplyEffectToTarget(attackPayload, collision);  
        }
    }
}
