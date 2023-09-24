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
    private GameObject playerToUpgrade;

    [SerializeField] 
    private GameObject interactHint;

    [SerializeField]
    private bool testing;

    [SerializeField]
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

            CloseUI();
        } else
        {
            // handle setting active ability...
            ActiveUpgrade active = chosenUpgrade as ActiveUpgrade;

            inputPrompt.SetActive(true);

            StartCoroutine(GetKey(active));


        }
    }

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
    /// Freezes the game and waits for a user input key
    /// REF: https://forum.unity.com/threads/waiting-for-input-in-a-custom-function.474387/
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetKey(ActiveUpgrade active)
    {
        bool done = false;
        while (!done)
        {
            Debug.Log("nothing chosen...");
            if (Input.GetKeyDown(KeyCode.Q))
            {
                done = true;
                Debug.Log("Q chosen!");
                QAbility.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
                CloseUI();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                done = true;
                Debug.Log("E chosen!");
                EAbility.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
                CloseUI();
            } 
            else  if (Input.GetMouseButtonDown(1))
            {
                done = true;
                Debug.Log("RMB chosen!");
                RMBAbility.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
                CloseUI();
            }
            yield return null;
        }
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
