using UnityEngine;

/// <summary>
/// Placed on the container in a puzzle room. Once unlocked, it assigns the correct symbol item
/// based on the puzzleID that rewards that symbol.
/// </summary>
public class SymbolContainer : MonoBehaviour
{
    [Tooltip("The puzzleID that this container rewards. Must match the puzzle's ID.")]
    public int puzzleID;

    [Tooltip("The collectible item inside the container, initially without config.")]
    public SymbolCollectibleItem symbolItem;

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
            symbolItem.SetItemConfig(config);
            Debug.Log($"Assigned symbol item '{config.itemName}' for puzzleID {puzzleID}.");
        }
        else
        {
            Debug.LogWarning($"No symbol item config found for puzzleID {puzzleID} or symbolItem missing.");
        }
    }
}