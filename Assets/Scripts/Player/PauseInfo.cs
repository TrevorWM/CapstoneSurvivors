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

        speedText.GetComponent<TextMeshProUGUI>().text = "Move Speed: +" + Mathf.Round(playerStats.MoveSpeedModifier * 100f - 100) + "%";

        fireText.GetComponent<TextMeshProUGUI>().text = "Fire Affinity: Fire abilities deal +" + Mathf.Round(playerStats.FireAffinity * 100f - 100f) + "% damage";

        waterText.GetComponent<TextMeshProUGUI>().text = "Water Affinity: Water abilities deal +" + Mathf.Round(playerStats.WaterAffinity * 100f - 100f) + "% damage";

        natureText.GetComponent<TextMeshProUGUI>().text = "Nature Affinity: Nature abilities deal +" + Mathf.Round(playerStats.NatureAffinity * 100f - 100f) + "% damage";

        critDamText.GetComponent<TextMeshProUGUI>().text = "Critical Damage: +" + Mathf.Round(playerStats.CriticalDamageMultiplier * 100f - 100) + "% damage";

        critRateText.GetComponent<TextMeshProUGUI>().text = "Critical Damage Chance: " + playerStats.CriticalChance + "%";
    }

    private void DisplayReducedStats()
    {
        damageText.GetComponent<TextMeshProUGUI>().text = playerStats.BaseDamage + "";

        rechargeText.GetComponent<TextMeshProUGUI>().text = "+" + playerStats.CooldownReduction * 100f + "%";

        defenseText.GetComponent<TextMeshProUGUI>().text = playerStats.Defense + "";

        speedText.GetComponent<TextMeshProUGUI>().text = "+" + Mathf.Round(playerStats.MoveSpeedModifier * 100f - 100) + "%";

        fireText.GetComponent<TextMeshProUGUI>().text = "+" + Mathf.Round(playerStats.FireAffinity * 100f - 100f) + "%";

        waterText.GetComponent<TextMeshProUGUI>().text = "+" + Mathf.Round(playerStats.WaterAffinity * 100f - 100f) + "%";

        natureText.GetComponent<TextMeshProUGUI>().text = "+" + Mathf.Round(playerStats.NatureAffinity * 100f - 100f) + "%";

        critDamText.GetComponent<TextMeshProUGUI>().text = "+" + Mathf.Round(playerStats.CriticalDamageMultiplier * 100f - 100) + "%";

        critRateText.GetComponent<TextMeshProUGUI>().text = playerStats.CriticalChance + "%";
    }

}
