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

    public void ModifyStat(CharacterStatsSO characterStats, UpgradeRarity rolledUpgradeRarity)
    { 
        InitializeUpgradeValue(rolledUpgradeRarity);

        switch (passiveUpgradeSO.StatToModify)
        {
            case Stat.MaxHealth:
                characterStats.MaxHealth += upgradeModifyValue;
                break;
            case Stat.Defense:
                characterStats.Defense += upgradeModifyValue;
                break;
            case Stat.MoveSpeed:
                characterStats.MoveSpeed += upgradeModifyValue;
                break;
            case Stat.BaseDamage:
                characterStats.BaseDamage += upgradeModifyValue;
                break;
            case Stat.AttacksPerSecond:
                characterStats.AttacksPerSecond += upgradeModifyValue;
                break;
            case Stat.CriticalChance:
                characterStats.CriticalChance += upgradeModifyValue;
                break;
            case Stat.CriticalDamageBonus:
                characterStats.CriticalDamageBonus += upgradeModifyValue;
                break;
            case Stat.WaterAffinity:
                characterStats.WaterAffinity += upgradeModifyValue;
                break;
            case Stat.FireAffinity:
                characterStats.FireAffinity += upgradeModifyValue;
                break;
            case Stat.NatureAffinity:
                characterStats.NatureAffinity += upgradeModifyValue;
                break;
            case Stat.CooldownReduction:
                characterStats.CooldownReduction += upgradeModifyValue;
                break;
        }
    }
}
