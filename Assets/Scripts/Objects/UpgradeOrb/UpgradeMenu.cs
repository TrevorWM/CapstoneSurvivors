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
    [SerializeField]
    private GameObject[] statImages = new GameObject[3];


    /// <summary>
    /// Sets the upgrades to the portals in the UI
    /// </summary>
    /// <param name="upgrades"></param>
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
                    SetUpgradeSprite(statImages[i], upgrades[i]);
                    break;
                case UpgradeRarity.Uncommon:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite =
                        (upgrades[i].Category == UpgradeCategory.Active) ? redUncommon : purpleUncommon;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayText();
                    SetUpgradeSprite(statImages[i], upgrades[i]);
                    break;
                case UpgradeRarity.Rare:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite =
                        (upgrades[i].Category == UpgradeCategory.Active) ? redRare : purpleRare;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayText();
                    SetUpgradeSprite(statImages[i], upgrades[i]);
                    break;
                case UpgradeRarity.Legendary:
                    portals[i].GetComponent<UnityEngine.UI.Image>().sprite =
                        (upgrades[i].Category == UpgradeCategory.Active) ? redLegendary : purpleLegendary;
                    texts[i].GetComponent<TextMeshProUGUI>().text = upgrades[i].DisplayText();
                    SetUpgradeSprite(statImages[i], upgrades[i]);
                    break;
            }
        }
    }

    private void SetUpgradeSprite(GameObject statImage, IUpgrade upgrade)
    {
        if (upgrade.Category == UpgradeCategory.Active)
        {
            ActiveUpgrade active = upgrade as ActiveUpgrade;
            statImage.GetComponent<UnityEngine.UI.Image>().sprite = active.UpgradeType.ActiveAbilitySO.AbilityIcon;
            // make it a lil bigger cause active abilities kinda smol
            statImage.transform.localScale = Vector3.one;
        }
        else
        {
            PassiveUpgrade passive = upgrade as PassiveUpgrade;
            statImage.GetComponent<UnityEngine.UI.Image>().sprite = passive.UpgradeType.PassiveUpgradeSO.Sprite;
            statImage.transform.localScale = new Vector3(.75f, .75f, 1);
        }
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
