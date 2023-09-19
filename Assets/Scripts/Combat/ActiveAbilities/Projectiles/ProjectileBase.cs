using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    [SerializeField]
    private LayerMask hitLayers;

    private Vector3 shootDirection;
    private float projectileSpeed;
    protected AttackPayload attackPayload;
    protected ProjectilePool pool;

    public ProjectilePool Pool { get => pool; set => pool = value; }

    /// <summary>
    /// Used when creating a new projectile in order to set its direction
    /// and the speed of the projectile.
    /// 
    /// </summary>
    /// <param name="shootDirection"></param>
    /// <param name="projectileSpeed"></param>
    public void FireProjectile(Vector2 shootDirection, float projectileSpeed, AttackPayload payload)
    {
        this.shootDirection = shootDirection;
        this.projectileSpeed = projectileSpeed;
        this.attackPayload = payload;
    }

    private void Update()
    {
        transform.position += shootDirection * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Uses a bitshift and bitwise and in order to see if the object being
        // hit is in the layermask that the projectile is looking at.
        // Info from https://discussions.unity.com/t/check-if-colliding-with-a-layer/145616/2 User: Krnitheesh16
        if ((hitLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            OnTriggerEnterLogic();
            pool.ReleaseProjectileFromPool(this);
        }   
    }

    /// <summary>
    /// Virtual function that allows children to override to add their logic to the
    /// OnTriggerEnter2D function without having to re-implement the collision behavior
    /// logic.
    /// </summary>
    protected virtual void OnTriggerEnterLogic()
    {
        return;
    }
}
