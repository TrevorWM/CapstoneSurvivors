using UnityEngine;
using UnityEngine.EventSystems;


public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private RectTransform portal;
    private UnityEngine.UI.Image image;
    private Vector2 originalSize;
    private Vector2 hoverSize;
    private UpgradeOrb orb;
    private IUpgrade currentUpgrade;

    [SerializeField]
    private TooltipUI tooltip;

    public IUpgrade CurrentUpgrade { get => currentUpgrade; set => currentUpgrade = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        tooltip.HideTooltip();
        //change size back
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
        //send upgrade to orb
        orb.SetSelectedUpgrade(currentUpgrade);
        //tell orb that it can finalize the upgrade process
        orb.FinalizeChoice();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hoverSize.x);
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hoverSize.y);
        tooltip.ShowTooltip(currentUpgrade.GetDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        portal.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
        tooltip.HideTooltip();
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

}
