using System.Collections.Generic;
using UnityEngine;
using static DialogueManager;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<string, int> _inventoryItem = new Dictionary<string, int>();
    [SerializeField]
    private GameObject _inventoryUI;
    private bool _isDisplay;

    void AddItem(string item)
    {
        Debug.Log(DialogueManager.GetDialogue("Dialogue", "Text"));
        if (_inventoryItem.ContainsKey(item))
        {
            _inventoryItem[item]++;
        }
        else
        {
            _inventoryItem[item] = 1;
        }

    }



    void UseItem(string item)
    {
        //ToComplete thanks to Mathieu
    }



    void RemoveItem(string item)
    {
        if (_inventoryItem.ContainsKey(item))
        {
            _inventoryItem[item]--;
        }
        else
        {
            _inventoryItem.Remove(item);
        }
    }



    void ToggleInventoryDisplay(bool display)
    {
        _isDisplay = display;

        if (_isDisplay)
        {
            _inventoryUI.SetActive(true);
        }
        else
        {
            _inventoryUI.SetActive(false);
        }


    }
}
