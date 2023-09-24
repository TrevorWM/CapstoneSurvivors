using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveAbilitySO", menuName = "ScriptableObjects/Upgrades/ActiveAbilitySO", order = 2)]
public class ActiveAbilitySO : ScriptableObject
{

    [Header("=== General Ability Info ===")]
    [SerializeField]
    private string abilityName;

    [SerializeField]
    private string abilityDescription;

    [SerializeField]
    private Sprite abilityIcon;

    [SerializeField]
    private float abilityCooldown;

    [SerializeField]
    private int dotTime;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private ElementType abilityElement;

    [Header("=== Rarity Specific Info ===")]
    [SerializeField]
    private float commonDamageModifier;

    [SerializeField]
    private float uncommonDamageModifier;

    [SerializeField]
    private float rareDamageModifier;

    [SerializeField]
    private float legendaryDamageModifier;

    public string AbilityName { get => abilityName; }
    public string AbilityDescription { get => abilityDescription; }
    public Sprite AbilityIcon { get => abilityIcon; }
    public float AbilityCooldown { get => abilityCooldown; }
    public int DotTime { get => dotTime; private set => dotTime = Mathf.Max(0, value); }
    public float ProjectileSpeed { get => projectileSpeed; }
    public float CommonDamageModifier { get => commonDamageModifier; set => commonDamageModifier = Mathf.Max(0, value); }
    public float UncommonDamageModifier { get => uncommonDamageModifier; set => uncommonDamageModifier = Mathf.Max(0, value); }
    public float RareDamageModifier { get => rareDamageModifier; set => rareDamageModifier = Mathf.Max(0, value); }
    public float LegendaryDamageModifier { get => legendaryDamageModifier; set => legendaryDamageModifier = Mathf.Max(0, value); }
    public ElementType AbilityElement { get => abilityElement; }

    private void OnValidate()
    {
        DotTime = dotTime;
        CommonDamageModifier = commonDamageModifier;
        UncommonDamageModifier = uncommonDamageModifier;
        RareDamageModifier = rareDamageModifier;
        LegendaryDamageModifier = legendaryDamageModifier;
    }


}
