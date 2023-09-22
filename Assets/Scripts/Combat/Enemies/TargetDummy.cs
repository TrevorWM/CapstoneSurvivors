using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private DamageCalculator damageCalculator;

    [SerializeField]
    private CharacterStatsSO enemyStats;
   public void TakeDamage(AttackPayload payload)
    {
        float damage = damageCalculator.CalculateDamage(payload, defaultOwnerStats: enemyStats);
    }
}
