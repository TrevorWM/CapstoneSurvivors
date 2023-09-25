using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveAbilityBase : MonoBehaviour
{
    [SerializeField]
    private ActiveAbilitySO abilitySO;

    private ProjectileBase projectileScript;

    private UpgradeRarity abilityRarity;

    private float damageModifierValue = 0f;

    private bool onCooldown = false;

    public ActiveAbilitySO ActiveAbilitySO { get => abilitySO; }
    public float DamageModifierValue { get => damageModifierValue; }
    public bool OnCooldown { get => onCooldown; }

    private void Start()
    {
        projectileScript = GetComponent<ProjectileBase>();
    }

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
        abilityRarity = rolledAbilityRarity;

        // This destroys the ability instance already in the hotkey if it exists
        if (playerControls.CurrentAbilities[abilityIndex] != null) Destroy(playerControls.CurrentAbilities[abilityIndex].gameObject);
        
        // Set the position to the player position so that we shoot from the right spot. 
        abilityInstance.transform.parent = playerControls.gameObject.transform;
        abilityInstance.transform.position = playerControls.gameObject.transform.position;

        playerControls.CurrentAbilities[abilityIndex] = abilityInstance.gameObject;
    }

    // These functions allow us to have cooldowns for each ability be tied to the ability itself.
    public void StartCooldown(float duration)
    {
        onCooldown = true;
        StartCoroutine(CooldownTimer(duration));
    }

    private IEnumerator CooldownTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        onCooldown = false;
    }
}
