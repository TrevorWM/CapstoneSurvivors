using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePillarOnHit : OnHitEffect, IDamager
{
    private AttackPayload attackPayload;

    private Collider2D effectCollider;
    public AttackPayload GetAttackPayload()
    {
        return attackPayload;
    }

    public override void ActivateEffect(AttackPayload payload, Transform hitLocation, float effectDuration)
    {
        this.transform.parent = hitLocation.parent;
        this.transform.position = hitLocation.position;
        this.attackPayload = payload;
        effectCollider = this.gameObject.GetComponent<Collider2D>();

        StartCoroutine(DamageOverTimeEffect(effectDuration));
    }

    private IEnumerator DamageOverTimeEffect(float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            if (effectCollider != null) effectCollider.enabled = true;
            yield return new WaitForSeconds(1f);
            if (effectCollider != null) effectCollider.enabled = false;
            yield return new WaitForEndOfFrame();
            timeElapsed += 1f; 
        }
        Despawn();
    }

    private void Despawn()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
