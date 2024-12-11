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
    
    [Header("Door Reference")]
    [Tooltip("Door that needs to be opened when the container is unlocked.")]
    [SerializeField] private GameObject containerDoor;

    private Lock _containerLock;

    private void Awake()
    {
        _containerLock = GetComponent<Lock>();
        if (_containerLock != null)
        {
            _containerLock.OnUnlock += OnContainerUnlocked;
        }
    }

    private void OnDestroy()
    {
        if (_containerLock != null)
        {
            _containerLock.OnUnlock -= OnContainerUnlocked;
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
            
            // Open the door.
            if (containerDoor != null)
            {
                containerDoor.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning($"No symbol item config found for puzzleID {puzzleID} or symbolItem missing.");
        }
    }
}