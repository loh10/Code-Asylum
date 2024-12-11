using UnityEngine;
using UnityEngine.UI;

public class SymbolRoomDisplay : MonoBehaviour
{
    [Header("UI Image References for Symbols (X, Y, Z)")]
    [Tooltip("UI Image for Symbol X.")]
    public Image symbolXImage;
    
    [Tooltip("UI Image for Symbol Y.")]
    public Image symbolYImage;
    
    [Tooltip("UI Image for Symbol Z.")]
    public Image symbolZImage;

    private ItemConfig xConfig;
    private ItemConfig yConfig;
    private ItemConfig zConfig;

    private InventoryManager playerInventory;
    
    private Color transparent = new Color(1, 1, 1, 0);
    private Color solid = new Color(1, 1, 1, 1);

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

        // Hide or set empty images initially
        ResetSymbolImages();

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

    private void ResetSymbolImages()
    {
        if (symbolXImage != null)
        {
            symbolXImage.sprite = null;
            symbolXImage.color = transparent;
            symbolXImage.enabled = false;
        }

        if (symbolYImage != null)
        {
            symbolYImage.sprite = null;
            symbolYImage.color = transparent;
            symbolYImage.enabled = false;
        }

        if (symbolZImage != null)
        {
            symbolZImage.sprite = null;
            symbolZImage.color = transparent;
            symbolZImage.enabled = false;
        }
    }

    private void OnItemAdded(ItemConfig item)
    {
        // Check if the added item matches any of the configured symbols
        if (item == xConfig && symbolXImage != null)
        {
            UpdateSymbolImage(symbolXImage, xConfig.icon);
        }
        else if (item == yConfig && symbolYImage != null)
        {
            UpdateSymbolImage(symbolYImage, yConfig.icon);
        }
        else if (item == zConfig && symbolZImage != null)
        {
            UpdateSymbolImage(symbolZImage, zConfig.icon);
        }
    }

    private void UpdateSymbolImage(Image targetImage, Sprite icon)
    {
        if (targetImage != null)
        {
            targetImage.sprite = icon;
            targetImage.color = solid;
            targetImage.enabled = true;
        }
    }
}
