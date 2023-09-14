using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnHitEffect : MonoBehaviour
{
    public abstract void ActivateEffect(AttackPayload payload, Transform hitLocation, float effectDuration = .2f);
}
