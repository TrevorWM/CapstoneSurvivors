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
    private float projectileLifeTime;

    private ObjectPool<ProjectileBase> projectilePool;
    private RoomManager roomManager;
    

    WaitForSeconds projectileTimeout;

    public int DefaultPoolSize { get => defaultPoolSize; set => defaultPoolSize = Mathf.Max(0,value); }
    public int MaxPoolSize { get => maxPoolSize; set => maxPoolSize = Mathf.Max(defaultPoolSize, value); }

    private void OnValidate()
    {
        DefaultPoolSize = defaultPoolSize;
        MaxPoolSize = maxPoolSize;
    }

    private void Start()
    {
        projectilePool = new ObjectPool<ProjectileBase>(CreateProjectile, GetProjectileFromPool, ReleaseProjectileFromPool, DestroyPoolObject, true, defaultPoolSize, MaxPoolSize);
        projectileTimeout = new WaitForSeconds(projectileLifeTime);
        roomManager = FindObjectOfType<RoomManager>();
        if (roomManager != null) roomManager.GetComponent<RoomManager>();
    }

    // Creation of a new projectile when the pool is full
    private ProjectileBase CreateProjectile()
    {
        ProjectileBase projectile;

        if (roomManager != null)
        {
            projectile = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation, roomManager.CurrentRoom.transform);
        }
        else
        {
            projectile = Instantiate(projectilePrefab);
        }
        
        return projectile;
    }

    // How the game will grab an object from the pool.
    // In our case we just set the projectile to be active
    private void GetProjectileFromPool(ProjectileBase projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.Pool = this;
        StartCoroutine(ReleaseObjectAfterTime(projectile));
    }

    // Logic performed when an object is placed in the pool and ready
    // to be spawned again.
    public void ReleaseProjectileFromPool(ProjectileBase projectile)
    {
        if (projectile)
        {
            projectile.gameObject.SetActive(false);
        }
    }

    // Logic when getting rid of an object from the pool.
    private void DestroyPoolObject(ProjectileBase projectile)
    {
        if (projectile)
        {
            Destroy(projectile.gameObject);       
        }
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

    public void ClearPool()
    {
        projectilePool.Clear();
    }
}
