using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class ShootProjectile : MonoBehaviour
{
    private CharacterStats stats;

    [SerializeField]
    private AimTowardsMouseComponent aimHelper;

    [SerializeField]
    private ProjectilePool projectilePool;

    [SerializeField]
    private float projectileSpeed;

    private AttackPayload payload;

    private void Start()
    {
        stats = GetComponentInParent<CharacterStats>();
    }

    /// <summary>
    /// Function that creates an AttackPayload object with the information for the attack.
    /// This is then passed to the hurtbox so that the hurtbox knows information about what
    /// kind of attack it took and then can provide that info to other objects it is attached
    /// to.
    /// </summary>
    private void BuildAttackPayload()
    {
        payload = new AttackPayload(stats.BaseDamage, 0, ElementType.None, stats.CriticalChance, stats.CriticalDamageMultiplier);
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
        projectile.transform.position = aimHelper.GetShootPosition();
        projectile.transform.rotation = aimHelper.GetShootRotation();
        Vector2 shootDirection = aimHelper.GetShootDirection();

        BuildAttackPayload();

        projectile.FireProjectile(shootDirection, projectileSpeed, payload);
    }
}
