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

    // Start is called before the first frame update
    void Start()
    {
        interactHint.SetActive(false);

    }

    public void InitializeOrb(GameObject playerObject)
    {
        playerStats = playerObject.GetComponent<CharacterStats>();
    }

    public void OnInteract()
    {
        (PassiveUpgradeBase upgrade, UpgradeRarity rolledRarity) = upgradeOrbSO.RollUpgrade();
        Debug.LogFormat("UpgradeOrb rolled a {0} {1}", rolledRarity, upgrade.PassiveUpgradeSO.UpgradeName);
        upgrade.ModifyStat(playerStats, rolledRarity);
        playerStats.PrintStatSheet();

        if (!testing)
        {
            this.gameObject.SetActive(false);
            interactHint.SetActive(false);
        }
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
