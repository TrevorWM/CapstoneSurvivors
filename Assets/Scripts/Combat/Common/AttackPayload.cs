public class AttackPayload
{
    private float damage;
    private bool isCrit;
    private int dotSeconds;

    private ElementType element;

    public float Damage { get; }
    public int DotSeconds { get; }
    public ElementType Element { get; }
    public float CritChance { get; }
    public float CritMultiplier { get; }
    public float DamageMultiplier { get; }
    public float ElementalAffinity { get; }
    public bool EnemyProjectile { get; }

    public AttackPayload(float damage, int dotSeconds, ElementType element, float critChance, float critMultiplier, float damageMultiplier = 1f, float elementalAffinity = 1f, bool enemyProjectile = false)
    {
        this.Damage = damage;
        this.DotSeconds = dotSeconds;
        this.Element = element;
        this.CritChance = critChance;
        this.CritMultiplier = critMultiplier;
        this.DamageMultiplier = damageMultiplier;
        this.ElementalAffinity = elementalAffinity;
        this.EnemyProjectile = enemyProjectile;
    }
}

public enum ElementType
{
    Fire,
    Nature,
    Water,
    None,
}
