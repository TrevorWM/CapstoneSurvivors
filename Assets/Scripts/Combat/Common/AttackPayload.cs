using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPayload
{
    private float damage;
    private bool isCrit;
    private int dotSeconds;

    private ElementType element;

    public float Damage { get; }
    public bool IsCrit { get; }
    public int DotSeconds { get; }
    public ElementType Element { get; }
    public bool enemyProjectile { get; }

    public AttackPayload(float damage, bool isCrit, int dotSeconds, ElementType element, bool enemyProjectile = false)
    {
        this.Damage = damage;
        this.IsCrit = isCrit;
        this.DotSeconds = dotSeconds;
        this.Element = element;
        this.enemyProjectile = enemyProjectile;
    }
}

public enum ElementType
{
    Fire,
    Nature,
    Water,
    None,
}
