
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    [SerializeField]
    private LayerMask damageLayers, collisionLayers;

    [SerializeField]
    private Collider2D damageCollider;

    [SerializeField]
    private Collider2D collisionCollider;
    
    private Vector3 shootDirection;
    private float projectileSpeed;
    protected ActiveAbilityBase abilityBase;
    protected AttackPayload attackPayload;
    protected ProjectilePool pool;

    public ProjectilePool Pool { get => pool; set => pool = value; }

    /// <summary>
    /// Used when creating a new projectile in order to set its direction
    /// and the speed of the projectile.
    /// 
    /// </summary>
    /// <param name="shootDirection"></param>
    /// <param name="projectileSpeed"></param>
    public void FireProjectile(Vector2 shootDirection, float projectileSpeed, AttackPayload payload, ActiveAbilityBase abilityBase = null)
    {
        this.shootDirection = shootDirection;
        this.projectileSpeed = projectileSpeed;
        this.attackPayload = payload;
        this.abilityBase = abilityBase;
        Vector3 shootAngle = this.transform.eulerAngles;
        SpriteRenderer projectileSprite = this.GetComponentInChildren<SpriteRenderer>();

        //Flips the sprite so it's always right side up when shooting
        if ((shootAngle.z > 90 && shootAngle.z < 270))
        { 
            if (projectileSprite != null) projectileSprite.flipY = true;
        }
        else
        {
            if (projectileSprite != null) projectileSprite.flipY = false;
        }
        
    }

    private void Update()
    {
        if (projectileSpeed > 0) transform.position += shootDirection * projectileSpeed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageCollider.IsTouchingLayers(damageLayers))
        {
            DamageColliderLogic(collision);
        }

        if (collisionCollider.IsTouchingLayers(collisionLayers))
        {
            CollisionColliderLogic(collision);
        }

        
    }

    /// <summary>
    /// Virtual function that allows children to override to add their logic to the
    /// OnTriggerEnter2D function for the collision collider without having to
    /// re-implement the collision behavior
    /// logic.
    /// </summary>
    protected virtual void CollisionColliderLogic(Collider2D collision)
    {
       if (pool != null) pool.ReleaseProjectileFromPool(this);
    }

    protected virtual void DamageColliderLogic(Collider2D collision)
    {
        return;
    }

    /// <summary>
    /// Virtual function that allows children to override to add their logic to the
    /// OnDisable function.
    /// </summary>
    protected virtual void EndOfLifetimeLogic()
    {
        return;
    }

    public void EndOfLifetime()
    {
        EndOfLifetimeLogic();
    }
}
