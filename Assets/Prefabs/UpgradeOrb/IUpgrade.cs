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

    public string getInfo();

}

public class ActiveUpgrade : IUpgrade
{

    private ActiveAbilityBase upgradeType;
    private UpgradeRarity rarity;

    public UpgradeCategory Category => UpgradeCategory.Active;

    public UpgradeRarity Rarity { get => rarity; set => rarity = value; }
    public ActiveAbilityBase UpgradeType { get => upgradeType; set => upgradeType = value; }

    public string DisplayText()
    {
        return UpgradeType.ActiveAbilitySO.AbilityName;
    }

    // will be used to get the description text that appears when hovering over the upgrade
    public string getInfo()
    {
        return UpgradeType.ActiveAbilitySO.AbilityDescription;
    }
}

public class PassiveUpgrade : IUpgrade
{
    private PassiveUpgradeBase upgradeType;
    private UpgradeRarity rarity;

    public PassiveUpgradeBase UpgradeType { get => upgradeType; set => upgradeType = value; }
    public UpgradeRarity Rarity { get => rarity; set => rarity = value; }
    UpgradeCategory IUpgrade.Category => UpgradeCategory.Passive;

    public string DisplayText()
    {
        return UpgradeType.PassiveUpgradeSO.UpgradeName;
    }

    // will be used to get the description text that appears when hovering over the upgrade
    public string getInfo()
    {
        throw new System.NotImplementedException();
    }

    public PassiveUpgradeBase GetBase() => upgradeType;

}

public enum UpgradeCategory
{
    Passive = 0,
    Active = 1,
}

