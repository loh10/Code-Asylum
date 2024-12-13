using UnityEngine;

[CreateAssetMenu(fileName = "KeyCondition", menuName = "ScriptableObjects/Conditions/KeyCondition")]
public class KeyConditionConfig : UnlockConditionConfig
{
    [Tooltip("The required key item to unlock.")]
    public ItemConfig requiredKey;

    public override bool IsConditionMet(InventoryManager inventory)
    {
        return inventory != null && requiredKey != null && inventory.HasItem(requiredKey);
    }
}