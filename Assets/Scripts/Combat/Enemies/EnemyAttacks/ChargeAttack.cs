using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
    private Vector2 direction;
    private CharacterStatsSO enemyStats;
    private Hinderance h;
    private float speedReduction = 1f;


    public void DoAttack(CharacterStatsSO stats = null, Vector2 aimDirection = default, Hinderance hinderance = Hinderance.None)
    {
        StartCoroutine(ChargeUp());
        h = hinderance;
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
        //get player location
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, enemyStats.DetectionRadius, hitLayers);
        direction = (playerCollider.transform.position - transform.position).normalized;

        if (h == Hinderance.Slow)
        {
            speedReduction = 0.3f;
        } else if (h == Hinderance.Stop)
        {
            speedReduction = 0.01f;
        } else
        {
            speedReduction = 1f;
        }
        //charge at player
        enemyRigidbody.velocity = direction * enemyStats.MoveSpeed * 6.0f * speedReduction;
        attackCollider.enabled = true;
        
        yield return new WaitForSeconds(0.75f);
        attackCollider.enabled = false;
        
        ownerScript.StopMoving = false;
    }

    public void Initialize(CharacterStatsSO stats, UpgradeRarity rarity = UpgradeRarity.Common)
    {
        enemyStats = stats;

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