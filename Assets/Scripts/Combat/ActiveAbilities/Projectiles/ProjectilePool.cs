using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField]
    private ProjectileBase projectilePrefab;

    [SerializeField]
    private int defaultPoolSize;

    [SerializeField]
    private int maxPoolSize;

    [SerializeField]
    private int projectileLifeTime;

    private ObjectPool<ProjectileBase> projectilePool;

    WaitForSeconds projectileTimeout;

    public int MaxPoolSize { get => maxPoolSize; set => maxPoolSize = Mathf.Max(defaultPoolSize, value); }

    private void OnValidate()
    {
        MaxPoolSize = maxPoolSize;
    }

    private void Start()
    {
        projectilePool = new ObjectPool<ProjectileBase>(CreateProjectile, GetProjectileFromPool, ReleaseProjectileFromPool, DestroyPoolObject, true, defaultPoolSize, MaxPoolSize);
        projectileTimeout = new WaitForSeconds(projectileLifeTime);
    }

    // Creation of a new projectile when the pool is full
    private ProjectileBase CreateProjectile()
    {
        ProjectileBase projectile = Instantiate(projectilePrefab);
        return projectile;
    }

    // How the game will grab an object from the pool.
    // In our case we just set the projectile to be active
    private void GetProjectileFromPool(ProjectileBase projectile)
    {
        projectile.gameObject.SetActive(true);
        StartCoroutine(ReleaseObjectAfterTime(projectile));
    }

    // Logic performed when an object is placed in the pool and ready
    // to be spawned again.
    private void ReleaseProjectileFromPool(ProjectileBase projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    // Logic when getting rid of an object from the pool.
    private void DestroyPoolObject(ProjectileBase projectile)
    {
        Destroy(projectile.gameObject);
    }

    // Timer to tell the pool when to release an object
    private IEnumerator ReleaseObjectAfterTime(ProjectileBase projectile)
    {
        yield return projectileTimeout;

        if (projectile != null) projectilePool.Release(projectile);
    }

    public ProjectileBase GetProjectile()
    {
       return projectilePool.Get();
    }
}
