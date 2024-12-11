using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleCondition", menuName = "ScriptableObjects/Conditions/PuzzleCondition")]
public class PuzzleConditionConfig : UnlockConditionConfig
{
    [Tooltip("Reference to the puzzle ID that must be solved.")]
    public string puzzleID; // TODO: Replace string with an enum or a reference to a scriptable object

    public override bool IsConditionMet(InventoryManager inventory)
    {
        return PuzzleManager.Instance != null && PuzzleManager.Instance.IsPuzzleSolved(puzzleID);
    }
}