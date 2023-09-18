using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbilityBase : MonoBehaviour
{
    [SerializeField]
    private ActiveAbilitySO abilitySO;

    [SerializeField]
    private ProjectileBase projectileScript;

    private float damageModifierValue = 0f;

    public ActiveAbilitySO ActiveAbilitySO { get => abilitySO; }
    public float DamageModifierValue { get => damageModifierValue; }

    private void InitializeDamageModifier(UpgradeRarity rolledUpgradeRarity)
    {
        switch (rolledUpgradeRarity)
        {
            case UpgradeRarity.Common:
                damageModifierValue = ActiveAbilitySO.CommonDamageModifier;
                break;
            case UpgradeRarity.Uncommon:
                damageModifierValue = ActiveAbilitySO.UncommonDamageModifier;
                break;
            case UpgradeRarity.Rare:
                damageModifierValue = ActiveAbilitySO.RareDamageModifier;
                break;
            case UpgradeRarity.Legendary:
                damageModifierValue = ActiveAbilitySO.LegendaryDamageModifier;
                break;
        }
    }

    public void AddAbilityToPlayer(PlayerControls playerControls, UpgradeRarity rolledAbilityRarity)
    {
        InitializeDamageModifier(rolledAbilityRarity);
        Debug.Log("Still need to add the ability to the player loadout! :(");
        //TODO: Add logic to place ability on the player
    }
}
