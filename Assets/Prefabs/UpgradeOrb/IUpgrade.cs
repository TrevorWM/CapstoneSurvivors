using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Allows upgrades to be dealt with as a singular type
/// </summary>
public interface IUpgrade
{
    UpgradeRarity Rarity { get; }
    UpgradeCategory Category { get; }

    public string DisplayText();

    public string GetDescription();

    public void InitializeRarityInformation();

}

public class ActiveUpgrade : IUpgrade
{

    private ActiveAbilityBase upgradeType;
    private GameObject upgradePrefab;
    private UpgradeRarity rarity;

    public UpgradeCategory Category => UpgradeCategory.Active;

    public UpgradeRarity Rarity { get => rarity; set => rarity = value; }
    public ActiveAbilityBase UpgradeType { get => upgradeType; set => upgradeType = value; }
    public GameObject UpgradePrefab { get => upgradePrefab; set => upgradePrefab = value; }

    public string DisplayText()
    {
        return UpgradeType.ActiveAbilitySO.AbilityName;
    }

    // will be used to get the description text that appears when hovering over the upgrade
    public string GetDescription()
    {
        string description = rarity + " ";
        description += upgradeType.ActiveAbilitySO.AbilityName + ": ";
        description += upgradeType.ActiveAbilitySO.AbilityElement + " type ability\n";
        description += upgradeType.ActiveAbilitySO.Description + "\n";

        description += "Deals ";

        switch (rarity) {

            case UpgradeRarity.Common:
                description += upgradeType.ActiveAbilitySO.CommonDamageModifier * 100;
                break;
            case UpgradeRarity.Uncommon:
                description += upgradeType.ActiveAbilitySO.UncommonDamageModifier * 100;
                break;
            case UpgradeRarity.Rare:
                description += upgradeType.ActiveAbilitySO.RareDamageModifier * 100;
                break;
            case UpgradeRarity.Legendary:
                description += upgradeType.ActiveAbilitySO.LegendaryDamageModifier * 100;
                break;
        }

        description += "% of base damage";

        if (upgradeType.ActiveAbilitySO.DotTime > 0)
        {
            description += " per second for " + upgradeType.ActiveAbilitySO.DotTime + " seconds";
        }

        description += "\nCooldown: " + upgradeType.ActiveAbilitySO.AbilityCooldown + " seconds";

        return description;
    }

    public void InitializeRarityInformation()
    {
        upgradeType.InitializeRarityBasedStats(Rarity);
    }
}

public class PassiveUpgrade : IUpgrade
{
    private PassiveUpgradeBase upgradeType;
    private UpgradeRarity rarity;

    public PassiveUpgradeBase UpgradeType { get => upgradeType; set => upgradeType = value; }
    public UpgradeRarity Rarity { get => rarity; set => rarity = value; }
    UpgradeCategory IUpgrade.Category => UpgradeCategory.Passive;
    public PassiveUpgradeBase GetBase() => upgradeType;

    public string DisplayText()
    {
        return UpgradeType.PassiveUpgradeSO.UpgradeName;
    }
    
    // will be used to get the description text that appears when hovering over the upgrade
    public string GetDescription()
    {
        string description = rarity + " ";
        description += upgradeType.PassiveUpgradeSO.UpgradeName + ":\n";
        description += upgradeType.PassiveUpgradeSO.Description;
        float amount;

        switch (rarity)
        {
            case UpgradeRarity.Common:
                amount = upgradeType.PassiveUpgradeSO.CommonUpgradeAmount;
                description += " by " + (amount < 1 ? amount * 100 + "%" : amount);
                break;
            case UpgradeRarity.Uncommon:
                amount = upgradeType.PassiveUpgradeSO.UncommonUpgradeAmount;
                description += " by " + (amount < 1 ? amount * 100 + "%" : amount);
                break;
            case UpgradeRarity.Rare:
                amount = upgradeType.PassiveUpgradeSO.RareUpgradeAmount;
                description += " by " + (amount < 1 ? amount * 100 + "%" : amount);
                break;
            case UpgradeRarity.Legendary:
                amount = upgradeType.PassiveUpgradeSO.LegendaryUpgradeAmount;
                description += " by " + (amount < 1 ? amount * 100 + "%" : amount);
                break;
        }
        
        if (upgradeType.PassiveUpgradeSO.UpgradeName == "Critical Rate Up")
        {
            // for some reason critical rate is the only one where the SO is in percent rather than decimal
            description += "%";
        }

        return description;
    }

    public void InitializeRarityInformation()
    {
        upgradeType.InitializeUpgradeValue(Rarity);
    }
}

public enum UpgradeCategory
{
    Passive = 0,
    Active = 1,
}

