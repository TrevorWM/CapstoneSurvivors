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
    private Sprite redCommon;
    [SerializeField]
    private Sprite redUncommon;
    [SerializeField]
    private Sprite redRare;
    [SerializeField]
    private Sprite redLegendary;

    [SerializeField]
    private GameObject[] portals = new GameObject[3];
    [SerializeField]
    private GameObject[] texts = new GameObject[3];

    private void Start()
    {
        
    }

    public void SetUpgrades(IUpgrade[] upgrades)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            portals[i].GetComponent<UIHoverEffect>().CurrentUpgrade = upgrades[i];

            switch (upgrades[i].Rarity)
            {
                case UpgradeRarity.Common:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite = 
                        (upgrades[i].Category == UpgradeCategory.Active) ? redCommon : purpleCommon;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayText();
                    break;
                case UpgradeRarity.Uncommon:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite =
                        (upgrades[i].Category == UpgradeCategory.Active) ? redUncommon : purpleUncommon;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayText();
                    break;
                case UpgradeRarity.Rare:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite =
                        (upgrades[i].Category == UpgradeCategory.Active) ? redRare : purpleRare;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayText();
                    break;
                case UpgradeRarity.Legendary:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite =
                        (upgrades[i].Category == UpgradeCategory.Active) ? redLegendary : purpleLegendary;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayText();
                    break;
            }
        }
    }

    public IUpgrade GetUpgrade(IUpgrade[] upgrades)
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
