using UnityEngine;

/// <summary>
/// A collectible symbol item that can be assigned an ItemConfig at runtime.
/// Once player collects it, it's added to the inventory.
/// </summary>
public class Symbol : MonoBehaviour, ICollectable
{
    [HideInInspector] public bool canBeCollected = false;
    private ItemConfig _itemConfig; // Set dynamically by SymbolContainer when unlocked.
    public bool CanCollect => canBeCollected && _itemConfig != null;

    public string CollectHint => _itemConfig != null 
        ? $"Pick up {_itemConfig.itemName}" 
        : "Nothing to collect.";

    /// <summary>
    /// Called by SymbolContainer when unlocked.
    /// </summary>
    public void SetItemConfig(ItemConfig config)
    {
        _itemConfig = config;
        canBeCollected = true; 
    }

    public void OnCollect(GameObject collector)
    {
        if (!CanCollect)
        {
            Debug.Log("Item not ready for collection.");
            return;
        }

        InventoryManager inventory = collector.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(_itemConfig);
            Debug.Log($"{_itemConfig.itemName} collected.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No inventory system found on collector.");
        }
    }
}