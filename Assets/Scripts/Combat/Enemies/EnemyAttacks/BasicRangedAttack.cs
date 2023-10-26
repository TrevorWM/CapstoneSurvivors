using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField]
    ProjectilePool projectilePool;

    private AttackPayload payload;

    public void DoAttack(CharacterStatsSO stats, Vector2 aimDirection = default(Vector2), Hinderance hinderance = Hinderance.None)
    {
        Transform parentTransform = this.gameObject.transform.parent;
        ProjectileBase projectile = projectilePool.GetProjectile();

        projectile.transform.position = parentTransform.position;
        projectile.transform.rotation = parentTransform.rotation;

        // This parents the projectiles to the room rather than the enemy
        // if we change where the enemies shoot we will need to change how this parents
        // Easiest would be to grab a reference to the dungeon room object.
        projectile.transform.parent = parentTransform.parent;

        //Vector2 shootDirection = aimDirection;
        
        //payload = new AttackPayload(stats.BaseDamage, dotSeconds, stats.CharacterElement, stats.CriticalChance, stats.CriticalDamageMultiplier, enemyProjectile: enemyAttack);

        projectile.FireProjectile(aimDirection, stats.ProjectileSpeed, payload);

    }

    public void Initialize(CharacterStatsSO stats, UpgradeRarity rarity = UpgradeRarity.Common)
    {
        payload = new AttackPayload(stats.BaseDamage, 0, stats.CharacterElement,
            stats.CriticalChance, stats.CriticalDamageMultiplier, enemyProjectile: true);
    }

    public void AbilityCleanup()
    {
        projectilePool.ClearPool();
    }
}
