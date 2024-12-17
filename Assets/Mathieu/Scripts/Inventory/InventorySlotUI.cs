using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text quantityText;

    private ItemConfig currentItem;
    private int currentQuantity;
    private InventoryUI parentUI;

    public void Initialize(InventoryUI parent)
    {
        parentUI = parent;
    }

    public void SetItem(ItemConfig item, int quantity)
    {
        currentItem = item;
        currentQuantity = quantity;

        if (item != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
            quantityText.gameObject.SetActive(quantity > 1);
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        currentQuantity = 0;
        iconImage.sprite = null;
        iconImage.enabled = false;
        quantityText.text = "";
        quantityText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null && parentUI != null)
        {
            parentUI.OnSlotClicked(currentItem);
        }
    }
}