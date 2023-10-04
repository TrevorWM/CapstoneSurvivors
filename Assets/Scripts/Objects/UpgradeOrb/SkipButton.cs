using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform textBox;
    private UnityEngine.UI.Image image;
    private Vector2 originalSize;
    private Vector2 hoverSize;
    private UpgradeOrb orb;

    [SerializeField]
    private TooltipUI tooltip;


    public void OnPointerClick(PointerEventData eventData)
    {
        tooltip.HideTooltip();
        //change size back
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
        //send upgrade to orb
        orb.SkipUpgrade();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hoverSize.x);
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hoverSize.y);
        tooltip.ShowTooltip("This will destroy the ORB");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        textBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
        tooltip.HideTooltip();
    }

    // Start is called before the first frame update
    void Start()
    {
        orb = GetComponentInParent<UpgradeOrb>();
        textBox = GetComponent<RectTransform>();
        image = GetComponent<UnityEngine.UI.Image>();
        originalSize = new(image.sprite.rect.width, image.sprite.rect.height);
        hoverSize = new(image.sprite.rect.width * 1.2f, image.sprite.rect.height * 1.2f);
    }
}
