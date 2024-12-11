using UnityEngine;

/// <summary>
/// A collectible symbol item that can be assigned an ItemConfig at runtime.
/// Once player collects it, it's added to the inventory.
/// </summary>
public class SymbolCollectibleItem : MonoBehaviour, ICollectable
{
    [SerializeField] private ItemConfig itemConfig; // Initially null or a placeholder
    public bool canBeCollected = false;

    public bool CanCollect => canBeCollected && itemConfig != null;

    public string CollectHint => itemConfig != null 
        ? $"Pick up {itemConfig.itemName}" 
        : "Nothing to collect.";

    /// <summary>
    /// Called by PuzzleSymbolContainer when unlocked.
    /// </summary>
    public void SetItemConfig(ItemConfig config)
    {
        itemConfig = config;
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
            inventory.AddItem(itemConfig);
            Debug.Log($"{itemConfig.itemName} collected.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No inventory system found on collector.");
        }
    }
}