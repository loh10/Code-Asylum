using UnityEngine;

[CreateAssetMenu(fileName = "CodeCondition", menuName = "ScriptableObjects/Conditions/CodeCondition")]
public class CodeConditionConfig : UnlockConditionConfig
{
    [Tooltip("The code that unlocks the lock.")]
    public string requiredCode;

    public override bool IsConditionMet(InventoryManager inventory)
    {
        return CodeManager.Instance != null && CodeManager.Instance.HasEnteredCode(requiredCode);
    }
}