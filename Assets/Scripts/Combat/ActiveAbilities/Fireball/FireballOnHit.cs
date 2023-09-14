using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballOnHit : OnHitEffect
{
    public override void ActivateEffect()
    {
        Debug.Log("Boom!");
    }
}
