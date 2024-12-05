using UnityEngine;

public class Lock : MonoBehaviour, IInteractable
{
    [Header("Lock Properties")]
    public string lockID; // Unique ID for this lock
    public bool isLocked = true;

    [Header("Unlock Requirements")]
    public string requiredKeyName; // Optional: Specific key name required

    public bool IsInteractable => isLocked; // Only interactable if locked

    public string InteractionHint => isLocked 
        ? "Press 'E' to unlock" 
        : "This lock is already unlocked.";

    /// <summary>
    /// Handles player interaction with the lock (e.g., attempting to unlock it).
    /// </summary>
    public void Interact(GameObject interactor)
    {
        if (!IsInteractable)
        {
            Debug.Log("This lock is already unlocked.");
            return;
        }

        // TODO: Verify InventoryManager implementation.
        
        /*InventoryManager inventory = interactor.GetComponent<InventoryManager>();
        if (inventory == null)
        {
            Debug.Log("No inventory system found on interactor.");
            return;
        }

        // Find a key that can unlock this lock
        ItemData keyToUse = inventory.GetMatchingKey(lockID);

        if (keyToUse != null)
        {
            // Unlock the lock
            isLocked = false;
            Debug.Log($"Lock {lockID} unlocked with {keyToUse.itemName}.");

            // Check if the key should be consumed
            if (keyToUse.consumeOnUse)
            {
                inventory.RemoveItem(keyToUse);
                Debug.Log($"{keyToUse.itemName} has been consumed.");
            }

            // Trigger door opening or other effects
            OpenDoor();
        }
        else
        {
            Debug.Log("You don't have the required key to unlock this lock.");
        }*/
    }

    /// <summary>
    /// Placeholder method to handle door opening.
    /// </summary>
    private void OpenDoor()
    {
        // Implement door opening logic here (e.g., animation, state change)
        Debug.Log("Door is now open!");
    }
}
