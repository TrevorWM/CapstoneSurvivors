using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnHitProjectile : ProjectileBase
{
    [SerializeField]
    private OnHitEffect[] hitEffects;

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
                effectInstance.ActivateEffect(payload, this.transform);
            }
        }
        
    }
}
