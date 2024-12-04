using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [Header("Key Data")]
    public ItemData itemData; // Reference to the ScriptableObject holding item data
    public string[] unlockTargetIDs; // List of lock IDs this key can unlock

    [Header("Interactable State")]
    public bool canBeInteractedWith = true; // Local condition for interactability

    // Determine if the key can currently be interacted with
    public bool IsInteractable => canBeInteractedWith;

    // Provide a dynamic interaction hint based on the item's state
    public string InteractionHint => IsInteractable 
        ? $"Pick up {itemData.itemName}" 
        : "You can't pick this up right now.";

    /// <summary>
    /// Handles player interaction with the key (e.g., adding it to inventory).
    /// </summary>
    public void Interact(GameObject interactor)
    {
        if (!IsInteractable)
        {
            Debug.Log("This key cannot be interacted with right now.");
            return;
        }

        // TODO: Verify InventoryManager implementation.
        
        /*InventoryManager inventory = interactor.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(itemData);
            Debug.Log($"{itemData.itemName} added to inventory.");
            Destroy(gameObject); // Remove the key from the world
        }
        else
        {
            Debug.Log("No inventory system found on interactor.");
        }*/
    }
}