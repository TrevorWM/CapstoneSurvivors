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
        projectile.transform.position = aimHelper.transform.position;
        projectile.transform.rotation = aimHelper.transform.rotation;
        Vector2 shootDirection = aimHelper.LookDirection;
        BuildAttackPayload();

        projectile.FireProjectile(shootDirection, projectileSpeed, payload);
    }
}
