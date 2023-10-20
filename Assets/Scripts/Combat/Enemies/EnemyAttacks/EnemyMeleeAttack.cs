using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
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

    private void Awake()
    {
        ownerScript = GetComponentInParent<BasicEnemy>();

        Debug.Log("EnemyMeleeAttack");

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
