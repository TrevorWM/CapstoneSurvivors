using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveUpgradeSO", menuName = "ScriptableObjects/Upgrades/PassiveUpgrade", order = 1)]
public class PassiveUpgradeSO : ScriptableObject
{
    [SerializeField]
    private string upgradeName;

    [SerializeField]
    private Stat statToModify;

    [SerializeField]
    private float commonUpgradeAmount;

    [SerializeField]
    private float uncommonUpgradeAmount;

    [SerializeField]
    private float rareUpgradeAmount;

    [SerializeField]
    private float legendaryUpgradeAmount;


    public float CommonUpgradeAmount { get => commonUpgradeAmount; set => commonUpgradeAmount = Mathf.Max(0, value); }
    public float UncommonUpgradeAmount { get => uncommonUpgradeAmount; set => uncommonUpgradeAmount = Mathf.Max(0, value); }
    public float RareUpgradeAmount { get => rareUpgradeAmount; set => rareUpgradeAmount = Mathf.Max(0, value); }
    public float LegendaryUpgradeAmount { get => legendaryUpgradeAmount; set => legendaryUpgradeAmount = Mathf.Max(0, value); }
    public Stat StatToModify { get => statToModify; set => statToModify = value; }
    public string UpgradeName { get => upgradeName; }

    private void OnValidate()
    {
        CommonUpgradeAmount = commonUpgradeAmount;
        UncommonUpgradeAmount = uncommonUpgradeAmount;
        RareUpgradeAmount = rareUpgradeAmount;
        LegendaryUpgradeAmount = legendaryUpgradeAmount;
    }
}

public enum Stat
{
    MaxHealth,
    Defense,
    MoveSpeed,
    BaseDamage,
    AttacksPerSecond,
    CriticalChance,
    CriticalDamageBonus,
    WaterAffinity,
    FireAffinity,
    NatureAffinity,
    CooldownReduction,
}
