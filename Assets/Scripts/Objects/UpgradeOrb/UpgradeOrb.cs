using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOrb : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UpgradeOrbSO upgradeOrbSO;

    [SerializeField] 
    private GameObject interactHint;

    [SerializeField]
    private bool testing;

    [SerializeField]
    private GameObject playerFallback;

    private GameObject playerToUpgrade;

    public UpgradeMenu upgradeUI;

    private CharacterStats playerStats;
    private PlayerControls playerControls;

    private IUpgrade chosenUpgrade;


    // Start is called before the first frame update
    void Start()
    {
        interactHint.SetActive(false);
        if (testing) InitializeOrb(playerFallback);
    }

    public void InitializeOrb(GameObject playerObject)
    {
        playerToUpgrade = playerObject;
        playerStats = playerToUpgrade.GetComponent<CharacterStats>();
        playerControls = playerToUpgrade.GetComponent<PlayerControls>();
    }

    public void OnInteract()
    {
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
            (GameObject upgradePrefab, UpgradeRarity rolledRarity) = upgradeOrbSO.RollActiveUpgrade();
            Debug.LogFormat("UpgradeOrb rolled a {0} {1}", rolledRarity, upgradePrefab);
            
            GameObject upgrade = Instantiate(upgradePrefab);
            ActiveAbilityBase upgradeBase = upgrade.GetComponent<ActiveAbilityBase>();

            //Replace Random bit with the index for the hotkey you want
            int abilityHotkey = UnityEngine.Random.Range(0, 3);

            upgradeBase.AddAbilityToPlayer(playerControls, rolledRarity, upgrade, abilityHotkey);
        }
        */
        HandleUI();
    }

    /// <summary>
    /// Handles all the Upgrade UI elements
    /// </summary>
    private void HandleUI()
    {
        // stop time for pause state, disable controls so player cannot activate orb multiple times
        playerControls.enabled = false;
        Time.timeScale = 0.0f;

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
            ActiveUpgrade active = chosenUpgrade as ActiveUpgrade;
            GameObject abilityInstance = Instantiate(active.UpgradePrefab);

            ActiveAbilityBase upgradeBase = abilityInstance.GetComponent<ActiveAbilityBase>();

            //Replace Random bit with the index for the hotkey you want
            int abilityHotkey = UnityEngine.Random.Range(0, 3);

            upgradeBase.AddAbilityToPlayer(playerControls, active.Rarity, abilityInstance, abilityHotkey);

        }


        //re-enable time, re-enable controls
        playerControls.enabled = true;
        Time.timeScale = 1.0f;

        // delete orb
        if (!testing)
        {
            this.gameObject.SetActive(false);
            interactHint.SetActive(false);
        }
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
