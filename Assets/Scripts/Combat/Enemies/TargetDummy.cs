using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour, IDamageable
{
   public void TakeDamage(AttackPayload payload)
    {
        Debug.LogFormat("I've been hit by the following attack:\n" +
            "Damage: {0}, Critical: {1}, DoT Time: {2}, Element: {3}",
            payload.damage, payload.isCrit, payload.dotSeconds, payload.element);
    }
}
