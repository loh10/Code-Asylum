using UnityEngine;

/// <summary>
/// Placed on the container in a puzzle room. Once unlocked, it spawns/assigns the correct symbol item.
/// </summary>
public class PuzzleSymbolContainer : MonoBehaviour
{
    [Tooltip("The puzzleID that this container rewards. Must match the puzzle's ID.")]
    public string puzzleID;

    [Tooltip("The collectible item inside the container, initially disabled or without config.")]
    public SymbolCollectibleItem symbolItem;

    private Lock containerLock;

    private void Awake()
    {
        containerLock = GetComponent<Lock>();
        if (containerLock != null)
        {
            containerLock.OnUnlock += OnContainerUnlocked;
        }
    }

    private void OnDestroy()
    {
        if (containerLock != null)
        {
            containerLock.OnUnlock -= OnContainerUnlocked;
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