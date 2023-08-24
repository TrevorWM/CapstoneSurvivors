using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 shootDirection;
    private float projectileSpeed;


    /// <summary>
    /// Used when creating a new projectile in order to set its direction
    /// and the speed of the projectile.
    /// 
    /// </summary>
    /// <param name="shootDirection"></param>
    /// <param name="projectileSpeed"></param>
    public void FireProjectile(Vector2 shootDirection, float projectileSpeed)
    {
        this.shootDirection = shootDirection;
        this.projectileSpeed = projectileSpeed;
    }

    private void Update()
    {
        transform.position += shootDirection * projectileSpeed * Time.deltaTime;
    }
}
