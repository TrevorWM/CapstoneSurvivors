using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPayload
{
    public float damage;
    public bool isCrit;
    public int dotSeconds;

    public ElementType element;

    public AttackPayload(float damage, bool isCrit, int dotSeconds, ElementType element)
    {
        this.damage = damage;
        this.isCrit = isCrit;
        this.dotSeconds = dotSeconds;
        this.element = element;
    }
}

public enum ElementType
{
    Fire,
    Nature,
    Water,
    None,
}
