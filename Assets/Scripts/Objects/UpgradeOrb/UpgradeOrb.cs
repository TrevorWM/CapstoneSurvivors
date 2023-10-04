using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent orbUsed;


    // Start is called before the first frame update
    void Start()
    {
        interactHint.SetActive(false);
    }

    public void InitializeOrb(GameObject playerObject)
    {
        playerToUpgrade = playerObject;
        playerStats = playerToUpgrade.GetComponent<CharacterStats>();
        playerControls = playerToUpgrade.GetComponent<PlayerControls>();
    }

    public void OnInteract()
    {
        if (playerToUpgrade == null) InitializeOrb(playerFallback);
        
        orbUsed?.Invoke();
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

            // get the player to create the UI and handle the rest
            playerControls.SetAbility(active);
            
            // delete orb
            if (!testing)
            {
                this.gameObject.SetActive(false);
                interactHint.SetActive(false);
            }
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
