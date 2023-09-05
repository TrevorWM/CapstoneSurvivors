using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOrb : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UpgradeOrbSO upgradeOrbSO;

    [SerializeField]
    private CharacterStatsSO playerStatsSO;

    [SerializeField] 
    private GameObject interactHint;

    [SerializeField]
    private bool testing;

    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        interactHint.SetActive(false);
    }


    public void OnInteract()
    {
        (PassiveUpgradeBase upgrade, UpgradeRarity rolledRarity) = upgradeOrbSO.RollUpgrade();
        Debug.LogFormat("UpgradeOrb rolled a {0}", upgrade);
        upgrade.ModifyStat(playerStatsSO, rolledRarity);
        if (!testing) this.gameObject.SetActive(false);
    }

    public void ToggleInteractUI()
    {
        interactHint.SetActive(!interactHint.activeInHierarchy);
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            ToggleInteractUI();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
            ToggleInteractUI();
        }
    }
}
