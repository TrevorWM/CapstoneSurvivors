using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public interface IUpgrade
{
    UpgradeRarity Rarity { get; }
    UpgradeCategory Category { get; }

    /// <summary>
    /// Each upgrade type needs to at the very least be able to return a string 
    /// </summary>
    /// <returns></returns>
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
        /*
        // add space after each capital
        string upgradeName = Regex.Replace(upgradeType.ToString(), "([A-Z])", " $1");
        // remove text in paranthesis
        upgradeName = Regex.Replace(upgradeName, "\\([^()]*\\)", "");
        // remove "Active"
        upgradeName = Regex.Replace(upgradeName, "Active", "");
        */

        return UpgradeType.ActiveAbilitySO.AbilityName;
    }

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
        /*
        // add space after each capital
        string upgradeName = Regex.Replace(upgradeType.ToString(), "([A-Z])", " $1");
        // remove text in paranthesis
        upgradeName = Regex.Replace(upgradeName, "\\([^()]*\\)", "");
        // remove "passive"
        upgradeName = Regex.Replace(upgradeName, "Passive", "");
        */

        
        return UpgradeType.PassiveUpgradeSO.UpgradeName;
    }

    public string getInfo()
    {
        throw new System.NotImplementedException();
    }



    public override string ToString()
    {
        return "Upgrade: " + upgradeType + ", Rarity: " + Rarity;
    }
}

public enum UpgradeCategory
{
    Passive = 0,
    Active = 1,
}

