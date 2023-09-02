using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    // Range radius to detect the player
    [SerializeField]
    private float targetDetectionRange = 5f;

    // Since we need to check the obstacles layer and the player layer
    [SerializeField]
    private LayerMask obstaclesLayerMask, playerLayerMask;

    [SerializeField]
    private bool showGizmos = false;

    // gizmo parameters
    private List<Transform> colliders;

    private Vector2 direction;

    public Vector2 Direction { get => direction; set => direction = value; }

    private void Start()
    {
        BasicEnemy basicEnemy = GetComponentInParent<BasicEnemy>();
        targetDetectionRange = basicEnemy.EnemyStats.DetectionRadius;
    }

    public override void Detect(AIData aiData)
    {
        // Find out if player is near
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);

        if (playerCollider != null)
        {
            // Check if you see the player
            Direction = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction, targetDetectionRange, obstaclesLayerMask);

            // Make sure that the collider we see is on the "Player" layer
            if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, Direction * targetDetectionRange, Color.magenta);
                colliders = new List<Transform>() { playerCollider.transform };
            }
            else
            {
                // player not detected
                colliders = null;
            }
        }
        else
        {
            // Enemy doesn't see the player
            colliders = null;
        }
        aiData.targets = colliders;
    }

    // for showing the overlay gizmos
    private void OnDrawGizmosSelected()
    {
        if (showGizmos == false)
            return;

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }

}
