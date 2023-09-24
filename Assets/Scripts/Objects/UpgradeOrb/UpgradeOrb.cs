using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
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

    [SerializeField]
    private GameObject QAbility;
    [SerializeField]
    private GameObject RMBAbility;
    [SerializeField]
    private GameObject EAbility;
    [SerializeField]
    private GameObject inputPrompt;
    private char inputKey;

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
            // handle setting passive upgrade...
            PassiveUpgrade passive = chosenUpgrade as PassiveUpgrade;

            passive.GetBase().ModifyStat(playerStats, chosenUpgrade.Rarity);
            playerStats.PrintStatSheet();

            CloseUI();
        } else
        {
            // handle setting active ability...
            ActiveUpgrade active = chosenUpgrade as ActiveUpgrade;
            // show prompt
            inputPrompt.SetActive(true);

            // prompt user to put the ability on a specific key
            StartCoroutine(GetKey(active));
/*
            ActiveUpgrade active = chosenUpgrade as ActiveUpgrade;
            GameObject abilityInstance = Instantiate(active.UpgradePrefab);

            ActiveAbilityBase upgradeBase = abilityInstance.GetComponent<ActiveAbilityBase>();

            //Replace Random bit with the index for the hotkey you want
            int abilityHotkey = UnityEngine.Random.Range(0, 3);

            upgradeBase.AddAbilityToPlayer(playerControls, active.Rarity, abilityInstance, abilityHotkey);
            */

        }
    }

    /// <summary>
    /// Closes orb UI
    /// </summary>
    private void CloseUI()
    {
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
    /// Freezes the game and waits for a user input key, then applies the ability to the chosen key
    /// REF: https://forum.unity.com/threads/waiting-for-input-in-a-custom-function.474387/
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetKey(ActiveUpgrade active)
    {
        bool done = false;
        while (!done)
        {
            // You can set the ability to the key in here too, we have the ability and the key so we
            // just need the ability to bind it 
            if (Input.GetKeyDown(KeyCode.Q))
            {
                done = true;
                Debug.Log("Q chosen!");
                QAbility.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                done = true;
                Debug.Log("E chosen!");
                EAbility.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
            } 
            else  if (Input.GetMouseButtonDown(1)) //RMB
            {
                done = true;
                Debug.Log("RMB chosen!");
                RMBAbility.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
            }
            yield return null;
        }
        // close input prompt UI
        CloseUI();
        inputPrompt.SetActive(false);
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
