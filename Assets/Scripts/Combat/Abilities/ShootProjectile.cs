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
    private ProjectilePool projectilePool;

    [SerializeField]
    private float projectileSpeed;


    // Everything in here is to define the behaviour of the object pool.
    #region ObjectPool Stuff

    #endregion
    public void Attack()
    {
        Projectile projectile = projectilePool.GetProjectile();
        aimHelper.UpdateAimTowardsMouse();
        projectile.transform.position = aimHelper.transform.position;
        projectile.transform.rotation = aimHelper.transform.rotation;
        Vector2 shootDirection = aimHelper.LookDirection;

        projectile.FireProjectile(shootDirection, projectileSpeed);
    }
}
