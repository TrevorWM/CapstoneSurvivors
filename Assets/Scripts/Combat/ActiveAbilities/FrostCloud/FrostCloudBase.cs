using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FrostCloudBase : ActiveAbilityBase
{
    
    private void InitializeRarityEffectTimeScale(UpgradeRarity rarity)
    {
        switch (rarity)
        {
            case UpgradeRarity.Common:
                EffectTime = 3f;
                break;
            case UpgradeRarity.Uncommon:
                EffectTime = 5f;
                break;
            case UpgradeRarity.Rare:
                EffectTime = 7f;
                break;
            case UpgradeRarity.Legendary:
                EffectTime = 10f;
                break;
        }
    }
    

    public override void InitializeRarityBasedStats(UpgradeRarity rolledUpgradeRarity)
    {
        base.InitializeRarityBasedStats(rolledUpgradeRarity);
        InitializeRarityEffectTimeScale(rolledUpgradeRarity);
    }

    
    public override void ApplyEffectToTarget(AttackPayload payload, Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Logic to show poison effect wil go here
            Transform target = collision.gameObject.transform;
            
            SpriteRenderer targetSprite = target.GetComponentInChildren<SpriteRenderer>();
            if (targetSprite != null) targetSprite.color = Color.blue;

            
        }
    }
    
}
