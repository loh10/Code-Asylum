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
    public bool isLocked = true;

    [Header("Unlock Conditions")]
    public bool unlockRequiresAllConditions = true;
    public List<UnlockConditionConfig> unlockConditions = new List<UnlockConditionConfig>();

    public bool IsInteractable => true;
    public string InteractionHint => "Press 'E' to interact";

    public event Action OnUnlock;

    public void Interact(GameObject interactor)
    {
        if (!isLocked) return;

        InventoryManager inventory = interactor.GetComponent<InventoryManager>();
        if (inventory == null)
        {
            Debug.Log("No inventory system found on interactor.");
            return;
        }

        // Check conditions
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
            // Debug.Log("Lock unlocked!");
            
            isLocked = false;
            
            // Show "unlocked" message
            string lockedMessage = DialogueManager.GetDialogue("Door", "DoorUnlocked");
            if (!string.IsNullOrEmpty(lockedMessage))
            {
                DialogueMessageBoxUI.Instance.ShowMessage(lockedMessage, 3f);
            }
            
            ConsumeItemsFromConditions(metConditions, inventory);
            OnUnlock?.Invoke();
        }
        else
        {
            // Debug.Log("Conditions not met.");
            
            // Show "locked" message
            string lockedMessage = DialogueManager.GetDialogue("Door", "DoorLocked");
            if (!string.IsNullOrEmpty(lockedMessage))
            {
                DialogueMessageBoxUI.Instance.ShowMessage(lockedMessage, 3f);
            }

            // Check if there's a code to enter
            CodeConditionConfig codeCond = GetUnmetCodeCondition(inventory);
            if (codeCond != null)
            {
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

    private List<UnlockConditionConfig> GetMetConditions(InventoryManager inventory)
    {
        List<UnlockConditionConfig> metConds = new List<UnlockConditionConfig>();

        if (unlockConditions.Count == 0) return metConds;

        if (unlockRequiresAllConditions)
        {
            foreach (var c in unlockConditions)
            {
                if (!c.IsConditionMet(inventory))
                {
                    metConds.Clear();
                    return metConds;
                }
            }
            metConds.AddRange(unlockConditions);
        }
        else
        {
            foreach (var c in unlockConditions)
            {
                if (c.IsConditionMet(inventory))
                {
                    metConds.Add(c);
                    break;
                }
            }
        }

        return metConds;
    }

    private void ConsumeItemsFromConditions(List<UnlockConditionConfig> conditions, InventoryManager inventory)
    {
        foreach (var cond in conditions)
        {
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

    private void TryConsumeItem(ItemConfig item, InventoryManager inventory)
    {
        if (item.consumeOnUse && inventory.HasItem(item))
        {
            inventory.RemoveItem(item);
            Debug.Log($"{item.itemName} has been consumed on use.");
        }
    }

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