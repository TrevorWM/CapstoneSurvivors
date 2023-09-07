using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour, IDamageable
{
   public void TakeDamage(AttackPayload payload)
    {
        Debug.LogFormat("I've been hit by the following attack:\n" +
            "Damage: {0}, DoT Time: {1}, Element: {2}",
            payload.Damage, payload.DotSeconds, payload.Element);
    }
}
