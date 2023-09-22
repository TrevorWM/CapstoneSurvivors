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
        HandleUI();

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
        // disable player controls and stop movement while UI is open
        playerControls.enabled = false;
        playerControls.StopMovement();

        // show menu get upgrades, and send it to the ui
        upgradeUI.ShowUpgradeMenu();
        IUpgrade[] upgrades = new IUpgrade[3];
        for (int i = 0; i < 3; i++)
        {
            upgrades[i] = upgradeOrbSO.RollUpgrade();
        }
        upgradeUI.SetUpgrades(upgrades);
    }

    /// <summary>
    /// called by UIHoverEffect when an upgrade is chosen
    /// applies the upgrade to the player
    /// </summary>
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
        
        //re-enable player controls
        playerControls.enabled = true;
    }

    /// <summary>
    /// called by UIHoverEffect, sets the upgade that was chosen in the UI
    /// </summary>
    /// <param name="selected"></param>
    public void SetSelectedUpgrade(IUpgrade selected)
    {
        chosenUpgrade = selected;
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
