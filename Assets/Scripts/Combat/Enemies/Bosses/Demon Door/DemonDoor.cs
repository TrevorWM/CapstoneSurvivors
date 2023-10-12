using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
    private Transform player;
    private bool fightStarted = false;

    public UnityEvent bossSpawned;
    public UnityEvent bossDeath;

    private void Start()
    {
        runtimeHP = bossStats.MaxHealth;
        bossHealthBar.gameObject.SetActive(false);
        bossSpawned?.Invoke();
    }

    public void StartFight(Transform playerTransform)
    {
        player = playerTransform;
        fightStarted = true;
        bossHealthBar.gameObject.SetActive(true);
        bossHealthBar.UpdateHealthBarValue(runtimeHP, bossStats.MaxHealth);
        StartPhaseOne();
    }

    public void TakeDamage(AttackPayload payload)
    {
        if (payload.EnemyProjectile == false && fightStarted)
        {
            runtimeHP -= damageCalculator.CalculateDamage(payload, defaultOwnerStats: bossStats);
            bossHealthBar.UpdateHealthBarValue(runtimeHP, bossStats.MaxHealth);
            if (runtimeHP <= 0) HandleDeath();
        }
    }

    private void HandleDeath()
    {
        bossDeath?.Invoke();
        this.gameObject.SetActive(false);
    }

    private void ShootAtPosition(Transform shootPosition, Transform target)
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
        AttackPayload payload = new AttackPayload(bossStats.BaseDamage, dotSeconds, bossStats.CharacterElement,
            bossStats.CriticalChance, bossStats.CriticalDamageMultiplier, enemyProjectile: enemyAttack);

        projectile.FireProjectile(shootDirection, bossStats.ProjectileSpeed, payload);

    }

    private IEnumerator ShootRotation(Transform shootPosition, float degrees, float totalRotation, float shotDelay = 0f)
    {
        int shotNumber = 0;

        WaitForSeconds delay = new WaitForSeconds(shotDelay);

        while ((degrees * shotNumber) < totalRotation) 
        {
            ProjectileBase projectile = projectilePool.GetProjectile();
            
            Vector2 shootDirection = new Vector2(Mathf.Cos((degrees * shotNumber) * Mathf.Deg2Rad), Mathf.Sin((degrees * shotNumber) * Mathf.Deg2Rad));

            projectile.transform.position = shootPosition.position;
            projectile.transform.rotation = shootPosition.rotation;

            projectile.transform.parent = gameObject.transform.parent;

            int dotSeconds = 0;
            bool enemyAttack = true;
            AttackPayload payload = new AttackPayload(bossStats.BaseDamage, dotSeconds, bossStats.CharacterElement, 
                bossStats.CriticalChance, bossStats.CriticalDamageMultiplier, enemyProjectile: enemyAttack);

            projectile.FireProjectile(shootDirection, bossStats.ProjectileSpeed, payload);

            shotNumber++;
            if (shotDelay != 0f) yield return delay;
            
        }
        yield return null;
    }

    private void StartPhaseOne()
    {
        Debug.Log("Phase One Start");
        StartCoroutine(PhaseOne());              
    }

    private IEnumerator PhaseOne()
    {
        //Each pillar shoots at a random position
        foreach (Transform shootPosition in bossShootPositions)
        {
            targetPosition.position = new Vector2(Random.Range(-5, 15), Random.Range(-10, 5));

            ShootAtPosition(shootPosition, player);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);


        //Random pillar shoots a circle
        int randomPosition = Random.Range(0, bossShootPositions.Length);
        StartCoroutine(ShootRotation(bossShootPositions[randomPosition], 10f, 360f));
        yield return new WaitForSeconds(1f);
        

        if (runtimeHP > bossStats.MaxHealth/2)
        {
            StartCoroutine(PhaseOne());
        }
        else
        {
            Debug.Log("Phase Two Start");
            StartCoroutine(PhaseTwo());
        }
    }

    private IEnumerator PhaseTwo()
    {
        foreach (Transform shootPosition in bossShootPositions)
        {
            StartCoroutine(ShootRotation(shootPosition, 5f, 360f, 0.2f));
        }
        yield return new WaitForSeconds(1f);


        StartCoroutine(ShootRotation(bossShootPositions[0], 20f, 360f));
        StartCoroutine(ShootRotation(bossShootPositions[2], 20f, 360f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(ShootRotation(bossShootPositions[1], 20f, 360f));
        StartCoroutine(ShootRotation(bossShootPositions[3], 20f, 360f));
        yield return new WaitForSeconds(3f);
        
        if (runtimeHP > 0) StartCoroutine(PhaseTwo());
    }
}
