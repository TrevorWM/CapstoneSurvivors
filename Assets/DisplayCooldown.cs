using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCooldown : MonoBehaviour
{
    [SerializeField]
    private Slider cooldownBar;

    private void Start()
    {
        cooldownBar = GetComponent<Slider>();
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
            yield return null;
        }      
    }
}
