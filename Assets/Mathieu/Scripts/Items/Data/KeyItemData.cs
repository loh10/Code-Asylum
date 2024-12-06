using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyItemData", menuName = "ScriptableObjects/Items/NewKeyData")]
public class KeyItemData : ItemData
{
    public string[] unlockTargetIDs; // List of lock IDs this key can unlock
}