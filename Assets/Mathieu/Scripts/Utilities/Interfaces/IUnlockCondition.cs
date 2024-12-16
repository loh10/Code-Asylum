/// <summary>
/// Interface for conditions that must be met to unlock a lock.
/// </summary>
public interface IUnlockCondition
{
    /// <summary>
    /// Returns true if the condition required to unlock is currently met.
    /// </summary>
    bool IsConditionMet(InventoryManager inventory);
}