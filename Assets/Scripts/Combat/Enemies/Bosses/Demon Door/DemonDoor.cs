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
        runtimeHP -= damageCalculator.CalculateDamage(payload, defaultOwnerStats: bossStats);
        bossHealthBar.UpdateHealthBarValue(runtimeHP, bossStats.MaxHealth);
        if (runtimeHP <= 0) HandleDeath();
    }

    private void Start()
    {
        runtimeHP = bossStats.MaxHealth;
        bossHealthBar.UpdateHealthBarValue(runtimeHP, bossStats.MaxHealth);
        bossSpawned?.Invoke();
    }

    private void Update()
    {
        
    }

    private void HandleDeath()
    {
        bossDeath?.Invoke();
        this.gameObject.SetActive(false);
    }
}
