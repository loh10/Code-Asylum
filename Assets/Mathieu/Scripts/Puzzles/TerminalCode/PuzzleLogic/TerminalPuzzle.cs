using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerminalPuzzle : MonoBehaviour, IPuzzle, IInteractable
{
    [Header("Puzzle Configuration")]
    [SerializeField] private int _PuzzleID;
    [SerializeField] private string _PuzzleHint;

    [Header("UI References")]
    public GameObject terminalUI;
    public GameObject codeEntryPanel;

    public TMP_InputField result1Field;
    public TMP_InputField result2Field;
    public TMP_InputField result3Field;
    public Button confirmButton;
    public Button closeButton;

    [Header("Auto-Solve Button")]
    public Button autoSolveButton;
    [Tooltip("Input this code in the terminal and press Confirm to unlock the Auto-Solve button.")]
    [SerializeField] private string devCode = "69 69 69";

    [Header("Screen Shake Configuration")]
    public Transform cameraTransform;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    public bool IsSolved { get; set; }
    public string PuzzleHint { get; set; }
    public int PuzzleID { get; set; }

    public bool IsInteractable => true;
    public string InteractionHint => "Press 'E' to access terminal";

    private void Awake()
    {
        PuzzleID = _PuzzleID;
        PuzzleHint = _PuzzleHint;

        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmClicked);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideTerminal);
        }

        if (autoSolveButton != null)
        {
            autoSolveButton.onClick.AddListener(AutoSolve);
            autoSolveButton.gameObject.SetActive(false); // Hidden by default
        }

        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
            else
            {
                Debug.LogError("No cameraTransform assigned and no Main Camera found!");
            }
        }

        HideTerminal();
    }

    public void Interact(GameObject interactor)
    {
        if (terminalUI.activeSelf) HideTerminal();
        else Activate();
    }

    public void Activate()
    {
        if (IsSolved) return;

        terminalUI.SetActive(true);

        if (codeEntryPanel != null)
            codeEntryPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
    }

    public void Solve()
    {
        if (!IsSolved)
        {
            IsSolved = true;
            PuzzleManager.Instance.SetPuzzleSolved(PuzzleID);
            Debug.Log($"Puzzle {PuzzleID} solved.");
        }
    }

    private void HideTerminal()
    {
        terminalUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
    }

    private void OnConfirmClicked()
    {
        string enteredCode = $"{result1Field.text.Trim()} {result2Field.text.Trim()} {result3Field.text.Trim()}";
        
        string normalizedEnteredCode = enteredCode.Replace(" ", string.Empty);
        string normalizedDevCode = devCode.Replace(" ", string.Empty);
        
        if (normalizedEnteredCode == normalizedDevCode)
        {
            Debug.Log("Developer Code entered. Auto-Solve button unlocked.");
            EnableAutoSolveButton();
            ResetInputFields();
            return;
        }

        if (int.TryParse(result1Field.text.Trim(), out int R1) &&
            int.TryParse(result2Field.text.Trim(), out int R2) &&
            int.TryParse(result3Field.text.Trim(), out int R3))
        {
            if (CheckCode(R1, R2, R3))
            {
                Debug.Log("Correct code!");
                Solve();
                HideTerminal();
            }
            else
            {
                Debug.Log("Incorrect code. Try again.");
                ResetInputFields();
                TriggerScreenShake();
            }
        }
        else
        {
            Debug.Log("Invalid input. Please enter numeric values.");
            ResetInputFields();
            TriggerScreenShake();
        }
    }

    private bool CheckCode(int R1, int R2, int R3)
    {
        string Xs = SymbolManager.Instance.GetSymbolX();
        string Ys = SymbolManager.Instance.GetSymbolY();
        string Zs = SymbolManager.Instance.GetSymbolZ();

        int X = SymbolManager.Instance.GetSymbolValue(Xs);
        int Y = SymbolManager.Instance.GetSymbolValue(Ys);
        int Z = SymbolManager.Instance.GetSymbolValue(Zs);

        int calcR1 = ((3 * X + Y) / 2) - Z;
        int calcR2 = ((2 * Y + Z) / 2) + X;
        int calcR3 = ((X + Y + Z) / 2) + 4;

        return (calcR1 == R1 && calcR2 == R2 && calcR3 == R3);
    }

    private void ResetInputFields()
    {
        result1Field.text = string.Empty;
        result2Field.text = string.Empty;
        result3Field.text = string.Empty;
    }

    private void TriggerScreenShake()
    {
        if (cameraTransform != null)
        {
            StartCoroutine(ScreenShakeUtility.TriggerScreenShake(cameraTransform, shakeDuration, shakeMagnitude));
        }
        else
        {
            Debug.LogWarning("No cameraTransform available for screen shake.");
        }
    }

    private void EnableAutoSolveButton()
    {
        if (autoSolveButton != null)
        {
            autoSolveButton.gameObject.SetActive(true);
        }
    }

    private void AutoSolve()
    {
        string Xs = SymbolManager.Instance.GetSymbolX();
        string Ys = SymbolManager.Instance.GetSymbolY();
        string Zs = SymbolManager.Instance.GetSymbolZ();

        int X = SymbolManager.Instance.GetSymbolValue(Xs);
        int Y = SymbolManager.Instance.GetSymbolValue(Ys);
        int Z = SymbolManager.Instance.GetSymbolValue(Zs);

        result1Field.text = (((3 * X + Y) / 2) - Z).ToString();
        result2Field.text = (((2 * Y + Z) / 2) + X).ToString();
        result3Field.text = (((X + Y + Z) / 2) + 4).ToString();

        Debug.Log("Auto-solve filled the fields with correct values.");
    }
}
