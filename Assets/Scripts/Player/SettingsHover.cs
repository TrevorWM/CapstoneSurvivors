using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private RectTransform textBox;
    private UnityEngine.UI.Image image;
    private Vector2 originalSize;
    private Vector2 hoverSize;



    public void OnPointerClick(PointerEventData eventData)
    {
        //change size back
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
        // open settings screen
        Debug.Log("Settings clicked...");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hoverSize.x);
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hoverSize.y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        textBox = GetComponent<RectTransform>();
        image = GetComponent<UnityEngine.UI.Image>();
        originalSize = new(image.sprite.rect.width, image.sprite.rect.height);
        hoverSize = new(image.sprite.rect.width * 1.2f, image.sprite.rect.height * 1.2f);
    }
}
