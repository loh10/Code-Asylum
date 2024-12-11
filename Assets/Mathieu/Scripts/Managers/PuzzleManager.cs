using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the solved state of puzzles by their puzzleID.
/// Puzzles call SetPuzzleSolved(puzzleID) when completed.
/// PuzzleConditionConfig checks IsPuzzleSolved(puzzleID).
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    private Dictionary<string, bool> puzzleStates = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Marks a puzzle as solved by puzzleID.
    /// </summary>
    public void SetPuzzleSolved(string puzzleID)
    {
        puzzleStates[puzzleID] = true;
        Debug.Log($"Puzzle {puzzleID} set to solved.");
    }

    /// <summary>
    /// Checks if a puzzle is solved.
    /// </summary>
    public bool IsPuzzleSolved(string puzzleID)
    {
        return puzzleStates.TryGetValue(puzzleID, out bool solved) && solved;
    }
}