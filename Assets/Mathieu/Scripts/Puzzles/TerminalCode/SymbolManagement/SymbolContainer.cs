using UnityEngine;

/// <summary>
/// Placed on the container in a puzzle room. Once unlocked, it assigns the correct symbol item
/// based on the puzzleID that rewards that symbol.
/// </summary>
public class SymbolContainer : MonoBehaviour
{
    [Header("Container Settings")]
    [Tooltip("Used to assign the correct symbol (X, Y, Z). See SymbolManager for puzzleID values.")]
    public int puzzleID;

    [Tooltip("The collectible item inside this container.")]
    public Symbol symbolItem;
    
    [Tooltip("If your lock is on a different GameObject, assign it here.")]
    [SerializeField] private Lock lockComponent;

    private void Awake()
    {
        if (lockComponent == null)
        {
            lockComponent = GetComponent<Lock>();
        }

        if (lockComponent != null)
        {
            lockComponent.OnUnlock += OnContainerUnlocked;
        }
        else
        {
            Debug.LogWarning($"No Lock component assigned or found on the GameObject '{gameObject.name}'.");
        }
    }

    private void OnDestroy()
    {
        if (lockComponent != null)
        {
            lockComponent.OnUnlock -= OnContainerUnlocked;
        }
    }

    private void OnContainerUnlocked()
    {
        // The container is unlocked. Assign the correct symbol item config from SymbolManager.
        ItemConfig config = SymbolManager.Instance.GetSymbolItemConfigForPuzzleID(puzzleID);
        if (config != null && symbolItem != null)
        {
            // Assign the symbol item config to the collectible item.
            symbolItem.SetItemConfig(config);
            Debug.Log($"Assigned symbol item '{config.itemName}' for puzzleID {puzzleID}.");
        }
        else
        {
            Debug.LogWarning($"No symbol item config found for puzzleID {puzzleID} or symbolItem missing.");
        }
    }
}