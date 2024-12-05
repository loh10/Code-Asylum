using UnityEngine;

public class Tool : MonoBehaviour, IInteractable
{
    [Header("Tool Data")]
    public ItemData itemData; // Reference to the ScriptableObject holding item data
    public int currentDurability; // Current durability of the tool

    [Header("Interactable State")]
    public bool canBeInteractedWith = true; // Local condition for interactability

    // Determine if the tool can currently be interacted with
    public bool IsInteractable => canBeInteractedWith && currentDurability > 0;

    // Provide a dynamic interaction hint based on the tool's state
    public string InteractionHint => IsInteractable 
        ? $"Pick up {itemData.itemName}" 
        : $"{itemData.itemName} is broken and cannot be picked up.";

    /// <summary>
    /// Handles player interaction with the tool (e.g., adding it to inventory).
    /// </summary>
    public void Interact(GameObject interactor)
    {
        if (!IsInteractable)
        {
            Debug.Log($"{itemData.itemName} cannot be interacted with right now.");
            return;
        }
        
        // TODO: Verify InventoryManager implementation.
        
        /*InventoryManager inventory = interactor.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(itemData);
            Debug.Log($"{itemData.itemName} added to inventory.");
            Destroy(gameObject); // Remove the tool from the world
        }
        else
        {
            Debug.Log("No inventory system found on interactor.");
        }*/
    }

    /// <summary>
    /// Reduces the tool's durability when used.
    /// </summary>
    public void UseTool()
    {
        if (currentDurability > 0)
        {
            currentDurability--;
            Debug.Log($"{itemData.itemName} used. Remaining durability: {currentDurability}");

            if (currentDurability <= 0)
            {
                Debug.Log($"{itemData.itemName} has broken.");
                // Optionally handle tool breaking (e.g., remove from inventory)
            }
        }
        else
        {
            Debug.Log($"{itemData.itemName} is already broken.");
        }
    }
}
