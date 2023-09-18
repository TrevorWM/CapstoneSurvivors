using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnHitProjectile : ProjectileBase
{

    [SerializeField]
    private float hitEffectDuration = 0.2f;

    [SerializeField]
    private OnHitEffect[] hitEffects;

    /// <summary>
    /// Override for the ProjectileBase virtual function. Allows us to change the
    /// logic of OnTriggerEnter without having to override the shared collision detection
    /// behaviour.
    /// </summary>
    protected override void OnTriggerEnterLogic()
    {
        ActivateOnHitEffects(attackPayload);
    }

    /// <summary>
    /// If the projectile has a prefab with a OnHitEffect script this will
    /// iterate over all of the attached prefabs in order to instantiate them
    /// and run their effects.
    /// </summary>
    /// <param name="payload"></param>
    private void ActivateOnHitEffects(AttackPayload payload)
    {
        if (hitEffects.Length > 0)
        {
            foreach (OnHitEffect effect in hitEffects)
            {
                OnHitEffect effectInstance = Instantiate(effect);
                effectInstance.ActivateEffect(payload, this.transform, hitEffectDuration);
            }
        }
        
    }
}
