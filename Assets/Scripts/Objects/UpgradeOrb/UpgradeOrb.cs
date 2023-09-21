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

    private CharacterStats playerStats;
    private PlayerControls playerControls;

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
            ActiveAbilityBase upgradeBase = upgradePrefab.GetComponent<ActiveAbilityBase>();

            //Replace Random bit with the index for the hotkey you want
            int abilityHotkey = UnityEngine.Random.Range(0, 3);

            upgradeBase.AddAbilityToPlayer(playerControls, rolledRarity, upgrade, abilityHotkey);
        }
        

        if (!testing)
        {
            this.gameObject.SetActive(false);
            interactHint.SetActive(false);
        }
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
