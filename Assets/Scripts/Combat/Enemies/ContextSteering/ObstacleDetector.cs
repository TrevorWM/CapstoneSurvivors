using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : Detector
{
    [SerializeField]
    private float detectionRadius = 2;

    // layer mask obstacles are on
    [SerializeField]
    private LayerMask layerMask;

    // to display the overlay information
    [SerializeField]
    private bool showGizmos = true;

    // save references to detected obstacles for gizmo
    Collider2D[] colliders;


    public override void Detect(AIData aiData)
    {
        // circle cast to detect obstacles around the object
        colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask);
        aiData.obstacles = colliders;
    }

    // for drawing the overlay
    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;
        if (Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.red;
            foreach (Collider2D obstacleCollider in colliders)
            {
                Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
            }
        }
    }
}
