using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class DemonDoor : MonoBehaviour, IDamageable
{
    [SerializeField]
    private CharacterStatsSO bossStats;

    [SerializeField]
    private Transform targetPosition;

    [SerializeField]
    private ProjectilePool projectilePool;

    [SerializeField]
    private Transform[] bossShootPositions;

    [SerializeField]
    private HealthBar bossHealthBar;

    [SerializeField]
    private DamageCalculator damageCalculator;

    private float runtimeHP = 0;

    public UnityEvent<float, float> updateBossHealth;
    public UnityEvent bossSpawned;
    public UnityEvent bossDeath;

    public void TakeDamage(AttackPayload payload)
    {
        if (payload.EnemyProjectile == false)
        {
            runtimeHP -= damageCalculator.CalculateDamage(payload, defaultOwnerStats: bossStats);
            bossHealthBar.UpdateHealthBarValue(runtimeHP, bossStats.MaxHealth);
            if (runtimeHP <= 0) HandleDeath();
        }  
    }

    private void Start()
    {
        runtimeHP = bossStats.MaxHealth;
        bossHealthBar.UpdateHealthBarValue(runtimeHP, bossStats.MaxHealth);
        bossSpawned?.Invoke();
        StartPhaseOne();
    }

    private void HandleDeath()
    {
        bossDeath?.Invoke();
        this.gameObject.SetActive(false);
    }

    private void ShootProjectiles(Transform shootPosition, Transform target)
    {
        ProjectileBase projectile = projectilePool.GetProjectile();

        projectile.transform.position = shootPosition.position;
        projectile.transform.rotation = shootPosition.rotation;

        // This parents the projectiles to the room rather than the enemy
        // if we change where the enemies shoot we will need to change how this parents
        // Easiest would be to grab a reference to the dungeon room object.
        projectile.transform.parent = gameObject.transform.parent;

        Vector2 shootDirection = (target.position - shootPosition.position).normalized;
        int dotSeconds = 0;
        bool enemyAttack = true;
        AttackPayload payload = new AttackPayload(bossStats.BaseDamage, dotSeconds, bossStats.CharacterElement, bossStats.CriticalChance, bossStats.CriticalDamageMultiplier, enemyProjectile: enemyAttack);

        projectile.FireProjectile(shootDirection, bossStats.ProjectileSpeed, payload);

    }

    private void StartPhaseOne()
    {
        Debug.Log("Phase One Start");
        StartCoroutine(PhaseOneFirst());
        
               
    }

    private IEnumerator PhaseOneFirst()
    {
        foreach (Transform shootPosition in bossShootPositions)
        {
            ShootProjectiles(shootPosition, targetPosition);
        }
        yield return new WaitForSeconds(3f);

        if (runtimeHP > 250)
        {
            StartCoroutine(PhaseOneFirst());
        }
    }
}
