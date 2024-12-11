using UnityEngine;
using UnityEngine.UI;

public class SymbolRoomDisplay : MonoBehaviour
{
    [Header("Symbol Image References (X, Y, Z)")]
    public Image symbolXImage;
    public Image symbolYImage;
    public Image symbolZImage;

    private ItemConfig xConfig;
    private ItemConfig yConfig;
    private ItemConfig zConfig;

    private InventoryManager playerInventory;

    private void Start()
    {
        // Get player inventory
        playerInventory = FindFirstObjectByType<InventoryManager>();
        if (playerInventory == null)
        {
            Debug.LogWarning("No InventoryManager found. SymbolRoomDisplay won't update.");
            return;
        }

        // Identify which ItemConfigs correspond to X, Y, Z
        xConfig = SymbolManager.Instance.GetSymbolItemConfigForPuzzleID(SymbolManager.Instance.puzzleID_X);
        yConfig = SymbolManager.Instance.GetSymbolItemConfigForPuzzleID(SymbolManager.Instance.puzzleID_Y);
        zConfig = SymbolManager.Instance.GetSymbolItemConfigForPuzzleID(SymbolManager.Instance.puzzleID_Z);

        // Hide or set empty sprites initially
        if (symbolXImage != null) symbolXImage.enabled = false;
        if (symbolYImage != null) symbolYImage.enabled = false;
        if (symbolZImage != null) symbolZImage.enabled = false;

        // Subscribe to inventory events
        playerInventory.OnItemAdded += OnItemAdded;
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnItemAdded -= OnItemAdded;
        }
    }

    private void OnItemAdded(ItemConfig item)
    {
        // Check if this item is one of the symbol items
        if (item == xConfig && symbolXImage != null)
        {
            symbolXImage.sprite = xConfig.icon;
            symbolXImage.enabled = true;
        }
        else if (item == yConfig && symbolYImage != null)
        {
            symbolYImage.sprite = yConfig.icon;
            symbolYImage.enabled = true;
        }
        else if (item == zConfig && symbolZImage != null)
        {
            symbolZImage.sprite = zConfig.icon;
            symbolZImage.enabled = true;
        }
    }
}
