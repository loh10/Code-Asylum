using UnityEngine;
using System;

public class Lock : MonoBehaviour, IInteractable
{
    [Header("Lock Properties")]
    public string lockID; // Unique ID for this lock
    public bool isLocked = true;

    public bool IsInteractable => isLocked; // Only interactable if locked

    public string InteractionHint => isLocked
        ? "Press 'E' to unlock"
        : "This lock is already unlocked.";

    // Event triggered when the lock is unlocked
    public event Action OnUnlock;
    
    public void Interact(GameObject interactor)
    {
        if (!IsInteractable)
        {
            Debug.Log("This lock is already unlocked.");
            return;
        }

        InventoryManager inventory = interactor.GetComponent<InventoryManager>();
        if (inventory == null)
        {
            Debug.Log("No inventory system found on interactor.");
            return;
        }

        // Find a key that can unlock this lock
        KeyItemData keyToUse = inventory.GetMatchingKey(lockID);

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

            // Trigger the OnUnlock event
            OnUnlock?.Invoke();
        }
        else
        {
            Debug.Log("You don't have the required key to unlock this lock.");
        }
    }
}