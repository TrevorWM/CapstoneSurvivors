using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TooltipUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvas;

    private RectTransform backgroundRect;
    private TextMeshProUGUI textMesh;
    private RectTransform textRect;

    private void Awake()
    {
        backgroundRect = transform.Find("Background").GetComponent<RectTransform>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textRect = transform.GetComponent<RectTransform>();

        HideTooltip();
    }

    private void SetText(string text)
    {
        textMesh.SetText(text);
        textMesh.ForceMeshUpdate();

        Vector2 textSize = textMesh.GetRenderedValues(false);
        Vector2 padding = new(4f, 4f);

        backgroundRect.sizeDelta = textSize + padding;

    }

    private void Update()
    {
        Vector3 mousePadding = new(-1f, -1f, 0f);
        Vector2 position = Input.mousePosition + mousePadding;

        if (position.x + backgroundRect.rect.width > canvas.rect.width)
        {
            position.x = canvas.rect.width - backgroundRect.rect.width;
        }
        if (position.y + backgroundRect.rect.height > canvas.rect.height)
        {
            position.y = canvas.rect.height - backgroundRect.rect.height;
        }

        textRect.anchoredPosition = position;

    }

    public void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        Vector3 mousePadding = new(-1f, -1f, 0f);
        textRect.anchoredPosition = Input.mousePosition + mousePadding;
        SetText(text);
    }
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
