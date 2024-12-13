using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Category Panels")]
    [SerializeField] private Transform keyPanel;
    [SerializeField] private Transform symbolPanel;
    [SerializeField] private Transform toolsPanel;

    [Header("Prefabs")]
    [SerializeField] private GameObject slotPrefab;

    [Header("Item Inspection UI")]
    [SerializeField] private ItemInspectionUI inspectionUI;

    [Header("Navigation")]
    [SerializeField] private Button closeInventoryButton;

    private InventoryManager _inventoryManager;

    private void Start()
    {
        _inventoryManager = FindFirstObjectByType<InventoryManager>();
        if (_inventoryManager != null)
        {
            _inventoryManager.OnItemAdded += OnItemAdded;
        }

        if (closeInventoryButton != null)
        {
            closeInventoryButton.onClick.AddListener(OnCloseInventoryClicked);
        }

        RefreshUI();
    }

    private void OnDestroy()
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.OnItemAdded -= OnItemAdded;
        }
    }

    private void OnItemAdded(ItemConfig item)
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (_inventoryManager == null) return;

        ClearPanel(keyPanel);
        ClearPanel(symbolPanel);
        ClearPanel(toolsPanel);

        // Retrieve the inventory items directly
        List<InventoryItem> items = _inventoryManager.GetInventoryItems();
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
    
    private void OnCloseInventoryClicked()
    {
        // Hide the inventory UI
        if (inspectionUI != null)
        {
            inspectionUI.HideItem();
        }

        if (_inventoryManager != null)
        {
            _inventoryManager.ToggleInventoryDisplay(false);
        }
    }
    
    private void ToggleCursor(bool enableCursor)
    {
        Cursor.lockState = enableCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enableCursor;
    }

}
