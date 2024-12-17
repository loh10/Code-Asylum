using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Represents a key item lying in the world that can be collected.
/// </summary>
public class Key : MonoBehaviour, ICollectable
{
    [Header("Key Data")]
    public ItemConfig keyItemConfig;
    public bool canBeCollected = true;

    public bool CanCollect => canBeCollected;
    public string CollectHint => $"Pick up {keyItemConfig.itemName}";

    /// <summary>
    /// Called when the player collects this key.
    /// Adds the item to the player's inventory and destroys the world object.
    /// </summary>
    public void OnCollect(GameObject collector)
    {
        InventoryManager inventory = collector.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(keyItemConfig);
            
            // Show a message to the player
            string message = DialogueManager.GetDialogue("Key", "GotKey");
            DialogueMessageBoxUI.Instance.ShowMessage(message, 3f);
            
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No inventory system found on collector.");
        }
    }
}