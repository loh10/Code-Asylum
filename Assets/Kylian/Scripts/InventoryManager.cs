using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the player's inventory, allowing adding, removing, and querying items.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public static InventoryManager Instance;

    [SerializeField] private GameObject _inventoryUI;
    private bool _isDisplay;
    
    public delegate void ItemAddedEventHandler(ItemConfig item);
    public event ItemAddedEventHandler OnItemAdded;

    private void Awake()
    {
        Instance = this;
    }
    public void OpenInventory(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled) return;
        
        ToggleInventoryDisplay(!_isDisplay);
    }
    
    public void ToggleInventoryDisplay(bool display)
    {
        if (Cursor.visible == display) return;

        MiniGameManager.currentMiniGame = _inventoryUI;
        _isDisplay = display;

        if (_inventoryUI != null)
        {
            _inventoryUI.SetActive(display);
        }

        // Manage cursor visibility and player input    
        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = display;
        PlayerController.freezeInput = display;
    }

    public void AddItem(ItemConfig item)
    {
        InventoryItem existingItem = inventoryItems.Find(i => i.ItemConfig == item);
        if (existingItem != null)
        {
            if (item.isStackable)
            {
                existingItem.quantity++;
            }
            else
            {
                inventoryItems.Add(new InventoryItem(item, 1));
            }
        }
        else
        {
            inventoryItems.Add(new InventoryItem(item, 1));
        }

        Debug.Log($"{item.itemName} added to inventory.");
        OnItemAdded?.Invoke(item);
    }

    public void RemoveItem(ItemConfig item)
    {
        InventoryItem existingItem = inventoryItems.Find(i => i.ItemConfig == item);
        if (existingItem != null)
        {
            existingItem.quantity--;
            if (existingItem.quantity <= 0)
            {
                inventoryItems.Remove(existingItem);
            }
            Debug.Log($"{item.itemName} removed from inventory.");
        }
        else
        {
            Debug.LogWarning($"{item.itemName} not found in inventory.");
        }
    }

    public bool HasItem(ItemConfig item)
    {
        return inventoryItems.Exists(i => i.ItemConfig == item);
    }

    public int GetItemQuantity(ItemConfig item)
    {
        InventoryItem existingItem = inventoryItems.Find(i => i.ItemConfig == item);
        return existingItem != null ? existingItem.quantity : 0;
    }

    /// <summary>
    /// Returns the raw list of inventory items (ItemConfig + quantity)
    /// </summary>
    public List<InventoryItem> GetInventoryItems()
    {
        return inventoryItems;
    }
}

public class InventoryItem
{
    public ItemConfig ItemConfig;
    public int quantity;

    public InventoryItem(ItemConfig itemConfig, int quantity)
    {
        this.ItemConfig = itemConfig;
        this.quantity = quantity;
    }
}
