using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global PuzzleManager handling puzzle states by PuzzleID.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    private HashSet<string> solvedPuzzles = new HashSet<string>(); // TODO: Replace string with an enum or a reference to a scriptable object

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
    /// Checks if a puzzle with the given ID is solved.
    /// </summary>
    public bool IsPuzzleSolved(string puzzleID) // TODO: Replace string with an enum or a reference to a scriptable object
    {
        return solvedPuzzles.Contains(puzzleID);
    }

    /// <summary>
    /// Marks a puzzle as solved.
    /// </summary>
    public void MarkPuzzleAsSolved(string puzzleID) // TODO: Replace string with an enum or a reference to a scriptable object
    {
        solvedPuzzles.Add(puzzleID);
        Debug.Log($"Puzzle {puzzleID} marked as solved.");
    }
}