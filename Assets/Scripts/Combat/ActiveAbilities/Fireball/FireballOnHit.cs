using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FireballOnHit : OnHitEffect, IDamager
{
    private AttackPayload attackPayload;

    /// <summary>
    /// Sets the location of the effect and spawns it. Then it starts a despawn
    /// coroutine so that the object disappears.
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="hitLocation"></param>
    /// <param name="effectDuration"></param>
    public override void ActivateEffect(AttackPayload payload, Transform hitLocation, float effectDuration)
    {
        this.transform.parent = hitLocation.parent;
        this.transform.position = hitLocation.position;
        this.transform.rotation = hitLocation.rotation;
        this.attackPayload = payload;

        this.gameObject.SetActive(true);
    }

    public AttackPayload GetAttackPayload()
    {
        return this.attackPayload;
    }

    private void Despawn()
    {
        Destroy(this.gameObject);
    }
}
