using UnityEngine;

public class TerminalUIEnhancer : MonoBehaviour
{
    [Header("Item Configs for Manual and Sheet")]
    [SerializeField] private ItemConfig manualItemConfig;
    [SerializeField] private ItemConfig sheetItemConfig;

    [Header("UI Panels")]
    [Tooltip("UI panel for the manual displayed inside the terminal UI.")]
    [SerializeField] private GameObject manualPanelInTerminal;

    [Tooltip("UI panel for the symbol sheet displayed inside the terminal UI.")]
    [SerializeField] private GameObject sheetPanelInTerminal;

    private InventoryManager _inventoryManager;

    private void Awake()
    {
        // Disable panels initially
        if (manualPanelInTerminal != null)
            manualPanelInTerminal.SetActive(false);
        if (sheetPanelInTerminal != null)
            sheetPanelInTerminal.SetActive(false);
    }

    private void Start()
    {
        _inventoryManager = FindFirstObjectByType<InventoryManager>();
        if (_inventoryManager != null)
        {
            _inventoryManager.OnItemAdded += OnItemAdded;
        }
        else
        {
            Debug.LogWarning("No InventoryManager found. Manual and sheet won't appear in the terminal UI.");
        }
    }

    private void OnDestroy()
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.OnItemAdded -= OnItemAdded;
        }
    }

    private void OnItemAdded(ItemConfig item)
    {
        // If the player collects the manual or sheet, enable their panels in the terminal UI.
        if (item == manualItemConfig && manualPanelInTerminal != null)
        {
            manualPanelInTerminal.SetActive(true);
            Debug.Log("Manual panel now available in the terminal UI.");
        }

        if (item == sheetItemConfig && sheetPanelInTerminal != null)
        {
            sheetPanelInTerminal.SetActive(true);
            Debug.Log("Sheet panel now available in the terminal UI.");
        }
    }
}