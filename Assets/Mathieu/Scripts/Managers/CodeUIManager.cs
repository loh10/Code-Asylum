using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeUIManager : MonoBehaviour
{
    public static CodeUIManager Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("The panel that contains the code input UI.")]
    public GameObject codePanel;

    [Tooltip("The input field where the player enters the code.")]
    public TMP_InputField codeInputField;

    [Tooltip("The button the player presses to confirm the code.")]
    public Button confirmButton;

    [Header("Dependencies")]
    [SerializeField] private PlayerController playerController;
    
    private Lock currentLock;
    private string requiredCode;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        HideCodePanel();
    }

    /// <summary>
    /// Shows the code panel and lets the player enter the code.
    /// Also disables player movement and unlocks the mouse cursor.
    /// </summary>
    public void ShowCodePanel(string requiredCode, Lock lockReference)
    {
        this.requiredCode = requiredCode;
        this.currentLock = lockReference;

        if (codePanel != null)
        {
            codePanel.SetActive(true);
        }

        if (codeInputField != null)
        {
            codeInputField.text = "";
            codeInputField.Select();
            codeInputField.ActivateInputField();
        }

        // Freeze player input
        FreezePlayerInput(true);

        // Unlock and show the cursor so the player can interact with UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Hides the code panel.
    /// Also re-enables player movement and locks the mouse cursor.
    /// </summary>
    public void HideCodePanel()
    {
        if (codePanel != null)
        {
            codePanel.SetActive(false);
        }

        // Unfreeze player input
        FreezePlayerInput(false);

        // Re-lock the cursor since we're done with UI
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Called when the player clicks the confirm button.
    /// Checks if the entered code is correct and, if so, records it.
    /// After that, hides the panel.
    /// </summary>
    private void OnConfirmButtonClicked()
    {
        if (codeInputField == null) return;

        string enteredCode = codeInputField.text.Trim();
        if (!string.IsNullOrEmpty(enteredCode))
        {
            if (enteredCode == requiredCode)
            {
                Debug.Log("Correct code entered!");
                CodeManager.Instance.RecordCode(requiredCode);
            }
            else
            {
                Debug.Log("Incorrect code entered.");
            }
        }

        HideCodePanel();
        // After entering code (correct or not), the player can interact with the lock again.
    }

    /// <summary>
    /// Helper method to freeze/unfreeze player input.
    /// Finds the PlayerController and calls FreezeInput().
    /// Make sure there's only one PlayerController in the scene.
    /// </summary>
    private void FreezePlayerInput(bool freeze)
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }

        if (playerController != null)
        {
            playerController.FreezeInput(freeze);
        }
        else
        {
            Debug.LogWarning("No PlayerController found in scene to freeze input.");
        }
    }
}