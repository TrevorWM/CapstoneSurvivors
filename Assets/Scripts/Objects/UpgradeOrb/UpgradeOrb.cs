using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOrb : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UpgradeOrbSO upgradeOrbSO;

    [SerializeField]
    private CharacterStats playerStats;

    [SerializeField] 
    private GameObject interactHint;

    [SerializeField]
    private bool testing;

    [SerializeField]
    public UpgradeMenu upgradeUI;

    // Start is called before the first frame update
    void Start()
    {
        interactHint.SetActive(false);
        Debug.Log(upgradeUI);
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
