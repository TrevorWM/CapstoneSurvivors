using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField]
    private AimTowardsMouseComponent aimHelper;

    [SerializeField]
    private bool shootFromMouse = false;

    [SerializeField]
    private ProjectilePool projectilePool;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private bool alwaysUp = false;

    private CharacterStats characterStats;
    private ActiveAbilityBase activeAbility;
    private ActiveAbilitySO abilityStats;
    private Rigidbody2D ownerRigidbody;

    private AttackPayload payload;

    private void Start()
    {
        characterStats = GetComponentInParent<CharacterStats>();
        ownerRigidbody = characterStats.gameObject.GetComponent<Rigidbody2D>();
        activeAbility = GetComponent<ActiveAbilityBase>();
        if (activeAbility != null ) abilityStats = activeAbility.ActiveAbilitySO;
    }

    /// <summary>
    /// Function that creates an AttackPayload object with the information for the attack.
    /// This is then passed to the hurtbox so that the hurtbox knows information about what
    /// kind of attack it took and then can provide that info to other objects it is attached
    /// to.
    /// </summary>
    private void BuildAttackPayload()
    {
        if (activeAbility != null)
        {
            payload = new AttackPayload(characterStats.BaseDamage, activeAbility.DotTime, abilityStats.AbilityElement, characterStats.CriticalChance,
                characterStats.CriticalDamageMultiplier, activeAbility.DamageModifierValue, GetCharacterElementalAffinity(), hinderance: abilityStats.HinderanceType, effectTime: activeAbility.EffectTime);
        } 
        else
        {
            payload = new AttackPayload(characterStats.BaseDamage, 0, ElementType.None, characterStats.CriticalChance, characterStats.CriticalDamageMultiplier);
        }
    }

    /// <summary>
    /// Grabs a projectile from the pool object on the attack prefab. Then gets
    /// the aim position and rotatin from the mouse. Afterwards it spawns the projectile
    /// and fires it in the direction of the mouse.
    /// </summary>
    public void Attack()
    {
        ProjectileBase projectile = projectilePool.GetProjectile();
        
        aimHelper.UpdateAimTowardsMouse();

        if (shootFromMouse) projectile.transform.position = aimHelper.GetMousePosition();
        else projectile.transform.position = aimHelper.GetShootPosition();

        if (alwaysUp) projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
        else projectile.transform.rotation = aimHelper.GetShootRotation();
        
        Vector2 shootDirection = aimHelper.GetShootDirection();

        BuildAttackPayload();

        if (activeAbility != null)
        {
            projectileSpeed = activeAbility.ActiveAbilitySO.ProjectileSpeed;
            if (activeAbility.ActiveAbilitySO.ProjectileSpeed >= characterStats.MoveSpeed) projectileSpeed += characterStats.MoveSpeed;

            projectile.FireProjectile(shootDirection, projectileSpeed, payload, activeAbility);
        } else
        {
            projectile.FireProjectile(shootDirection, projectileSpeed, payload);
        }
        
    }

    private float GetCharacterElementalAffinity()
    {
        switch (abilityStats.AbilityElement)
        {
            case ElementType.Fire:
                return characterStats.FireAffinity;
            case ElementType.Nature:
                return characterStats.NatureAffinity;
            case ElementType.Water:
                return characterStats.WaterAffinity;
            case ElementType.None:
                    return 1.0f;
            default:
                return 1.0f;
        }
    }
}
