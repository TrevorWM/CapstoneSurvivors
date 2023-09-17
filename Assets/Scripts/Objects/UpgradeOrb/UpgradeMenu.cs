using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Sprite purpleCommon;
    [SerializeField]
    private Sprite purpleUncommon;
    [SerializeField]
    private Sprite purpleRare;
    [SerializeField]
    private Sprite purpleLegendary;

    [SerializeField]
    private GameObject[] portals = new GameObject[3];
    [SerializeField]
    private GameObject[] texts = new GameObject[3];

    private void Start()
    {
        
    }

    public void SetUpgrades(Upgrade[] upgrades)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            switch (upgrades[i].Rarity)
            {
                case UpgradeRarity.Common:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite = purpleCommon;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayName();
                    break;
                case UpgradeRarity.Uncommon:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite = purpleUncommon;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayName();
                    break;
                case UpgradeRarity.Rare:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite = purpleRare;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayName();
                    break;
                case UpgradeRarity.Legendary:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite = purpleLegendary;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayName();
                    break;

            }
        }
    }

    public Upgrade GetUpgrade(Upgrade[] upgrades)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            Debug.Log(upgrades[i].ToString());
        } 
        SetUpgrades(upgrades);

        return null;
    }
    public void ShowUpgradeMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void HideUpgradeMenu()
    {
        this.gameObject.SetActive(false);
    }

}
