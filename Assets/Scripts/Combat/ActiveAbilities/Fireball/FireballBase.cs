using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBase : ActiveAbilityBase
{
    [SerializeField]
    private OnHitEffect[] hitEffects;

    [SerializeField]
    private float hitEffectLifetime;

    private Vector3 aoeScale;

    public Vector3 AoeScale { get => aoeScale; }
    public float HitEffectLifetime { get => hitEffectLifetime; set => hitEffectLifetime = Mathf.Max(0,value); }

    private void InitializeRarityAoEScale(UpgradeRarity rarity)
    {
        if (rarity == UpgradeRarity.Legendary || rarity == UpgradeRarity.Legendary)
        {
            aoeScale = new Vector3(2.5f,2.5f,1);
        }
        else if (rarity == UpgradeRarity.Rare || rarity == UpgradeRarity.Legendary)
        {
            aoeScale = new Vector3(2f, 2f, 1);
        }
        else if (rarity == UpgradeRarity.Uncommon || rarity == UpgradeRarity.Legendary)
        {
            aoeScale = new Vector3(1.5f, 1.5f, 1);
        }
        else
        {
            aoeScale = new Vector3(1,1,1);
        }
    }

    protected override void InitializeRarityBasedStats(UpgradeRarity rolledUpgradeRarity)
    {
        base.InitializeRarityBasedStats(rolledUpgradeRarity);
        InitializeRarityAoEScale(rolledUpgradeRarity);
    }

    public override void SpawnOnHitEffect(AttackPayload payload, Transform hitLocation)
    {
        if (hitEffects.Length > 0)
        {
            foreach (OnHitEffect effect in hitEffects)
            {
                OnHitEffect effectInstance = Instantiate(effect);
                
                effectInstance.transform.localScale = Vector3.Scale(effectInstance.transform.localScale, aoeScale);
                effectInstance.ActivateEffect(payload, hitLocation, hitEffectLifetime);
            }
        }
    }
}
