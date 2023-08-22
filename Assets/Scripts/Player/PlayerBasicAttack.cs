using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerBasicAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerStatsSO playerStats;

    [SerializeField]
    private ShootPositionHelper shootPositionHelper;

    [SerializeField]
    private Projectile projectilePrefab;

    [SerializeField]
    private float projectileSpeed;

    protected ObjectPool<Projectile> projectilePool;
    WaitForSeconds projectileTimeout = new WaitForSeconds(10);


    public void Awake()
    {
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, TakeProjectileFromPool, ReturnProjectileToPool, DestroyPoolObject);
    }

    /* Everything in here are functions to define the behaviour of the
     * object pool.
     */

    #region ObjectPool Stuff

    // Creation of a new projectile when the pool is full
    protected Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab);

        return projectile;
    }

    // How the game will grab an object from the pool.
    // In our case we just set the projectile to be active
    protected void TakeProjectileFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);

    }

    // Logic performed when an object is placed in the pool and ready
    // to be spawned again.
    protected void ReturnProjectileToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);

    }

    // Logic when getting rid of an object from the pool.
    protected void DestroyPoolObject(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    // Timer to tell the pool when to release an object
    private IEnumerator ReleaseObjectAfterTime(Projectile projectile)
    {
        yield return projectileTimeout;
        projectilePool.Release(projectile);
    }
    #endregion
    public void Attack()
    {
        Projectile projectile = projectilePool.Get();
        projectile.transform.position = shootPositionHelper.GetShootPosition();
        projectile.transform.rotation = shootPositionHelper.GetShootRotation();
        Vector2 shootDirection = shootPositionHelper.GetShootDirection();

        projectile.FireProjectile(shootDirection, projectileSpeed);
        StartCoroutine(ReleaseObjectAfterTime(projectile));
    }
}
