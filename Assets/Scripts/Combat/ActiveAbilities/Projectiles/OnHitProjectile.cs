using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitProjectile : ProjectileBase
{
    [SerializeField]
    private OnHitEffect[] hitEffects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            bool isPlayer = collision.CompareTag("Player");

            if (!isPlayer) ActivateOnHitEffects(attackPayload);

        }
    }

    private void ActivateOnHitEffects(AttackPayload payload)
    {
        if (hitEffects.Length > 0)
        {
            foreach (OnHitEffect effect in hitEffects)
            {
                effect.ActivateEffect();
            }
        } 
    }
}
