using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObstacle : MonoBehaviour
{
    [SerializeField]
    private LayerMask collisionLayer;

    private ProjectileBase projectile;
    private Collider2D thisCollider;

    private void Start()
    {
        projectile = GetComponentInParent<ProjectileBase>();
        thisCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (projectile != null && thisCollider.IsTouchingLayers(collisionLayer))
        {
            Debug.Log("I hit the wall!");
            projectile.gameObject.SetActive(false);
        }
        
    }
}
