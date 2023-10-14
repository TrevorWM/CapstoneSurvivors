using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarSlider;

    [SerializeField]
    private TextMeshProUGUI healthText;
 

    public void UpdateHealthBarValue(float currentHP, float maxHP)
    {
        healthBarSlider.value = currentHP/maxHP;
        healthText.text = (currentHP.ToString("F0") + "/" + maxHP.ToString());

        if (healthBarSlider.value <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    
    
}
