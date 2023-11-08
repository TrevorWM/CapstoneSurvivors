using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "ScriptableObjects/StatSheets/CharacterStats", order = 0)]
public class CharacterStatsSO : ScriptableObject
{
    [Header("==== Default Stats ====")]
    [Header ("Base Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float defense;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float baseDamage;
    [SerializeField] private float attacksPerSecond;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamageMultiplier;
    [SerializeField] private ElementType characterElement;

    [Header ("Dodge Stats")]
    [SerializeField] private float dodgeForce;
    [SerializeField] private float dodgeDuration;
    [SerializeField] private float dodgeCooldown;

    [Header ("Ability Stats")]
    [SerializeField] private float waterAffinity;
    [SerializeField] private float fireAffinity;
    [SerializeField] private float natureAffinity;
    [SerializeField] private float cooldownReduction;

    [Header ("Enemy Specific Stats")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private float followDistance;
    [SerializeField] private EnemyType aiType;
    [SerializeField] private float projectileSpeed;

    [Header("Visual Elements")]
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private Sprite weaponSprite;
    [SerializeField] private bool rightFacingSprite;
    [SerializeField] private bool overrideVisuals;
    
    /*
        Field encapsulation so that we can contain the data validation logic to this class
        By changing the logic of the setters here we can change what values are valid for
        each of our stats. If needed we can add maximum values, but for now I left the
        max values open ended.
    */
    public float MaxHealth 
    { 
        get => maxHealth;
        set => maxHealth = Mathf.Max(0, value); 
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
    public float DetectionRadius 
    { 
        get => detectionRadius;
        set => detectionRadius = Mathf.Max(0, value); 
    }
    public float FollowDistance 
    { 
        get => followDistance; 
        set => followDistance = Mathf.Max(1, value); 
    }
    public global::System.Boolean RightFacingSprite 
    { 
        get => rightFacingSprite; 
        set => rightFacingSprite = value; 
    }
    public float ProjectileSpeed 
    { 
        get => projectileSpeed; 
        set => projectileSpeed = value; 
    }
    public ElementType CharacterElement 
    { 
        get => characterElement; 
        set => characterElement = value; 
    }
    public EnemyType AiType { get => aiType; set => aiType = value; }
    public bool OverrideVisuals { get => overrideVisuals; set => overrideVisuals = value; }


    /// <summary>
    /// Validates values when they are set in the inspector
    /// using the logic from the property setters.
    /// </summary>
    private void OnValidate()
    {
        MaxHealth = maxHealth;
        Defense = defense;
        MoveSpeed = moveSpeed;
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
        DetectionRadius = detectionRadius;
        FollowDistance = followDistance;
    }
}
