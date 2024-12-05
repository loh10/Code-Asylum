using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // List of items currently in the inventory
    private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    [SerializeField]
    private GameObject _inventoryUI;
    private bool _isDisplay;

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AddItem(ItemData item)
    {
        InventoryItem existingItem = inventoryItems.Find(i => i.itemData == item);

        if (existingItem != null)
        {
            if (item.isStackable)
            {
                existingItem.quantity++;
            }
            else
            {
                // Allow multiple instances of non-stackable items (Optional)
                inventoryItems.Add(new InventoryItem(item, 1));
                Debug.Log($"{item.itemName} added to inventory.");
            }
        }
        else
        {
            inventoryItems.Add(new InventoryItem(item, 1));
            Debug.Log($"{item.itemName} added to inventory.");
        }
    }

    /// <summary>
    /// Removes an item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    public void RemoveItem(ItemData item)
    {
        InventoryItem existingItem = inventoryItems.Find(i => i.itemData == item);

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

    /// <summary>
    /// Checks if the inventory contains a specific item.
    /// </summary>
    /// <param name="item">The item to check for.</param>
    /// <returns>True if the item is in the inventory; otherwise, false.</returns>
    public bool HasItem(ItemData item)
    {
        return inventoryItems.Exists(i => i.itemData == item);
    }

    /// <summary>
    /// Gets a list of all items in the inventory.
    /// </summary>
    /// <returns>A list of ItemData objects in the inventory.</returns>
    public List<ItemData> GetAllItems()
    {
        List<ItemData> items = new List<ItemData>();
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            items.Add(inventoryItem.itemData);
        }
        return items;
    }
    
    /// <summary>
    /// Gets the quantity of a specific item in the inventory.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>The quantity of the item.</returns>
    public int GetItemQuantity(ItemData item)
    {
        InventoryItem existingItem = inventoryItems.Find(i => i.itemData == item);
        return existingItem != null ? existingItem.quantity : 0;
    }

    /// <summary>
    /// Finds a key in the inventory that can unlock the specified lock ID.
    /// </summary>
    /// <param name="lockID">The ID of the lock to unlock.</param>
    /// <returns>The matching KeyItemData if found; otherwise, null.</returns>
    public KeyItemData GetMatchingKey(string lockID)
    {
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            if (inventoryItem.itemData.itemType == ItemType.Key)
            {
                KeyItemData keyItemData = inventoryItem.itemData as KeyItemData;
                if (keyItemData != null)
                {
                    if (System.Array.Exists(keyItemData.unlockTargetIDs, id => id == lockID))
                    {
                        return keyItemData;
                    }
                }
            }
        }
        return null; // No matching key found
    }

    /// <summary>
    /// Toggles the display of the inventory UI.
    /// </summary>
    /// <param name="display">True to display the UI; false to hide it.</param>
    public void ToggleInventoryDisplay(bool display)
    {
        _isDisplay = display;

        if (_inventoryUI != null)
        {
            _inventoryUI.SetActive(display);
        }
    }
}

/// <summary>
/// Represents an item in the inventory along with its quantity.
/// </summary>
public class InventoryItem
{
    public ItemData itemData;
    public int quantity;

    public InventoryItem(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
    }
}
