using UnityEngine;

[CreateAssetMenu(fileName = "ToolCondition", menuName = "ScriptableObjects/Conditions/ToolCondition")]
public class ToolConditionConfig : UnlockConditionConfig
{
    [Tooltip("The required tool item to unlock.")]
    public ItemConfig requiredTool;

    public override bool IsConditionMet(InventoryManager inventory)
    {
        return inventory != null && requiredTool != null && inventory.HasItem(requiredTool);
    }
}