using UnityEngine;

/// <summary>
/// A basic test implementation of the IPuzzle interface.
/// Allows interacting with the object to mark the puzzle as solved.
/// </summary>
public class TestPuzzle : MonoBehaviour, IPuzzle, IInteractable
{
    [Header("Puzzle Configuration")]
    [SerializeField] private int _PuzzleID;
    [SerializeField] private string _PuzzleHint;

    public bool IsSolved { get; set; }
    public string PuzzleHint { get; set; }
    public int PuzzleID { get; set; }

    public bool IsInteractable => !IsSolved;
    public string InteractionHint => IsSolved ? "Already solved" : "Press 'E' to solve the puzzle";

    private void Awake()
    {
        PuzzleID = _PuzzleID;
        PuzzleHint = _PuzzleHint;
    }

    public void Interact(GameObject interactor)
    {
        Activate();
    }

    public void Activate()
    {
        if (IsSolved) return;
        Solve();
    }

    public void Solve()
    {
        if (IsSolved) return;
        
        IsSolved = true;
        PuzzleManager.Instance.SetPuzzleSolved(PuzzleID);
        Debug.Log($"Puzzle {PuzzleID} has been solved!");
        string message = DialogueManager.GetDialogue("Enigma", "GotSymbol");
        DialogueMessageBoxUI.Instance.ShowMessage(message, 3f);
    }
}