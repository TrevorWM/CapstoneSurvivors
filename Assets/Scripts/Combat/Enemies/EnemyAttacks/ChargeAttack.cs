using System;
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
    [SerializeField]
    private SpriteRenderer chargeFade;
   
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
        if (chargeFade != null)
            StartCoroutine(FadeSprite());
        yield return new WaitForSeconds(0.75f);
        Debug.Log("Attacking");
        StartCoroutine(Charge());
    }

    IEnumerator FadeSprite()
    {
        float alpha = 0f;
        while (chargeFade.color.a < 1)
        {
            alpha += Time.deltaTime;
            Debug.Log("Color:" + alpha);
            chargeFade.color = new(1f, 1f, 1f, alpha);
            yield return null;
        }
        yield return null;
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
        } 
        else if (h == Hinderance.Confuse)
        {
            direction *= GetRandomMoveDirection();
        }
        else
        {
            speedReduction = 1f;
        }
        //charge at player
        enemyRigidbody.velocity = direction * enemyStats.MoveSpeed * 6.0f * speedReduction;
        attackCollider.enabled = true;

        yield return new WaitForSeconds(0.75f);
        attackCollider.enabled = false;
        
        ownerScript.StopMoving = false;
        
        if (chargeFade != null)
            chargeFade.color = new(1f, 1f, 1f, 0f);

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

    private Vector2 GetRandomMoveDirection()
    {
        float[] interest = new float[8];

        for (int i = 0; i < 8; i++)
        {
            interest[i] = UnityEngine.Random.Range(0f, 1f);
        }

        //get the average direction
        Vector2 outputDirection = Vector2.zero;

        for (int i = 0; i < 8; i++)
        {
            outputDirection += Directions.eightDirections[i] * interest[i];
        }

        outputDirection.Normalize();

        //return the selected movement direction
        return outputDirection;
    }
}
