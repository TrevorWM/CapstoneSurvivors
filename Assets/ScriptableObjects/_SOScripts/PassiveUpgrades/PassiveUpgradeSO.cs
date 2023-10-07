using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveUpgradeSO", menuName = "ScriptableObjects/Upgrades/PassiveUpgrade", order = 1)]
public class PassiveUpgradeSO : ScriptableObject
{
    [Header("=== General Upgrade Info ===")]
    [SerializeField]
    private string upgradeName;

    [SerializeField]
    private Stat statToModify;

    [Header("=== Rarity Specific Info ===")]
    [SerializeField]
    private float commonUpgradeAmount;

    [SerializeField]
    private float uncommonUpgradeAmount;

    [SerializeField]
    private float rareUpgradeAmount;

    [SerializeField]
    private float legendaryUpgradeAmount;

    [SerializeField]
    private string description;

    [SerializeField]
    private Sprite[] spriteArray;

    private Sprite currentSprite;


    public float CommonUpgradeAmount { get => commonUpgradeAmount; set => commonUpgradeAmount = Mathf.Max(0, value); }
    public float UncommonUpgradeAmount { get => uncommonUpgradeAmount; set => uncommonUpgradeAmount = Mathf.Max(0, value); }
    public float RareUpgradeAmount { get => rareUpgradeAmount; set => rareUpgradeAmount = Mathf.Max(0, value); }
    public float LegendaryUpgradeAmount { get => legendaryUpgradeAmount; set => legendaryUpgradeAmount = Mathf.Max(0, value); }
    public Stat StatToModify { get => statToModify; set => statToModify = value; }
    public string UpgradeName { get => upgradeName; }
    public string Description { get => description; set => description = value; }
    public Sprite[] SpriteArray { get => spriteArray; set => spriteArray = value;  }
    public Sprite Sprite { get => currentSprite; set => currentSprite = value; }

    private void OnValidate()
    {
        CommonUpgradeAmount = commonUpgradeAmount;
        UncommonUpgradeAmount = uncommonUpgradeAmount;
        RareUpgradeAmount = rareUpgradeAmount;
        LegendaryUpgradeAmount = legendaryUpgradeAmount;
        SpriteArray = spriteArray;
    }
}

public enum Stat
{
    MaxHealth,
    CurrentHealth,
    Defense,
    MoveSpeed,
    BaseDamage,
    AttacksPerSecond,
    CriticalChance,
    CriticalDamageBonus,
    WaterAffinity,
    FireAffinity,
    NatureAffinity,
    CooldownReduction,
}
