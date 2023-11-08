using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LichSecond : MonoBehaviour, IDamageable
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

    [SerializeField]
    private GameObject leftHand;

    [SerializeField]
    private GameObject rightHand;

    [SerializeField]
    private GameObject leftGun;

    [SerializeField]
    private GameObject rightGun;

    private float runtimeHP = 0;
    private Transform player;
    private bool fightStarted = false;

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
        if (activeAbilities.Length > 0)
        {
            enemyAttacks = new IEnemyAttack[activeAbilities.Length];

            for (int i = 0; i < activeAbilities.Length; i++)
            {
                enemyAttacks[i] = Instantiate(activeAbilities[i], gameObject.transform).GetComponent<IEnemyAttack>();
                enemyAttacks[i].Initialize(bossStats);
            }
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
    }

    private Vector2 GetDirectionToPlayer()
    {
        return (player.transform.position - transform.position).normalized;
    }
}
