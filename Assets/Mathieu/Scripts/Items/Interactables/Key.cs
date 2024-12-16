using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [Header("Key Data")]
    public KeyItemData keyItemData; // Reference to the ScriptableObject holding key item data

    [Header("Interactable State")]
    public bool canBeInteractedWith = true; // Local condition for interactability

    // Determine if the key can currently be interacted with
    public bool IsInteractable => canBeInteractedWith;

    // Provide a dynamic interaction hint based on the item's state
    // TODO: Replace this with the Dialogue System (Optional).
    public string InteractionHint => IsInteractable
        ? $"Pick up {keyItemData.itemName}"
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
        else
        {
            AudioManager.Instance.PlaySound(AudioType.button, AudioSourceType.player);
        }

        InventoryManager inventory = interactor.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(keyItemData);
            Destroy(gameObject); // Remove the key from the world
        }
        else
        {
            Debug.Log("No inventory system found on interactor.");
        }
    }
}