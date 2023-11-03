using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseInfo : MonoBehaviour
{
    private CharacterStats playerStats;

    [SerializeField]
    private GameObject damageText;
    [SerializeField]
    private GameObject rechargeText;
    [SerializeField]
    private GameObject defenseText;
    [SerializeField]
    private GameObject speedText;
    [SerializeField]
    private GameObject fireText;
    [SerializeField]
    private GameObject waterText;
    [SerializeField]
    private GameObject natureText;
    [SerializeField]
    private GameObject critDamText;
    [SerializeField]
    private GameObject critRateText;

    public void ShowStats(CharacterStats stats)
    {
        playerStats = stats;
        playerStats.PrintStatSheet();
        DisplayStats();
    }

    public void ShowReducedStats(CharacterStats stats)
    {
        playerStats = stats;
        playerStats.PrintStatSheet();
        DisplayReducedStats();
    }

    private void DisplayStats()
    {
        damageText.GetComponent<TextMeshProUGUI>().text = "Base Damage: " + playerStats.BaseDamage + " damage";

        rechargeText.GetComponent<TextMeshProUGUI>().text = "Cooldown Reduction: " + playerStats.CooldownReduction * 100f +"% faster recharge and attack rate";

        defenseText.GetComponent<TextMeshProUGUI>().text = "Defense: Reduce all damage taken by " + playerStats.Defense + " damage";

        speedText.GetComponent<TextMeshProUGUI>().text = "Move Speed: " + playerStats.MoveSpeedModifier * 100f + "%";

        fireText.GetComponent<TextMeshProUGUI>().text = "Fire Affinity: Fire abilities deal " + playerStats.FireAffinity * 100f + "% of base damage";

        waterText.GetComponent<TextMeshProUGUI>().text = "Water Affinity: Water abilities deal " + playerStats.WaterAffinity * 100f + "% of base damage";

        natureText.GetComponent<TextMeshProUGUI>().text = "Nature Affinity: Nature abilities deal " + playerStats.NatureAffinity * 100f + "% of base damage";

        critDamText.GetComponent<TextMeshProUGUI>().text = "Critical Damage Multiplier: " + playerStats.CriticalDamageMultiplier * 100f + "% base damage";

        critRateText.GetComponent<TextMeshProUGUI>().text = "Critical Damage Chance: " + playerStats.CriticalChance + "%";
    }

    private void DisplayReducedStats()
    {
        damageText.GetComponent<TextMeshProUGUI>().text = "" + playerStats.BaseDamage;

        rechargeText.GetComponent<TextMeshProUGUI>().text = playerStats.CooldownReduction * 100f + "%";

        defenseText.GetComponent<TextMeshProUGUI>().text = playerStats.Defense +"";

        speedText.GetComponent<TextMeshProUGUI>().text = playerStats.MoveSpeedModifier * 100f + "%";

        fireText.GetComponent<TextMeshProUGUI>().text = playerStats.FireAffinity * 100f + "%";

        waterText.GetComponent<TextMeshProUGUI>().text = playerStats.WaterAffinity * 100f + "%";

        natureText.GetComponent<TextMeshProUGUI>().text = playerStats.NatureAffinity * 100f + "%";

        critDamText.GetComponent<TextMeshProUGUI>().text = playerStats.CriticalDamageMultiplier * 100f + "%";

        critRateText.GetComponent<TextMeshProUGUI>().text = playerStats.CriticalChance + "%";
    }

}
