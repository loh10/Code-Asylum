using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleCondition", menuName = "ScriptableObjects/Conditions/PuzzleCondition")]
public class PuzzleConditionConfig : UnlockConditionConfig
{
    [Tooltip("The puzzleID that must be solved.")]
    public string puzzleID;

    public override bool IsConditionMet(InventoryManager inventory)
    {
        return PuzzleManager.Instance != null && PuzzleManager.Instance.IsPuzzleSolved(puzzleID);
    }
}