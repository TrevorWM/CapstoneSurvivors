using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField]
    private Projectile projectilePrefab;

    [SerializeField]
    private int defaultPoolSize;

    [SerializeField]
    private int maxPoolSize;

    [SerializeField]
    private int projectileLifeTime;

    private ObjectPool<Projectile> projectilePool;

    WaitForSeconds projectileTimeout;

    public int MaxPoolSize { get => maxPoolSize; set => maxPoolSize = Mathf.Max(defaultPoolSize, value); }

    private void OnValidate()
    {
        MaxPoolSize = maxPoolSize;
    }

    private void Start()
    {
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, GetProjectileFromPool, ReleaseProjectileFromPool, DestroyPoolObject, true, defaultPoolSize, MaxPoolSize);
        projectileTimeout = new WaitForSeconds(projectileLifeTime);
    }

    // Creation of a new projectile when the pool is full
    private Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab);
        return projectile;
    }

    // How the game will grab an object from the pool.
    // In our case we just set the projectile to be active
    private void GetProjectileFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.Pool = this;
        StartCoroutine(ReleaseObjectAfterTime(projectile));
    }

    // Logic performed when an object is placed in the pool and ready
    // to be spawned again.
    public void ReleaseProjectileFromPool(Projectile projectile)
    {
        if (projectile)
        {
            projectile.gameObject.SetActive(false);
        }
    }

    // Logic when getting rid of an object from the pool.
    private void DestroyPoolObject(Projectile projectile)
    {
        if (projectile)
        {
            Destroy(projectile.gameObject);
        }
    }

    // Timer to tell the pool when to release an object
    private IEnumerator ReleaseObjectAfterTime(Projectile projectile)
    {
        yield return projectileTimeout;

        if (projectile != null) projectilePool.Release(projectile);
    }

    public Projectile GetProjectile()
    {
       return projectilePool.Get();
    }
}
