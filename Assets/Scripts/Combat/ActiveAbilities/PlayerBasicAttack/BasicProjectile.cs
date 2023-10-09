using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : ProjectileBase, IDamager
{

    public AttackPayload GetAttackPayload()
    {
        return this.attackPayload;
    }
}
