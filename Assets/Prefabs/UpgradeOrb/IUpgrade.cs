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
        throw new System.NotImplementedException();
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

        return description;
    }
}

public enum UpgradeCategory
{
    Passive = 0,
    Active = 1,
}

