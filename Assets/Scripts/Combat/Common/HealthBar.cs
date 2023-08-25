using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private HealthComponent healthComponent;

    [SerializeField]
    private Slider healthBarSlider;
    

    public void UpdateHealthBarValue()
    {
        healthBarSlider.value = healthComponent.CurrentHP / healthComponent.MaximumHP;
    }

    private void Awake()
    {
        healthBarSlider.value = 1;
    }
}
