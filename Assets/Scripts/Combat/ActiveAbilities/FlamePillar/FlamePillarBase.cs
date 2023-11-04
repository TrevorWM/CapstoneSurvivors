using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePillarBase : ActiveAbilityBase
{
    [SerializeField]
    private OnHitEffect onHitEffect;

    public override void InitializeRarityBasedStats(UpgradeRarity rolledUpgradeRarity)
    {
        base.InitializeRarityBasedStats(rolledUpgradeRarity);
        EffectTime = this.ActiveAbilitySO.EffectTime;
    }

    private void OnEnable()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override void SpawnOnHitEffect(AttackPayload payload, Transform hitLocation)
    {
        OnHitEffect effectInstance = Instantiate(onHitEffect);
        effectInstance.ActivateEffect(payload, hitLocation, EffectTime);
    }
}
