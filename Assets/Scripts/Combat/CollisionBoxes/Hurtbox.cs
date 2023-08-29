using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField]
    private GameObject owner;

    private AttackPayload attackPayload;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IDamager>(out IDamager damager))
        {
            attackPayload = damager.GetAttackPayload();

            if (attackPayload != null)
            {
                SendPayloadToOwner();
            }
        }
    }

    private void SendPayloadToOwner()
    {
        IDamageable ownerComponent = owner.GetComponent<IDamageable>();

        if (ownerComponent != null)
        {
            ownerComponent.TakeDamage(attackPayload);
        }
    }
}
