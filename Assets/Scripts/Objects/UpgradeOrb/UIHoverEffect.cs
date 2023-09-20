using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private RectTransform portal;
    private UnityEngine.UI.Image image;
    private Vector2 originalSize;
    private Vector2 hoverSize;
    private UpgradeOrb orb;
    private IUpgrade currentUpgrade;

    public IUpgrade CurrentUpgrade { get => currentUpgrade; set => currentUpgrade = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        //send to orb UI handler
        //get from name of object (Portal1)
        //get number from name-1 for index of portal
        //on click event 
        orb.SetSelectedUpgrade(currentUpgrade);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //image.sprite = hoverSize;
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hoverSize.x);
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hoverSize.y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        orb = GetComponentInParent<UpgradeOrb>();
        portal = GetComponent<RectTransform>();
        image = GetComponent<UnityEngine.UI.Image>();
        originalSize = new(image.sprite.rect.width, image.sprite.rect.height);
        hoverSize = new(image.sprite.rect.width * 1.2f, image.sprite.rect.height * 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
