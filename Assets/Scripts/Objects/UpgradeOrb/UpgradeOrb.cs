using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOrb : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UpgradeOrbSO upgradeOrbSO;

    [SerializeField] 
    private GameObject interactHint;

    // Start is called before the first frame update
    void Start()
    {
        interactHint.SetActive(false);
    }


    public void OnInteract()
    {
        string upgradeString = upgradeOrbSO.RollUpgrade();
        Debug.LogFormat("UpgradeOrb rolled a {0}", upgradeString);
        this.gameObject.SetActive(false);
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
