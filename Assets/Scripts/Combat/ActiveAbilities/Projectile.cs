using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamager
{
    private Vector3 shootDirection;
    private float projectileSpeed;
    public AttackPayload attackPayload;
    [SerializeField]
    public LayerMask colliderLayers;

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
        float bulletRadius = 0.1f;
        Collider2D overlap = Physics2D.OverlapCircle(transform.position, bulletRadius, colliderLayers);
        if (overlap != null)
        {
            Destroy(transform.gameObject);
        }
    }

    public AttackPayload GetAttackPayload()
    {
        return attackPayload;
    }
}
