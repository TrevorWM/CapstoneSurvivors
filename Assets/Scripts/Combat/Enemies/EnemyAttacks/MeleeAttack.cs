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

    private IEnumerator ColliderDisableDelay()
    {
        yield return new WaitForEndOfFrame();
        attackCollider.enabled = false;
    }
}
