using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public void FadeOnDeath()
    {
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();

        StartCoroutine(Fade(canvasGroup));
    }

    // Adapted from: https://youtu.be/MkoIZTFUego
    IEnumerator Fade(CanvasGroup canvasGroup)
    {
        while (canvasGroup.alpha < 255)
        {
            canvasGroup.alpha += Time.deltaTime / 3;
            yield return null;
        }
        yield return null;
    }
}
