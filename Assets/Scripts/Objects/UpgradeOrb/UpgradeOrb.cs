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
            //TODO: Logic to place ability in hotbar
        }
        

        if (!testing)
        {
            this.gameObject.SetActive(false);
            interactHint.SetActive(false);
        }
    }

    private bool IsUpgradePassive()
    {
        float roll = UnityEngine.Random.Range(0, 2);
        return roll > 0;
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
