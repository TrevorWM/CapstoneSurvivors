using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstLich : MonoBehaviour, IDamageable
{
    [SerializeField]
    private CharacterStatsSO bossStats;

    [SerializeField]
    private HealthBar bossHealthBar;

    [SerializeField]
    private DamageCalculator damageCalculator;

    [SerializeReference]
    private GameObject[] activeAbilities;

    private IEnemyAttack[] enemyAttacks;

    [SerializeReference]
    private Transform[] teleportPositions;

    [SerializeField]
    private GameObject phaseOneAdd, phaseTwoAdd;

    private float runtimeHP = 0;
    private Transform player;
    private bool fightStarted = false;
    private bool phaseOneSpawn = false;
    private bool phaseTwoSpawn1 = false;
    private bool phaseTwoSpawn2 = false;
    private bool phaseTwoSpawn3 = false;

    public UnityEvent bossSpawned;
    public UnityEvent bossDeath;
    public UnityEvent<GameObject> spawnMonster;

    private void Start()
    {
        runtimeHP = bossStats.MaxHealth;
        bossHealthBar.gameObject.SetActive(false);
        InitializeAbilities();
        bossSpawned?.Invoke();
    }

    private void InitializeAbilities()
    {
        enemyAttacks = new IEnemyAttack[activeAbilities.Length];

        for (int i = 0; i < activeAbilities.Length; i++)
        {
            enemyAttacks[i] = Instantiate(activeAbilities[i], gameObject.transform).GetComponent<IEnemyAttack>();
            enemyAttacks[i].Initialize(bossStats);
        }
    }

    public void StartFight(Transform playerTransform)
    {
        player = playerTransform;
        bossHealthBar.gameObject.SetActive(true);
        bossHealthBar.UpdateHealthBarValue(runtimeHP, bossStats.MaxHealth);
        if (!fightStarted) StartPhaseOne();
        fightStarted = true;
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

    private void StartPhaseOne()
    {
        Debug.Log("Start Phase One!");
        StartCoroutine(PhaseOne());
    }

    private Vector2 GetDirectionToPlayer()
    {
        return (player.transform.position - transform.position).normalized;
    }

    private IEnumerator PhaseOne()
    {
        foreach (IEnemyAttack attack in enemyAttacks)
        {
            attack.DoAttack(bossStats, GetDirectionToPlayer());
            yield return new WaitForSeconds(1f);
        }
        this.transform.position = teleportPositions[UnityEngine.Random.Range(0, teleportPositions.Length)].position;


        if (runtimeHP < (bossStats.MaxHealth * 0.80f) && !phaseOneSpawn)
        {
            spawnMonster?.Invoke(phaseOneAdd);
            phaseOneSpawn = true;
        }

        if (runtimeHP < bossStats.MaxHealth * 0.70f)
        {
            StartCoroutine(PhaseTwo()); 
        }
        else
        {
            StartCoroutine(PhaseOne());
        }  
    }

    private IEnumerator PhaseTwo()
    {
        Debug.Log("Phase Two Start!");
        StopCoroutine(PhaseOne());

        int newPosition = UnityEngine.Random.Range(0, teleportPositions.Length);
        int lastPosition;

        this.transform.position = teleportPositions[newPosition].position;
        lastPosition = newPosition;
        yield return new WaitForSeconds(0.75f);
        ShootThreeTowardsPlayer(0);
        yield return new WaitForSeconds(1f);

        while (newPosition == lastPosition) newPosition = UnityEngine.Random.Range(0, teleportPositions.Length);

        this.transform.position = teleportPositions[newPosition].position;
        lastPosition = newPosition;
        yield return new WaitForSeconds(0.75f);
        ShootThreeTowardsPlayer(1);
        yield return new WaitForSeconds(1f);
        
        while (newPosition == lastPosition) newPosition = UnityEngine.Random.Range(0, teleportPositions.Length);

        this.transform.position = teleportPositions[newPosition].position;
        lastPosition = newPosition;
        yield return new WaitForSeconds(0.75f);
        ShootThreeTowardsPlayer(2);

        if (runtimeHP < (bossStats.MaxHealth * 0.4f) && !phaseTwoSpawn1)
        {
            spawnMonster?.Invoke(phaseTwoAdd);
            phaseTwoSpawn1 = true;
            yield return new WaitForSeconds(4f);
        }
        else if (runtimeHP < (bossStats.MaxHealth * 0.25f) && !phaseTwoSpawn2)
        {
            spawnMonster?.Invoke(phaseTwoAdd);
            phaseTwoSpawn2 = true;
            yield return new WaitForSeconds(4f);
        }
        else if (runtimeHP < (bossStats.MaxHealth * 0.1f) && !phaseTwoSpawn3)
        {
            spawnMonster?.Invoke(phaseTwoAdd);
            phaseTwoSpawn3 = true;
            yield return new WaitForSeconds(4f);
        }
        else
        {
            phaseTwoSpawn1 = false;
            phaseTwoSpawn2 = false;
            phaseTwoSpawn3 = false;
        }
        yield return new WaitForSeconds(2f);


        StartCoroutine(PhaseTwo());
    }

    private void ShootThreeTowardsPlayer(int abilityIndex)
    {
        Vector2 positiveRotation = Quaternion.AngleAxis(10f, Vector3.forward) * GetDirectionToPlayer();
        Vector2 negativeRotation = Quaternion.AngleAxis(-10f, Vector3.forward) * GetDirectionToPlayer();

        enemyAttacks[abilityIndex].DoAttack(bossStats, positiveRotation);
        enemyAttacks[abilityIndex].DoAttack(bossStats, negativeRotation);
        enemyAttacks[abilityIndex].DoAttack(bossStats, GetDirectionToPlayer());
    }
}
