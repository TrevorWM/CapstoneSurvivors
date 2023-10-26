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

        if (ownerComponent != null && owner.activeInHierarchy == true)
        { 
            if(attackPayload.DotSeconds > 0) StartCoroutine(DotTicks(attackPayload, ownerComponent));
            ownerComponent.TakeDamage(attackPayload);
        }
    }

    private IEnumerator DotTicks(AttackPayload payload, IDamageable target)
    {
        int ticks = payload.DotSeconds;

        // We use >1 so that we can have a damage instance when the projectile hits
        // Then the dot ticks for the rest of the time.
        while (ticks > 1)
        {
            yield return new WaitForSeconds(1f);
            target.TakeDamage(payload);
            ticks--;
        }
    }
}
