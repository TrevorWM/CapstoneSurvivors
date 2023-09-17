using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = "UpgradeOrbSO", menuName = "ScriptableObjects/Upgrades/UpgradeOrb", order = 0)]
public class UpgradeOrbSO : ScriptableObject
{
    [Header("=== Rarity Weights ===")]
    [SerializeField]
    private int commonWeight;

    [SerializeField]
    private int uncommonWeight;

    [SerializeField]
    private int rareWeight;

    [SerializeField]
    private int legendaryWeight;

    [SerializeField]
    private int minimumWeight;

    [Header("=== Upgrade Lists ===")]
    //Change this to Upgrade objects once implemented
    [SerializeField, SerializeReference]
    private PassiveUpgradeBase[] passiveUpgradeList;

    public int CommonWeight 
    {
        get => commonWeight;
        set => commonWeight = Mathf.Max(minimumWeight, value);
    }

    public int UncommonWeight
    {
        get => uncommonWeight;
        set => uncommonWeight = Mathf.Max(minimumWeight, value);
    }
    public int RareWeight
    { 
        get => rareWeight;
        set => rareWeight = Mathf.Max(minimumWeight, value);
    }
    public int LegendaryWeight
    { 
        get => legendaryWeight; 
        set => legendaryWeight = Mathf.Max(minimumWeight, value);
    }

    public void OnValidate()
    {
        CommonWeight = commonWeight;
        UncommonWeight = uncommonWeight;
        RareWeight = rareWeight;
        LegendaryWeight = legendaryWeight;
    }

    

    public Upgrade RollUpgrade()
    {
        if (passiveUpgradeList.Length > 0)
        {
            Upgrade upgrade = new()
            {
                Rarity = RollRarity(),
                UpgradeType = passiveUpgradeList[UnityEngine.Random.Range(0, passiveUpgradeList.Length)]
            };
            return (upgrade);
        } else
        {
            throw new Exception("No upgrades in list");
        }
    }

    private UpgradeRarity RollRarity()
    {
        int totalWeight = GetTotalWeight();
        int[] weightArray = GetWeightArray();
        int lootRoll = UnityEngine.Random.Range(1,totalWeight+1);

        for (int i = 0; i < weightArray.Length; i++)
        {
            lootRoll -= weightArray[i];

            if (lootRoll <= 0)
            {
                return (UpgradeRarity)i;
            }
        }
        return UpgradeRarity.None;
    }

    private int GetTotalWeight()
    {
        return commonWeight + uncommonWeight + rareWeight + legendaryWeight;
    }

    private int[] GetWeightArray()
    {
        int[] weightArray = { commonWeight, uncommonWeight, rareWeight, legendaryWeight };
        return weightArray;
    }
}

public class Upgrade
{
    private PassiveUpgradeBase upgradeType;
    private UpgradeRarity rarity;

    public PassiveUpgradeBase UpgradeType { get => upgradeType; set => upgradeType = value; }
    public UpgradeRarity Rarity { get => rarity; set => rarity = value; }

    public override string ToString()
    {
        return "Upgrade: " + upgradeType + ", Rarity: " + Rarity;
    }

    public string DisplayName()
    {
        // add space after each capital
        string upgradeName = Regex.Replace(upgradeType.ToString(), "([A-Z])", " $1");
        // remove text in paranthesis
        upgradeName = Regex.Replace(upgradeName, "\\([^()]*\\)", "");
        // remove "passive"
        upgradeName = Regex.Replace(upgradeName, "Passive", "");

        return upgradeName;
    }
}
public enum UpgradeRarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Legendary = 3,
    None = 4,
}
