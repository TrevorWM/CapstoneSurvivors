using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack, IDamager
{

    [SerializeField]
    private LayerMask hitLayers;
    [SerializeField]
    private Collider2D attackCollider;

    private BasicEnemy ownerScript;
    private CharacterStatsSO ownerStatSO;
    private AttackPayload attackPayload;


    public void DoAttack(CharacterStatsSO stats = null, Vector2 aimDirection = default)
    {
        UseMeleeAttack();
    }

    public void Initialize(CharacterStatsSO stats, UpgradeRarity rarity = UpgradeRarity.Common)
    {
        Debug.Log("EnemyMeleeAttack");

        if (attackCollider != null) attackCollider.enabled = false;

        attackPayload = new AttackPayload(stats.BaseDamage, 0, stats.CharacterElement,
            stats.CriticalChance, stats.CriticalDamageMultiplier, enemyProjectile: true);
    }

    public void UseMeleeAttack()
    {    
        attackCollider.enabled = true;
        Debug.Log("Collider: " + attackCollider.enabled);
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

    public void AbilityCleanup()
    {
        return;
    }
}
