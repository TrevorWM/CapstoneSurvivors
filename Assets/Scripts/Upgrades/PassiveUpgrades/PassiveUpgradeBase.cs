using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveUpgradeBase : MonoBehaviour
{
    [SerializeField]
    private PassiveUpgradeSO passiveUpgradeSO;

    private float upgradeModifyValue = 0;

    public PassiveUpgradeSO PassiveUpgradeSO { get => passiveUpgradeSO; }

    /// <summary>
    /// Changes the value to increase a stat by depending on the rarity of the upgradeType.
    /// </summary>
    /// <param name="rolledUpgradeRarity"></param>
    private void InitializeUpgradeValue(UpgradeRarity rolledUpgradeRarity)
    {
        switch(rolledUpgradeRarity)
        {
            case UpgradeRarity.Common:
                upgradeModifyValue = PassiveUpgradeSO.CommonUpgradeAmount;
                break;
            case UpgradeRarity.Uncommon:
                upgradeModifyValue = PassiveUpgradeSO.UncommonUpgradeAmount;
                break;
            case UpgradeRarity.Rare:
                upgradeModifyValue = PassiveUpgradeSO.RareUpgradeAmount;
                break;
            case UpgradeRarity.Legendary:
                upgradeModifyValue = PassiveUpgradeSO.LegendaryUpgradeAmount;
                break;
        }
    }

    /// <summary>
    /// Modifies the player's stats based on the stat type and rarity level of
    /// the upgradeType.
    /// </summary>
    /// <param name="playerStats"></param>
    /// <param name="rolledUpgradeRarity"></param>
    public void ModifyStat(CharacterStats playerStats, UpgradeRarity rolledUpgradeRarity)
    { 
        InitializeUpgradeValue(rolledUpgradeRarity);

        switch (PassiveUpgradeSO.StatToModify)
        {
            case Stat.MaxHealth:
                playerStats.MaxHealth += upgradeModifyValue;
                playerStats.CurrentHealth += upgradeModifyValue;
                break;
            case Stat.Defense:
                playerStats.Defense += upgradeModifyValue;
                break;
            case Stat.MoveSpeed:
                playerStats.MoveSpeed += upgradeModifyValue;
                break;
            case Stat.BaseDamage:
                playerStats.BaseDamage += upgradeModifyValue;
                break;
            case Stat.AttacksPerSecond:
                playerStats.AttacksPerSecond += upgradeModifyValue;
                break;
            case Stat.CriticalChance:
                playerStats.CriticalChance += upgradeModifyValue;
                break;
            case Stat.CriticalDamageBonus:
                playerStats.CriticalDamageMultiplier += upgradeModifyValue;
                break;
            case Stat.WaterAffinity:
                playerStats.WaterAffinity += upgradeModifyValue;
                break;
            case Stat.FireAffinity:
                playerStats.FireAffinity += upgradeModifyValue;
                break;
            case Stat.NatureAffinity:
                playerStats.NatureAffinity += upgradeModifyValue;
                break;
            case Stat.CooldownReduction:
                playerStats.CooldownReduction += upgradeModifyValue;
                break;
        }
    }

    public string DisplayText()
    {
        // add space after each capital
        string upgradeName = Regex.Replace(passiveUpgradeSO.UpgradeName, "([A-Z])", " $1");
        // remove text in paranthesis
        upgradeName = Regex.Replace(upgradeName, "\\([^()]*\\)", "");
        // remove "passive"
        upgradeName = Regex.Replace(upgradeName, "Passive", "");

        return upgradeName;
    }
}
