using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour, IDamageable
{
    [SerializeField]
    private CharacterStatsSO characterStatsSO;

    [SerializeField]
    private DamageCalculator calculator;

    [SerializeField]
    private FlashSprite flashSprite;

    [SerializeField]
    private bool testHealth;

    // Base Stats
    private float maxHealth;
    private float currentHealth;
    private float defense;
    private float moveSpeed;
    private float moveSpeedModifier;
    private float baseDamage;
    private float attacksPerSecond;
    private float criticalChance;
    private float criticalDamageMultiplier;
    private ElementType characterElement;

    //Dodge Stats
    private float dodgeForce;
    private float dodgeDuration;
    private float dodgeCooldown;

    //Ability Stats
    private float waterAffinity;
    private float fireAffinity;
    private float natureAffinity;
    private float cooldownReduction;

    //Enemy specific
    private float detectionRadius;
    private float followDistance;
    private bool rangedEnemy;
    private float projectileSpeed;

    //Visual
    private bool rightFacingSprite;
    private SpriteRenderer spriteRenderer;

    public UnityEvent playerDied;
    public UnityEvent updateHealth;

    #region Getters and Setters
    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = Mathf.Max(0, value);
    }

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Max(0, Mathf.Min(value, MaxHealth));
            updateHealth?.Invoke();
        }
    }
    public float Defense
    {
        get => defense;
        set => defense = Mathf.Max(0, value);
    }
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = Mathf.Max(0, value);
    }
    public float MoveSpeedModifier
    {
        get => moveSpeedModifier;
        set => moveSpeedModifier = Mathf.Max(1, value);
    }
    public float BaseDamage
    {
        get => baseDamage;
        set => baseDamage = Mathf.Max(1, value);
    }
    public float AttacksPerSecond
    {
        get => attacksPerSecond;
        set => attacksPerSecond = Mathf.Max(0.5f, value);
    }
    public float CriticalChance
    {
        get => criticalChance;
        set => criticalChance = Mathf.Max(0, value);
    }
    public float CriticalDamageMultiplier
    {
        get => criticalDamageMultiplier;
        set => criticalDamageMultiplier = Mathf.Max(0, value);
    }
    public float DodgeForce
    {
        get => dodgeForce;
        set => dodgeForce = Mathf.Max(1, value);
    }
    public float DodgeDuration
    {
        get => dodgeDuration;
        set => dodgeDuration = Mathf.Max(0.1f, value);
    }
    public float DodgeCooldown
    {
        get => dodgeCooldown;
        set => dodgeCooldown = Mathf.Max(0, value);
    }
    public float WaterAffinity
    {
        get => waterAffinity;
        set => waterAffinity = Mathf.Max(0, value);
    }
    public float FireAffinity
    {
        get => fireAffinity;
        set => fireAffinity = Mathf.Max(0, value);
    }
    public float NatureAffinity
    {
        get => natureAffinity;
        set => natureAffinity = Mathf.Max(0, value);
    }
    public float CooldownReduction
    {
        get => cooldownReduction;
        set => cooldownReduction = Mathf.Max(0, value);
    }
    public ElementType CharacterElement 
    { 
        get => characterElement; 
        set => characterElement = value; 
    }
    public float DetectionRadius 
    { 
        get => detectionRadius; 
        set => detectionRadius = value; 
    }
    public float FollowDistance 
    { 
        get => followDistance; 
        set => followDistance = value; 
    }
    public bool RangedEnemy 
    { 
        get => rangedEnemy; 
        set => rangedEnemy = value; 
    }
    public float ProjectileSpeed 
    { 
        get => projectileSpeed; 
        set => projectileSpeed = value; 
    }
    public bool RightFacingSprite 
    { 
        get => rightFacingSprite; 
        set => rightFacingSprite = value; 
    }
    #endregion

    private void OnValidate()
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        Defense = defense;
        MoveSpeed = moveSpeed;
        MoveSpeedModifier = moveSpeedModifier;
        BaseDamage = baseDamage;
        AttacksPerSecond = attacksPerSecond;
        CriticalChance = criticalChance;
        CriticalDamageMultiplier = criticalDamageMultiplier;
        DodgeForce = dodgeForce;
        DodgeDuration = dodgeDuration;
        DodgeCooldown = dodgeCooldown;
        WaterAffinity = waterAffinity;
        FireAffinity = fireAffinity;
        NatureAffinity = natureAffinity;
        CooldownReduction = cooldownReduction;
        CharacterElement = characterElement;
        DetectionRadius = detectionRadius;
        FollowDistance = followDistance;
        RangedEnemy = rangedEnemy;
        ProjectileSpeed = projectileSpeed;
        RightFacingSprite = rightFacingSprite;
    }

    private void Start()
    {
        maxHealth = characterStatsSO.MaxHealth;
        currentHealth = maxHealth;
        defense = characterStatsSO.Defense;
        moveSpeed = characterStatsSO.MoveSpeed;
        moveSpeedModifier = 1f;
        baseDamage = characterStatsSO.BaseDamage;
        attacksPerSecond = characterStatsSO.AttacksPerSecond;
        criticalChance = characterStatsSO.CriticalChance;
        criticalDamageMultiplier = characterStatsSO.CriticalDamageMultiplier;
        dodgeForce = characterStatsSO.DodgeForce;
        dodgeDuration = characterStatsSO.DodgeDuration;
        dodgeCooldown = characterStatsSO.DodgeCooldown;
        waterAffinity = characterStatsSO.WaterAffinity;
        fireAffinity = characterStatsSO.FireAffinity;
        natureAffinity = characterStatsSO.NatureAffinity;
        cooldownReduction = characterStatsSO.CooldownReduction;
        characterElement = characterStatsSO.CharacterElement;
        detectionRadius = characterStatsSO.DetectionRadius;
        followDistance = characterStatsSO.FollowDistance;
        rangedEnemy = characterStatsSO.RangedEnemy;
        projectileSpeed = characterStatsSO.ProjectileSpeed;
        rightFacingSprite = characterStatsSO.RightFacingSprite;
        
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        updateHealth?.Invoke();
        if (testHealth) StartCoroutine(TestHP());
    }

    #region Health Functions
    private void RemoveHealth(float damageTaken)
    {
        CurrentHealth -= damageTaken;
        updateHealth?.Invoke();
        Debug.LogFormat("Took {0} damage, now at {1} HP", damageTaken, CurrentHealth);

        if (CurrentHealth == 0)
        {
            HandleDeath();
        }
    }

    /// <summary>
    /// Adds health the object this component is attached too. Will not heal
    /// over the maximum HP value.
    /// </summary>
    /// <param name="damageHealed"></param>
    private void AddHealth(float damageHealed)
    {
        CurrentHealth += damageHealed;
        updateHealth?.Invoke();
        Debug.LogFormat("I healed for {0} and am now at {1} HP!", damageHealed, CurrentHealth);
    }

    /// <summary>
    /// Handles the logic for when the object this component is attached to loses
    /// all of its HP.
    /// </summary>
    private void HandleDeath()
    {
        Debug.LogFormat("I died!");

        // replace with death logic once animations, sfx, vfx, and other things are in
        this.gameObject.SetActive(false);
        if (this.gameObject.layer == LayerMask.NameToLayer("Player")) playerDied.Invoke();
    }

    /// <summary>
    /// Function used to test HP is working correctly. Feel free to mess
    /// with values as needed and add StartCoroutine where you wish to test.
    /// </summary>
    /// <returns>None</returns>
    private IEnumerator TestHP()
    {
        RemoveHealth(MaxHealth / 3);
        yield return new WaitForSeconds(1);
        AddHealth(MaxHealth);
        yield return new WaitForSeconds(1);
        StartCoroutine(TestHP());
    }
    #endregion

    public void PrintStatSheet()
    {
        Debug.LogFormat("=== Character Stats ===\n" +
            "Max Health: {0}\n" +
            "Current Health: {1}\n" +
            "Defense: {2}\n" +
            "Base Damage: {3}\n" +
            "Critical Chance: {4}\n" +
            "Critical Damage Bonus: {5}\n" +
            "Water Affinity: {6}\n" +
            "Fire Affinity {7}\n" +
            "Nature Affinity {8}\n" +
            "Cooldown Reduction: {9}\n" + 
            "Character Element: {10}", MaxHealth, CurrentHealth, Defense, BaseDamage, CriticalChance, CriticalDamageMultiplier,
            WaterAffinity, FireAffinity, NatureAffinity, CooldownReduction, CharacterElement);

    }

    public void TakeDamage(AttackPayload payload)
    {
        if (payload.EnemyProjectile)
        {
            flashSprite.HitFlash(spriteRenderer);
            float damage = calculator.CalculateDamage(payload, ownerStats: this);
            RemoveHealth(damage);
            
        }
    }
}
