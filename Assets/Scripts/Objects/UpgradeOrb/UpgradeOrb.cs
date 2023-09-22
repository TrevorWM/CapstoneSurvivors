using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOrb : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UpgradeOrbSO upgradeOrbSO;

    [SerializeField]
    private GameObject playerToUpgrade;

    [SerializeField] 
    private GameObject interactHint;

    [SerializeField]
    private bool testing;

    [SerializeField]
    public UpgradeMenu upgradeUI;
    private CharacterStats playerStats;
    private PlayerControls playerControls;

    private IUpgrade chosenUpgrade;


    // Start is called before the first frame update
    void Start()
    {
        interactHint.SetActive(false);
        playerStats = playerToUpgrade.GetComponent<CharacterStats>();
        playerControls = playerToUpgrade.GetComponent<PlayerControls>();
    }

    public void InitializeOrb(GameObject playerObject)
    {
        playerStats = playerObject.GetComponent<CharacterStats>();
    }

    public void OnInteract()
    {
        /*
        IUpgrade upgrade = upgradeOrbSO.RollUpgrade();
        Debug.LogFormat("UpgradeOrb rolled a {0} {1}", upgrade.Rarity, upgrade.UpgradeType);
        upgrade.UpgradeType.ModifyStat(playerStats, upgrade.Rarity);
        playerStats.PrintStatSheet();
        */

        HandleUI();
        /*
        if (IsUpgradePassive())
        {
            (PassiveUpgradeBase upgrade, UpgradeRarity rolledRarity) = upgradeOrbSO.RollPassiveUpgrade();
            Debug.LogFormat("UpgradeOrb rolled a {0} {1}", rolledRarity, upgrade.PassiveUpgradeSO.UpgradeName);
            upgrade.ModifyStat(playerStats, rolledRarity);
            playerStats.PrintStatSheet();
        }
        else
        {
            (ActiveAbilityBase upgrade, UpgradeRarity rolledRarity) = upgradeOrbSO.RollActiveUpgrade();
            Debug.LogFormat("UpgradeOrb rolled a {0} {1}", rolledRarity, upgrade.ActiveAbilitySO.AbilityName);
            upgrade.AddAbilityToPlayer(playerControls, rolledRarity);
        }
        */
        

        if (!testing)
        {
            this.gameObject.SetActive(false);
            interactHint.SetActive(false);
        }
    }

    /// <summary>
    /// Handles all the Upgrade UI elements
    /// </summary>
    private void HandleUI()
    {
        upgradeUI.ShowUpgradeMenu();
        IUpgrade[] upgrades = new IUpgrade[3];
        for (int i = 0; i < 3; i++)
        {
            upgrades[i] = upgradeOrbSO.RollUpgrade();
        }

        upgradeUI.GetUpgrade(upgrades);
    }

    public void FinalizeChoice()
    {
        upgradeUI.HideUpgradeMenu();
        Debug.Log("chosenUpgrade: " + chosenUpgrade);

        if (chosenUpgrade.Category == UpgradeCategory.Passive)
        {
            PassiveUpgrade passive = chosenUpgrade as PassiveUpgrade;

            passive.GetBase().ModifyStat(playerStats, chosenUpgrade.Rarity);
            playerStats.PrintStatSheet();
        } else
        {
            // handle setting active ability...
        }

    }

    public void SetSelectedUpgrade(IUpgrade selected)
    {
        chosenUpgrade = selected;
        Debug.Log("Selected: " + selected);
    }

    private bool IsUpgradePassive()
    {
        float roll = UnityEngine.Random.Range(0f, 1f);
        return roll <= upgradeOrbSO.PassiveChance;
    }

    public void ToggleInteractUI()
    {
        interactHint.SetActive(!interactHint.activeInHierarchy);   
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleInteractUI();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleInteractUI();
        }
    }

}
