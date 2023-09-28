using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private CharacterStats playerStats;

    [SerializeField]
    private Slider healthBarSlider;
    

    public void UpdateHealthBarValue()
    {
        healthBarSlider.value = playerStats.CurrentHealth / playerStats.MaxHealth;
        
        if (healthBarSlider.value <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        healthBarSlider.value = 1;
    }
}
