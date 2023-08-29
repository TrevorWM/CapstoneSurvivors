using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField]
    private CharacterStatsSO stats;

    [SerializeField]
    private AimTowardsMouseComponent aimHelper;

    [SerializeField]
    private ProjectilePool projectilePool;

    [SerializeField]
    private float projectileSpeed;

    private AttackPayload payload;

    private void BuildAttackPayload()
    {
        payload = new AttackPayload(stats.BaseDamage, false, 0, ElementType.None);
    }

    public void Attack()
    {
        Projectile projectile = projectilePool.GetProjectile();
        aimHelper.UpdateAimTowardsMouse();
        projectile.transform.position = aimHelper.transform.position;
        projectile.transform.rotation = aimHelper.transform.rotation;
        Vector2 shootDirection = aimHelper.LookDirection;
        BuildAttackPayload();

        projectile.FireProjectile(shootDirection, projectileSpeed, payload);
    }
}
