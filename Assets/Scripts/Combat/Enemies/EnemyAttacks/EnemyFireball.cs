using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyFireball : ActiveAbilityBase, IEnemyAttack
{
    [SerializeField]
    private OnHitEffect[] hitEffects;

    [SerializeField]
    private ProjectilePool projectilePool;

    [SerializeField]
    private float hitEffectLifetime;

    private Vector3 aoeScale;
    private AttackPayload attackPayload;

    public Vector3 AoeScale { get => aoeScale; }
    public float HitEffectLifetime { get => hitEffectLifetime; set => hitEffectLifetime = Mathf.Max(0, value); }

    private void InitializeRarityAoEScale(UpgradeRarity rarity)
    {
        if (rarity == UpgradeRarity.Legendary || rarity == UpgradeRarity.Legendary)
        {
            aoeScale = new Vector3(2.5f, 2.5f, 1);
        }
        else if (rarity == UpgradeRarity.Rare || rarity == UpgradeRarity.Legendary)
        {
            aoeScale = new Vector3(2f, 2f, 1);
        }
        else if (rarity == UpgradeRarity.Uncommon || rarity == UpgradeRarity.Legendary)
        {
            aoeScale = new Vector3(1.5f, 1.5f, 1);
        }
        else
        {
            aoeScale = new Vector3(1, 1, 1);
        }
    }

    public override void InitializeRarityBasedStats(UpgradeRarity rolledUpgradeRarity)
    {
        base.InitializeRarityBasedStats(rolledUpgradeRarity);
        InitializeRarityAoEScale(rolledUpgradeRarity);
    }

    public override void SpawnOnHitEffect(AttackPayload payload, Transform hitLocation)
    {
        if (hitEffects.Length > 0)
        {
            foreach (OnHitEffect effect in hitEffects)
            {
                OnHitEffect effectInstance = Instantiate(effect);

                effectInstance.transform.localScale = Vector3.Scale(effectInstance.transform.localScale, aoeScale);
                effectInstance.ActivateEffect(payload, hitLocation, hitEffectLifetime);
            }
        }
    }

    public void DoAttack(CharacterStatsSO stats = null, Vector2 aimDirection = default)
    {
        Transform parentTransform = this.gameObject.transform.parent;
        ProjectileBase projectile = projectilePool.GetProjectile();

        projectile.transform.position = parentTransform.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // This parents the projectiles to the room rather than the enemy
        // if we change where the enemies shoot we will need to change how this parents
        // Easiest would be to grab a reference to the dungeon room object.
        projectile.transform.parent = parentTransform.parent;

        projectile.FireProjectile(aimDirection, stats.ProjectileSpeed, attackPayload, this);
    }

    public void Initialize(CharacterStatsSO stats, UpgradeRarity rarity = UpgradeRarity.Common)
    {
        InitializeRarityBasedStats(rarity);
        projectilePool = GetComponent<ProjectilePool>();
        attackPayload = new AttackPayload(stats.BaseDamage, 0, ElementType.Fire, stats.CriticalChance, stats.CriticalDamageMultiplier, DamageModifierValue, enemyProjectile: true);
    }
}
