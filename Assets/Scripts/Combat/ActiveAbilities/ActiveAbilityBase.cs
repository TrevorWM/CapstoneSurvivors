using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// Changes the value to increase a stat by depending on the rarity of the upgrade.
    /// </summary>
    /// <param name="rolledUpgradeRarity"></param>
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

    /// <summary>
    /// Adds the ability to the player's loadout once we have the hotkeys implemented.
    /// </summary>
    /// <param name="playerControls"></param>
    /// <param name="rolledAbilityRarity"></param>
    public void AddAbilityToPlayer(PlayerControls playerControls, UpgradeRarity rolledAbilityRarity, GameObject abilityInstance, int abilityIndex)
    {
        InitializeDamageModifier(rolledAbilityRarity);
        Debug.Log("Still need to add the ability to the player loadout! :(");
        //TODO: Add logic to place ability on the player
        abilityInstance.transform.parent = playerControls.gameObject.transform;
        abilityInstance.transform.position = playerControls.gameObject.transform.position;
        playerControls.CurrentAbilities[abilityIndex] = abilityInstance.gameObject.GetComponent<ShootProjectile>();
    }
}
