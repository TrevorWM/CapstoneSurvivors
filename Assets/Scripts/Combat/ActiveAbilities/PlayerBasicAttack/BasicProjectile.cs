using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : ProjectileBase, IDamager
{
    protected override void OnTriggerEnterLogic(Collider2D collision)
    {
        if (abilityBase)
        {
            abilityBase.ApplyEffectToTarget(attackPayload, collision);
        }
    }

    public AttackPayload GetAttackPayload()
    {
        return this.attackPayload;
    }
}
