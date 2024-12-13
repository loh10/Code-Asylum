using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Category Panels")]
    [SerializeField] private Transform keyPanel;
    [SerializeField] private Transform symbolPanel;
    [SerializeField] private Transform documentsPanel;
    [SerializeField] private Transform toolsPanel;

    [Header("Prefabs")]
    [SerializeField] private GameObject slotPrefab;

    [Header("Item Inspection UI")]
    [SerializeField] private ItemInspectionUI inspectionUI;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.OnItemAdded += OnItemAdded;
        }
        RefreshUI();
    }

    private void OnDestroy()
    {
        if (inventoryManager != null)
        {
            inventoryManager.OnItemAdded -= OnItemAdded;
        }
    }

    private void OnItemAdded(ItemConfig item)
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (inventoryManager == null) return;

        ClearPanel(keyPanel);
        ClearPanel(symbolPanel);
        ClearPanel(documentsPanel);
        ClearPanel(toolsPanel);

        List<InventoryItem> items = inventoryManager.GetInventoryItems();
        foreach (var invItem in items)
        {
            Transform parentPanel = GetPanelForItem(invItem.ItemConfig);
            if (parentPanel != null)
            {
                GameObject slotGO = Instantiate(slotPrefab, parentPanel);
                InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
                slotUI.Initialize(this);
                slotUI.SetItem(invItem.ItemConfig, invItem.quantity);
            }
        }
    }

    private Transform GetPanelForItem(ItemConfig item)
    {
        switch (item.itemType)
        {
            case ItemType.Key:
                return keyPanel;
            case ItemType.Tool:
                return toolsPanel;
            case ItemType.Symbol:
                return symbolPanel;
            case ItemType.Generic:
                // Assume documents are generic
                return documentsPanel;
            default:
                return null;
        }
    }

    private void ClearPanel(Transform panel)
    {
        for (int i = panel.childCount - 1; i >= 0; i--)
        {
            Destroy(panel.GetChild(i).gameObject);
        }
    }

    public void OnSlotClicked(ItemConfig item)
    {
        // Show the item in the inspection UI
        if (inspectionUI != null)
        {
            inspectionUI.ShowItem(item);
        }
    }
}
