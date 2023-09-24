using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IDamager
{
    [SerializeField]
    private LayerMask hitLayers;

    private Collider2D attackCollider;

    private BasicEnemy ownerScript;
    private CharacterStatsSO ownerStatSO;
    private AttackPayload attackPayload;
    

    private void Start()
    {
        ownerScript = GetComponentInParent<BasicEnemy>();
        attackCollider = GetComponentInParent<Collider2D>();

        if (attackCollider != null) attackCollider.enabled = false;
        if (ownerScript != null) ownerStatSO = ownerScript.EnemyStats;

        attackPayload = new AttackPayload(ownerStatSO.BaseDamage, 0, ownerStatSO.CharacterElement,
            ownerStatSO.CriticalChance, ownerStatSO.CriticalDamageMultiplier, enemyProjectile: true);
    }

    public void UseMeleeAttack()
    {
        Debug.Log("Collider: " + attackCollider.enabled);
        attackCollider.enabled = true;
        StartCoroutine(ColliderDisableDelay());
    }

    public AttackPayload GetAttackPayload()
    {
        return this.attackPayload;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Uses a bitshift and bitwise and in order to see if the object being
        // hit is in the layermask that the projectile is looking at.
        // Info from https://discussions.unity.com/t/check-if-colliding-with-a-layer/145616/2 User: Krnitheesh16
        if ((hitLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
         
        }
    }

    private IEnumerator ColliderDisableDelay()
    {
        yield return new WaitForEndOfFrame();
        attackCollider.enabled = false;
    }
}
