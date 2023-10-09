using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonDartBase : ActiveAbilityBase
{
    
    private void InitializeRarityDotTimeScale(UpgradeRarity rarity)
    {
        switch (rarity)
        {
            case UpgradeRarity.Common:
                dotTime = 5;
                break;
            case UpgradeRarity.Uncommon:
                dotTime = 6;
                break;
            case UpgradeRarity.Rare:
                dotTime = 7;
                break;
            case UpgradeRarity.Legendary:
                dotTime = 9;
                break;
        }
    }

    protected override void InitializeRarityBasedStats(UpgradeRarity rolledUpgradeRarity)
    {
        base.InitializeRarityBasedStats(rolledUpgradeRarity);
        InitializeRarityDotTimeScale(rolledUpgradeRarity);
    }


    public override void ApplyEffectToTarget(AttackPayload payload, Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Logic to show poison effect wil go here
            Transform target = collision.gameObject.transform;
            
            SpriteRenderer targetSprite = target.GetComponentInChildren<SpriteRenderer>();
            if (targetSprite != null) targetSprite.color = Color.green;
        }
    }
}
