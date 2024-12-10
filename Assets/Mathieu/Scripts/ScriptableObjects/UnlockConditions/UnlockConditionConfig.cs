using UnityEngine;

/// <summary>
/// Abstract base class for all unlock conditions using ScriptableObjects.
/// </summary>
public abstract class UnlockConditionConfig : ScriptableObject, IUnlockCondition
{
    public abstract bool IsConditionMet(InventoryManager inventory);
}