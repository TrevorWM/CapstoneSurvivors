using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtPlayer : MonoBehaviour
{
    [SerializeField]
    private LayerMask hitLayers;
    [SerializeField]
    private float angleOffset = -45f;
    readonly float detectionRadius = 10000f;
    Collider2D playerCollider;
    Vector2 direction;
    float angle;

    private void FixedUpdate()
    {
        //get player location
        playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, hitLayers);
        direction = playerCollider.transform.position - transform.position;

        if (direction != Vector2.zero)
        {
            // Calculate the rotation angle
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation
            transform.rotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward);
        }

        //angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 45f, Vector3.forward);

    }
}
