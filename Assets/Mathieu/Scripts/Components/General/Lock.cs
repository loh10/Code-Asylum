using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents a lock that can be unlocked by meeting specified conditions.
/// Conditions are defined as ScriptableObjects that implement IUnlockCondition.
/// </summary>
public class Lock : MonoBehaviour, IInteractable
{
    [Header("Lock Initial State")]
    [Tooltip("Determines the initial state of the lock.")]
    public bool isLocked = true;
    
    [Header("Unlock Conditions")]
    [Tooltip("If true, ALL conditions must be met to unlock. If false, ANY one condition is enough.")]
    public bool unlockRequiresAllConditions = false;
    
    [Tooltip("A list of condition assets that must be met to unlock this lock.")]
    public List<UnlockConditionConfig> unlockConditions = new List<UnlockConditionConfig>();

    public bool IsInteractable => isLocked;
    public string InteractionHint => isLocked ? "Press 'E' to unlock" : "Already unlocked.";

    public event Action OnUnlock;

    public void Interact(GameObject interactor)
    {
        if (!IsInteractable)
        {
            Debug.Log("This lock is already unlocked.");
            return;
        }

        InventoryManager inventory = interactor.GetComponent<InventoryManager>();
        if (inventory == null)
        {
            Debug.Log("No inventory system found on interactor.");
            return;
        }

        // Get the conditions that are currently met
        List<UnlockConditionConfig> metConditions = GetMetConditions(inventory);

        bool conditionMet = metConditions.Count > 0;

        if (unlockConditions.Count <= 0)
        {
            OnUnlock?.Invoke();
            isLocked = false;
            return;
        }

        if (conditionMet)
        {
            isLocked = false;
            Debug.Log("Lock unlocked!");

            // Consume items from the met conditions if required
            ConsumeItemsFromConditions(metConditions, inventory);

            OnUnlock?.Invoke();
        }
        else
        {
            Debug.Log("Conditions not met.");

            CodeConditionConfig codeCond = GetUnmetCodeCondition(inventory);
            if (codeCond != null)
            {
                // Show code panel
                if (CodeUIManager.Instance != null)
                {
                    CodeUIManager.Instance.ShowCodePanel(codeCond.requiredCode, this);
                }
                else
                {
                    Debug.LogWarning("No CodeUIManager in scene to show code panel.");
                }
            }
        }
    }

    /// <summary>
    /// Returns a list of conditions that are currently met, based on unlockRequiresAllConditions.
    /// </summary>
    private List<UnlockConditionConfig> GetMetConditions(InventoryManager inventory)
    {
        List<UnlockConditionConfig> metConds = new List<UnlockConditionConfig>();

        if (unlockConditions.Count == 0) return metConds;

        if (unlockRequiresAllConditions)
        {
            // All conditions must be met
            foreach (var c in unlockConditions)
            {
                if (!c.IsConditionMet(inventory))
                {
                    // If one is not met, none qualify
                    metConds.Clear();
                    return metConds;
                }
            }
            // If we reach here, all are met
            metConds.AddRange(unlockConditions);
        }
        else
        {
            // ANY one condition is enough
            foreach (var c in unlockConditions)
            {
                if (c.IsConditionMet(inventory))
                {
                    // Return just this one condition for consumption
                    metConds.Add(c);
                    break;
                }
            }
        }

        return metConds;
    }

    /// <summary>
    /// Consumes any items associated with the given conditions if those items have consumeOnUse = true.
    /// </summary>
    private void ConsumeItemsFromConditions(List<UnlockConditionConfig> conditions, InventoryManager inventory)
    {
        // Conditions that might have items: KeyConditionConfig, ToolConditionConfig
        foreach (var cond in conditions)
        {
            // Check if the condition is a KeyCondition or ToolCondition
            if (cond is KeyConditionConfig keyCond && keyCond.requiredKey != null)
            {
                TryConsumeItem(keyCond.requiredKey, inventory);
            }
            else if (cond is ToolConditionConfig toolCond && toolCond.requiredTool != null)
            {
                TryConsumeItem(toolCond.requiredTool, inventory);
            }
        }
    }

    /// <summary>
    /// If the item is consumeOnUse, remove one instance from the inventory.
    /// </summary>
    private void TryConsumeItem(ItemConfig item, InventoryManager inventory)
    {
        if (item.consumeOnUse)
        {
            if (inventory.HasItem(item))
            {
                inventory.RemoveItem(item);
                Debug.Log($"{item.itemName} has been consumed on use.");
            }
        }
    }

    /// <summary>
    /// Returns a CodeConditionConfig that isn't met yet, if any, so we can prompt for code entry.
    /// </summary>
    private CodeConditionConfig GetUnmetCodeCondition(InventoryManager inventory)
    {
        foreach (var condition in unlockConditions)
        {
            CodeConditionConfig codeCond = condition as CodeConditionConfig;
            if (codeCond != null && !codeCond.IsConditionMet(inventory))
            {
                return codeCond;
            }
        }
        return null;
    }
}