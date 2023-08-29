using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI : MonoBehaviour
{
    
    public AIPath aiPath;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (aiPath.desiredVelocity.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (aiPath.desiredVelocity.x < 0)
        {
            spriteRenderer.flipX = false;
        }

    }
}
