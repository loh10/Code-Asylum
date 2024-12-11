/// <summary>
/// Interface defining the contract for puzzles in the game.
/// Puzzles should implement this to unify how they are activated and checked for completion.
/// </summary>
public interface IPuzzle
{
    /// <summary>
    /// Indicates whether the puzzle is currently solved.
    /// </summary>
    bool IsSolved { get; }

    /// <summary>
    /// A unique identifier for the puzzle, useful for referencing it in conditions.
    /// </summary>
    string PuzzleID { get; }

    /// <summary>
    /// Marks the puzzle as solved. This should be called when the player successfully completes it.
    /// </summary>
    void Solve();
}