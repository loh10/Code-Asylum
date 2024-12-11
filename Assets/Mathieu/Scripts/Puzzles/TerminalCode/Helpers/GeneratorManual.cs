using UnityEngine;

/// <summary>
/// When interacted with, shows equations to the player.
/// Equations reference X,Y,Z as the collected symbols.
/// </summary>
public class GeneratorManual : MonoBehaviour, IInteractable
{
    public bool IsInteractable => true;
    public string InteractionHint => "Press 'E' to view equations";

    [SerializeField] private GameObject manualUI; 

    public void Interact(GameObject interactor)
    {
        if (manualUI.activeSelf) HideManual();
        else ShowManual();
    }

    private void ShowManual()
    {
        manualUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
    }

    private void HideManual()
    {
        manualUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
    }
}