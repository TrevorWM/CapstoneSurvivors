using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SortingLayer;

public class SetAbilityUI : MonoBehaviour
{

    [SerializeField]
    private GameObject playerToUpgrade;


    [SerializeField]
    private GameObject QBackground;
    [SerializeField]
    private GameObject RMBBackground;
    [SerializeField]
    private GameObject EBackground;

    [SerializeField]
    private Sprite CommonBackground;
    [SerializeField]
    private Sprite UncommonBackground;
    [SerializeField]
    private Sprite RareBackground;
    [SerializeField]
    private Sprite LegendaryBackground;

    [SerializeField]
    private GameObject QAbility;
    [SerializeField]
    private GameObject RMBAbility;
    [SerializeField]
    private GameObject EAbility;
    [SerializeField]
    private GameObject inputPrompt;
    [SerializeField]
    private GameObject healthBar;

    private CharacterStats playerStats;
    private PlayerControls playerControls;

    private IUpgrade chosenUpgrade;


    // Start is called before the first frame update
    void Start()
    {
        playerStats = playerToUpgrade.GetComponent<CharacterStats>();
        playerControls = playerToUpgrade.GetComponent<PlayerControls>();
    }


    public void SetAbility(ActiveUpgrade active)
    {
        healthBar.SetActive(false);
        inputPrompt.SetActive(true);
        StartCoroutine(GetKey(active));
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
            if (Input.GetKeyDown(KeyCode.Q)) // key 0
            {
                done = true;
                Debug.Log("Q chosen!");
                QAbility.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
                AddAbilityToPlayer(active, 0);
                SetBackgroundRarity(active.Rarity, QBackground);
            }
            else if (Input.GetKeyDown(KeyCode.E)) // key 1
            {
                done = true;
                Debug.Log("E chosen!");
                EAbility.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
                AddAbilityToPlayer(active, 1);
                SetBackgroundRarity(active.Rarity, EBackground);
            }
            else if (Input.GetMouseButtonDown(1)) // 2
            {
                done = true;
                Debug.Log("RMB chosen!");
                RMBAbility.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
                AddAbilityToPlayer(active, 2);
                SetBackgroundRarity(active.Rarity, RMBBackground);
            }
            yield return null;
        }
        // close input prompt UI
        CloseUI();
    }



    private void SetBackgroundRarity(UpgradeRarity rarity, GameObject abilityBackground)
    {
        switch (rarity)
        {
            case UpgradeRarity.Common:
                abilityBackground.GetComponent<UnityEngine.UI.Image>().sprite = CommonBackground;
                break;
            case UpgradeRarity.Uncommon:
                abilityBackground.GetComponent<UnityEngine.UI.Image>().sprite = UncommonBackground;
                break;
            case UpgradeRarity.Rare:
                abilityBackground.GetComponent<UnityEngine.UI.Image>().sprite = RareBackground;
                break;
            case UpgradeRarity.Legendary:
                abilityBackground.GetComponent<UnityEngine.UI.Image>().sprite = LegendaryBackground;
                break;

        }
    }


    private void AddAbilityToPlayer(ActiveUpgrade active, int key)
    {
        GameObject abilityInstance = Instantiate(active.UpgradePrefab);

        ActiveAbilityBase upgradeBase = abilityInstance.GetComponent<ActiveAbilityBase>();

        upgradeBase.AddAbilityToPlayer(playerControls, active.Rarity, abilityInstance, key);
    }


    private void CloseUI()
    {
        healthBar.SetActive(true);
        inputPrompt.SetActive(false);
        //re-enable time, re-enable controls
        playerControls.enabled = true;
        Time.timeScale = 1.0f;
    }

}
