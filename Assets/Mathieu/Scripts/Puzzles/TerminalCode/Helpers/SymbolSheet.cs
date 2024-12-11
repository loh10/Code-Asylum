using UnityEngine;

/// <summary>
/// When interacted with, displays a UI showing the symbol-to-value mapping.
/// UI can be a simple panel listing all symbols and their values.
/// </summary>
public class SymbolSheet : MonoBehaviour, IInteractable
{
    public bool IsInteractable => true;
    public string InteractionHint => "Press 'E' to view symbol values";

    [SerializeField] private GameObject symbolSheetUI;
    [SerializeField] private GameObject closeButton;

    private void Start()
    {
        closeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(HideSheet);
    }
    public void Interact(GameObject interactor)
    {
        if (symbolSheetUI.activeSelf) HideSheet();
        else ShowSheet();
    }

    private void ShowSheet()
    {
        symbolSheetUI.SetActive(true);
        
        // Freeze player input if needed, unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
    }

    private void HideSheet()
    {
        symbolSheetUI.SetActive(false);
        
        // Re-lock cursor and enable input
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
    }
}