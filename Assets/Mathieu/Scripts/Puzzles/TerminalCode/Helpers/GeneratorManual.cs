using UnityEngine;

/// <summary>
/// When interacted with, shows equations to the player.
/// Equations reference X,Y,Z as the collected symbols.
/// </summary>
public class GeneratorManual : MonoBehaviour, IInteractable
{
    public bool IsInteractable => true;
    public string InteractionHint => collected ? "Press 'E' to view the manual" : "Press 'E' to pick up and view manual";

    [SerializeField] private GameObject manualUI;
    [SerializeField] private GameObject closeButton;

    [Header("Item Config")]
    [SerializeField] private ItemConfig manualItemConfig;
    
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
            // Collect the item, but do not destroy the object
            inventory.AddItem(manualItemConfig);
            collected = true;
            Debug.Log($"{manualItemConfig.itemName} collected. You can now view it in the terminal UI or by interacting here.");
        }

        // If collected, just show/hide UI based on current state
        if (manualUI.activeSelf) HideManual();
        else ShowManual();
    }

    private void ShowManual()
    {
        manualUI.SetActive(true);
        MiniGameManager.currentMiniGame = manualUI;
        
        // Unlock cursor and freeze player input
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
    }

    private void HideManual()
    {
        manualUI.SetActive(false);
        
        // Lock cursor and unfreeze player input
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
    }

    private void CloseAndDisable()
    {
        HideManual();

        // Disable the entire game object after the UI is closed
        gameObject.SetActive(false);
    }
}