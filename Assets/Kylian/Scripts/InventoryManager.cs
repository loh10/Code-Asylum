using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's inventory, allowing adding, removing, and querying items.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    /// <summary>
    /// The list of items currently in the player's inventory.
    /// </summary>
    private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    [SerializeField]
    private GameObject _inventoryUI;
    private bool _isDisplay;
    
    /// <summary>
    /// Event triggered when an item is added to the inventory.
    /// </summary>
    public delegate void ItemAddedEventHandler(ItemConfig item);
    public event ItemAddedEventHandler OnItemAdded;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DebugDisplayInventory();
        }
    }

    /// <summary>
    /// Displays the inventory content in the console for debugging.
    /// </summary>
    public void DebugDisplayInventory()
    {
        Debug.Log("Inventory Contents:");
        if (inventoryItems.Count == 0)
        {
            Debug.Log("Inventory is empty.");
        }
        else
        {
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                Debug.Log($"Item: {inventoryItem.ItemConfig.itemName}, Quantity: {inventoryItem.quantity}");
            }
        }
    }

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
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
                // Non-stackable item, add another instance
                inventoryItems.Add(new InventoryItem(item, 1));
                Debug.Log($"{item.itemName} added to inventory.");
            }
        }
        else
        {
            inventoryItems.Add(new InventoryItem(item, 1));
            Debug.Log($"{item.itemName} added to inventory.");
        }
        
        OnItemAdded?.Invoke(item); // Fire event after adding the item
    }

    /// <summary>
    /// Removes an item from the inventory.
    /// </summary>
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

    /// <summary>
    /// Checks if the inventory contains a specific item.
    /// </summary>
    public bool HasItem(ItemConfig item)
    {
        return inventoryItems.Exists(i => i.ItemConfig == item);
    }

    /// <summary>
    /// Retrieves all items currently in the inventory.
    /// </summary>
    public List<ItemConfig> GetAllItems()
    {
        List<ItemConfig> items = new List<ItemConfig>();
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            items.Add(inventoryItem.ItemConfig);
        }
        return items;
    }

    /// <summary>
    /// Gets the quantity of a specific item in the inventory.
    /// </summary>
    public int GetItemQuantity(ItemConfig item)
    {
        InventoryItem existingItem = inventoryItems.Find(i => i.ItemConfig == item);
        return existingItem != null ? existingItem.quantity : 0;
    }

    /// <summary>
    /// Toggles the display of the inventory UI.
    /// </summary>
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
/// Represents an item stored in the inventory with a given quantity.
/// </summary>
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
