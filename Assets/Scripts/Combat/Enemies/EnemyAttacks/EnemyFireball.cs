using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class EnemyFireball : FireballBase, IEnemyAttack
{
    [SerializeField]
    private ProjectilePool projectilePool;

    [SerializeField]
    private int projectileCount = 1;

    [SerializeField]
    private float projectileAngle = 0f;

    private AttackPayload attackPayload;
    private int projectileDepth = 0;

    private void OnValidate()
    {
        projectileCount = Mathf.Max(1, projectileCount);
        projectileAngle = Mathf.Max(0, projectileAngle);
    }

    public override void InitializeRarityBasedStats(UpgradeRarity rarity)
    {
        base.InitializeRarityBasedStats(rarity);
        base.InitializeRarityAoEScale(rarity);
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

    public void DoAttack(CharacterStatsSO stats = null, Vector2 aimDirection = default, Hinderance hinderance = Hinderance.None)
    {
        for (int i = 0; i < projectileCount; i++)
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

            if (i == 0)
            {
                projectile.FireProjectile(aimDirection, stats.ProjectileSpeed, attackPayload, this);
                projectileDepth++;
            }
            else if (i % 2 == 1)
            {
                Vector2 rotatedDirection = Quaternion.AngleAxis(projectileAngle * projectileDepth, Vector3.forward) * aimDirection;
                projectile.FireProjectile(rotatedDirection, stats.ProjectileSpeed, attackPayload, this);
            }
            else if (i % 2 == 0)
            {
                Vector2 rotatedDirection = Quaternion.AngleAxis(-projectileAngle * projectileDepth, Vector3.forward) * aimDirection;
                projectile.FireProjectile(rotatedDirection, stats.ProjectileSpeed, attackPayload, this);
                projectileDepth++;
            }
        }
        projectileDepth = 0;
    }

    public void Initialize(CharacterStatsSO stats, UpgradeRarity rarity = UpgradeRarity.Common)
    {
        InitializeRarityBasedStats(rarity);
        attackPayload = new AttackPayload(stats.BaseDamage, 0, ElementType.Fire, stats.CriticalChance, stats.CriticalDamageMultiplier, DamageModifierValue, enemyProjectile: true);
    }

    public void AbilityCleanup()
    {
        projectilePool.ClearPool();
    }
}
