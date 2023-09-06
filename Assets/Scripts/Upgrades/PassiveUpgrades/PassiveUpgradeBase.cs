using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveUpgradeBase : MonoBehaviour
{
    [SerializeField]
    private PassiveUpgradeSO passiveUpgradeSO;

    private float upgradeModifyValue = 0;

    /// <summary>
    /// Changes the value to increase a stat by depending on the rarity of the upgrade.
    /// </summary>
    /// <param name="rolledUpgradeRarity"></param>
    private void InitializeUpgradeValue(UpgradeRarity rolledUpgradeRarity)
    {
        switch(rolledUpgradeRarity)
        {
            case UpgradeRarity.Common:
                upgradeModifyValue = passiveUpgradeSO.CommonUpgradeAmount;
                break;
            case UpgradeRarity.Uncommon:
                upgradeModifyValue = passiveUpgradeSO.UncommonUpgradeAmount;
                break;
            case UpgradeRarity.Rare:
                upgradeModifyValue = passiveUpgradeSO.RareUpgradeAmount;
                break;
            case UpgradeRarity.Legendary:
                upgradeModifyValue = passiveUpgradeSO.LegendaryUpgradeAmount;
                break;
        }
    }

    /// <summary>
    /// Modifies the player's stats based on the stat type and rarity level of
    /// the upgrade.
    /// </summary>
    /// <param name="playerStats"></param>
    /// <param name="rolledUpgradeRarity"></param>
    public void ModifyStat(CharacterStats playerStats, UpgradeRarity rolledUpgradeRarity)
    { 
        InitializeUpgradeValue(rolledUpgradeRarity);

        switch (passiveUpgradeSO.StatToModify)
        {
            case Stat.MaxHealth:
                playerStats.MaxHealth += upgradeModifyValue;
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
                playerStats.CriticalDamageBonus += upgradeModifyValue;
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
}
