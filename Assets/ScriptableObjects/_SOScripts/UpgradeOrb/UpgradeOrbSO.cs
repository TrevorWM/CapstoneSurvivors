using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
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

    [Header("=== Upgrade Information ===")]

    [SerializeField, Range(0,1)]
    private float passiveChance;

    [SerializeField, SerializeReference]
    private PassiveUpgradeBase[] passiveUpgradeList;

    [SerializeField, SerializeReference]
    private GameObject[] activeUpgradeList;

    [SerializeField]
    private PassiveUpgradeBase healthPotion;

    private int lastActiveIndex;
    private int lastPassiveIndex;

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
    public float PassiveChance { get => passiveChance; set => passiveChance = Mathf.Clamp01(value); }

    public void OnValidate()
    {
        CommonWeight = commonWeight;
        UncommonWeight = uncommonWeight;
        RareWeight = rareWeight;
        LegendaryWeight = legendaryWeight;
        PassiveChance = passiveChance;

    }

    

    public IUpgrade RollUpgrade()
    {
        int randomIndex;

        if (IsUpgradePassive())
        {
            randomIndex = RollIndexWithProtection(lastPassiveIndex, passiveUpgradeList.Length);

            IUpgrade upgrade = new PassiveUpgrade()
            {
                Rarity = RollRarity(),
                UpgradeType = passiveUpgradeList[randomIndex]
            };
            lastPassiveIndex = randomIndex;
            upgrade.InitializeRarityInformation();
            return (upgrade);
            
        } else
        {
            randomIndex = RollIndexWithProtection(lastActiveIndex, activeUpgradeList.Length);

            GameObject upgradePrefab = activeUpgradeList[UnityEngine.Random.Range(0, activeUpgradeList.Length)];

            IUpgrade upgrade = new ActiveUpgrade()
            {
                Rarity = RollRarity(),
                UpgradePrefab = upgradePrefab,
                UpgradeType = upgradePrefab.GetComponent<ActiveAbilityBase>(),
            };
            upgrade.InitializeRarityInformation();
            lastActiveIndex = randomIndex;
            return (upgrade);
            
        }
        
    }

    public IUpgrade RollHealthPotion(float healthPercent)
    {
        int healthRoll = UnityEngine.Random.Range(0, 101);
        Debug.Log("Roll: " + healthRoll + " Player Health%: " + healthPercent);

        if (healthRoll > healthPercent)
        {
            IUpgrade upgrade = new PassiveUpgrade()
            {
                Rarity = RollRarity(),
                UpgradeType = healthPotion,
            };
            upgrade.InitializeRarityInformation();
            return (upgrade);
        }

        return null;
    }

    private bool IsUpgradePassive()
    {
        float roll = UnityEngine.Random.Range(0f, 1f);
        return roll <= PassiveChance;
    }

    public (GameObject, UpgradeRarity) RollActiveUpgrade()
    {
        if (activeUpgradeList.Length > 0)
        {
            UpgradeRarity itemRarity = RollRarity();
            GameObject upgradePrefab = Instantiate(activeUpgradeList[UnityEngine.Random.Range(0, activeUpgradeList.Length)]);
            return (upgradePrefab, itemRarity);
        }
        else
        {
            throw new Exception("No Active Abilities in list");
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

    private int RollIndexWithProtection(int lastIndex, int arrayLength)
    {
        int randomIndex = UnityEngine.Random.Range(0, arrayLength);

        while (randomIndex == lastIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, arrayLength);
        }

        return randomIndex;
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
