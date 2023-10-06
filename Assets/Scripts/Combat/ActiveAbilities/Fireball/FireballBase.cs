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
        if (rarity == UpgradeRarity.Rare || rarity == UpgradeRarity.Legendary)
        {
            aoeScale = new Vector3(3,3,1);
        }
        else
        {
            aoeScale = new Vector3(2,2,1);
        }
    }

    public override void InitializeRarityBasedStats(UpgradeRarity rolledUpgradeRarity)
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
