using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string RollUpgrade()
    {
        if (passiveUpgradeList.Length > 0)
        {
            UpgradeRarity itemRarity = RollRarity();
            PassiveUpgradeBase upgrade = passiveUpgradeList[UnityEngine.Random.Range(0, passiveUpgradeList.Length)];
            return itemRarity.ToString() + " " + upgrade;
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

public enum UpgradeRarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Legendary = 3,
    None = 4,
}
