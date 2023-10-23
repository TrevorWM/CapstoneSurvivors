using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour, IEnemyAttack, IDamager
{

    [SerializeField]
    private LayerMask hitLayers;
    [SerializeField]
    private Collider2D attackCollider;
   
    private BasicEnemy ownerScript;
    private AttackPayload attackPayload;
    private Rigidbody2D enemyRigidbody;
    private Vector2 otherDirection;


    public void DoAttack(CharacterStatsSO stats = null, Vector2 aimDirection = default)
    {
        otherDirection = aimDirection;
        StartCoroutine(ChargeUp());
    }

    private IEnumerator ChargeUp()
    {
        Debug.Log("ChargingUp");
        yield return new WaitForSeconds(0.75f);
        Debug.Log("Attacking");
        StartCoroutine(Charge());
    }

    private IEnumerator Charge()
    {
        
        enemyRigidbody.velocity = otherDirection * 6.0f;
        attackCollider.enabled = true;
        Debug.Log("Collider: " + attackCollider.enabled);
        yield return new WaitForSeconds(0.75f);
        attackCollider.enabled = false;
        Debug.Log("Collider: " + attackCollider.enabled);

        //UseMeleeAttack();
        ownerScript.StopMoving = false;
    }

    public void Initialize(CharacterStatsSO stats, UpgradeRarity rarity = UpgradeRarity.Common)
    {

        ownerScript = GetComponentInParent<BasicEnemy>();

        enemyRigidbody = GetComponentInParent<Rigidbody2D>();

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
