using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlamethrowerBase : ActiveAbilityBase
{
    
    private void InitializeRarityEffectTimeScale(UpgradeRarity rarity)
    {
        switch (rarity)
        {
            case UpgradeRarity.Common:
                EffectTime = 2;
                break;
            case UpgradeRarity.Uncommon:
                EffectTime = 3;
                break;
            case UpgradeRarity.Rare:
                EffectTime = 4;
                break;
            case UpgradeRarity.Legendary:
                EffectTime = 5;
                break;
        }
    }
    

    public override void InitializeRarityBasedStats(UpgradeRarity rolledUpgradeRarity)
    {
        base.InitializeRarityBasedStats(rolledUpgradeRarity);
        InitializeRarityEffectTimeScale(rolledUpgradeRarity);
    }

    
    
}
