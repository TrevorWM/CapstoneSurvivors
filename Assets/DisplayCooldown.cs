using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCooldown : MonoBehaviour
{
    [SerializeField]
    private Slider cooldownBar;

    private TextMeshProUGUI cooldownText;

    private void Start()
    {
        cooldownBar = GetComponent<Slider>();
        cooldownText = GetComponentInChildren<TextMeshProUGUI>();
        if (cooldownText) cooldownText.text = "";
    }

    public void StartShowCooldown(float duration)
    {
        StartCoroutine(ShowCooldown(duration));
    }

    private IEnumerator ShowCooldown(float duration)
    {
        float timeSinceAbilityStart = 0f;

        while (timeSinceAbilityStart < duration)
        {
            timeSinceAbilityStart += Time.deltaTime;
            if (cooldownBar) cooldownBar.value = Mathf.Lerp(1f, 0f, timeSinceAbilityStart / duration);
            cooldownText.text = (duration - timeSinceAbilityStart).ToString("F1");
            yield return null;
        }
        cooldownText.text = "";
    }
}
