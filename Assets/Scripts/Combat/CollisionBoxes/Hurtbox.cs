using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    // This object should have the IDamageable interface
    [SerializeField]
    private GameObject owner;

    private AttackPayload attackPayload;

    /// <summary>
    /// Gets the object the hurtbox is colliding with. Checks if it is a Damager object.
    /// If it is then it will get the payload from the object and send it to the owner
    /// of this hurtbox.
    /// </summary>
    /// <param name="collision"></param>
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

    /// <summary>
    /// This function checks if the owner has an IDamageable component, and if it
    /// does then it will send the attack payload object to the hurtbox to do what it wants
    /// with it.
    /// </summary>
    private void SendPayloadToOwner()
    {
        IDamageable ownerComponent = owner.GetComponent<IDamageable>();

        if (ownerComponent != null)
        {
            ownerComponent.TakeDamage(attackPayload);
        }
    }
}
