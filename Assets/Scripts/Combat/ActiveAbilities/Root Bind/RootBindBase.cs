using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RootBindBase : ActiveAbilityBase
{
    

    public override void InitializeRarityBasedStats(UpgradeRarity rolledUpgradeRarity)
    {
        base.InitializeRarityBasedStats(rolledUpgradeRarity);
        EffectTime = this.ActiveAbilitySO.EffectTime;
    }


    
}
