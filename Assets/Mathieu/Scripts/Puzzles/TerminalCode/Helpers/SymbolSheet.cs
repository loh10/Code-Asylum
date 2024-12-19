using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// When interacted with, displays a UI showing the symbol-to-value mapping.
/// </summary>
public class SymbolSheet : MonoBehaviour, IInteractable
{
    public bool IsInteractable => true;
    public string InteractionHint => collected ? "Press 'E' to view symbol values" : "Press 'E' to pick up and view symbol sheet";

    [SerializeField] private GameObject symbolSheetUI;
    [SerializeField] private Button closeButton;

    [Header("Item Config")]
    [SerializeField] private ItemConfig sheetItemConfig;

    private bool collected = false;

    private void Start()
    {
        closeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseAndDisable);
    }

    public void Interact(GameObject interactor)
    {
        InventoryManager inventory = interactor.GetComponent<InventoryManager>();
        if (inventory == null)
        {
            Debug.LogWarning("No InventoryManager found on interactor.");
            return;
        }

        if (!collected)
        {
            inventory.AddItem(sheetItemConfig);
            collected = true;
            Debug.Log($"{sheetItemConfig.itemName} collected. You can now view it in the terminal UI or by interacting here.");
        }

        if (symbolSheetUI.activeSelf) HideSheet();
        else ShowSheet();
    }

    private void ShowSheet()
    {
        symbolSheetUI.SetActive(true);
        MiniGameManager.currentMiniGame = symbolSheetUI;

        // Unlock cursor and freeze player input
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
    }

    private void HideSheet()
    {
        symbolSheetUI.SetActive(false);

        // Lock cursor and unfreeze player input
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
    }

    private void CloseAndDisable()
    {
        HideSheet();

        // Disable the entire game object after the UI is closed
        gameObject.SetActive(false);
    }
}
