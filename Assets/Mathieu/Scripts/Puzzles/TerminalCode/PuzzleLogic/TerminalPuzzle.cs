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
    public TMP_InputField result1Field;
    public TMP_InputField result2Field;
    public TMP_InputField result3Field;
    public Button confirmButton;
    public Button closeButton;
    
    public bool IsSolved { get; set; }
    public string PuzzleHint { get; set; }
    public int PuzzleID { get; set; }

    public bool IsInteractable => true;
    public string InteractionHint => "Press 'E' to enter code";

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
            }
        }
        else
        {
            Debug.Log("Invalid input. Please enter numeric values.");
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
}

