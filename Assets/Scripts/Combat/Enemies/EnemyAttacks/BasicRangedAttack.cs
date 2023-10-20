using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BasicRangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField]
    ProjectilePool projectilePool;
    
    GameObject parent;

    private AttackPayload payload;

    public void DoAttack(CharacterStatsSO stats = null, Vector2 aimDirection = default)
    {
        parent = GetComponentInParent<GameObject>();
        ProjectileBase projectile = projectilePool.GetProjectile();

        projectile.transform.position = parent.transform.position;
        projectile.transform.rotation = parent.transform.rotation;

        // This parents the projectiles to the room rather than the enemy
        // if we change where the enemies shoot we will need to change how this parents
        // Easiest would be to grab a reference to the dungeon room object.
        projectile.transform.parent = parent.transform.parent;

        Vector2 shootDirection = aimDirection;
        int dotSeconds = 0;
        bool enemyAttack = true;
        payload = new AttackPayload(stats.BaseDamage, dotSeconds, stats.CharacterElement, stats.CriticalChance, stats.CriticalDamageMultiplier, enemyProjectile: enemyAttack);

        projectile.FireProjectile(shootDirection, stats.ProjectileSpeed, payload);

    }

}
