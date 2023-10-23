using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFlash : MonoBehaviour
{
    [SerializeField]
    private Vector2 startPosition, endPosition;

    [SerializeField]
    private float duration, delay, delayRandomAmount;

    private float currentTime;
    private float randomizedDelay;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = startPosition;
        randomizedDelay = delay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, currentTime/duration);
        currentTime += Time.deltaTime;
        
        if (currentTime > (duration + randomizedDelay))
        {
            randomizedDelay = (delay + Random.Range(-delayRandomAmount, delayRandomAmount)); 
            currentTime = 0;
        }
    }
}
