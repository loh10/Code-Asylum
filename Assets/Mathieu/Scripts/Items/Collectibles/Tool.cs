using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Represents a tool item lying in the world that can be collected.
/// </summary>
public class Tool : MonoBehaviour, ICollectable
{
    [FormerlySerializedAs("toolItemData")] [Header("Tool ScriptableObjects")]
    public ItemConfig toolItemConfig;
    public bool canBeCollected = true;

    public bool CanCollect => canBeCollected;
    public string CollectHint => $"Pick up {toolItemConfig.itemName}";

    /// <summary>
    /// Called when the player collects this tool.
    /// Adds the item to the player's inventory and destroys the world object.
    /// </summary>
    public void OnCollect(GameObject collector)
    {
        InventoryManager inventory = collector.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(toolItemConfig);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No inventory system found on collector.");
        }
    }
}