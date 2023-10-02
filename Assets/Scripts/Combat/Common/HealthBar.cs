using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private CharacterStats playerStats;

    [SerializeField]
    private Slider healthBarSlider;

    [SerializeField]
    private TextMeshProUGUI healthText;
 
    


    public void UpdateHealthBarValue()
    {
        healthBarSlider.value = playerStats.CurrentHealth/playerStats.MaxHealth;
        healthText.text = (playerStats.CurrentHealth.ToString() + "/" + playerStats.MaxHealth.ToString());

        if (healthBarSlider.value <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    
    
}
