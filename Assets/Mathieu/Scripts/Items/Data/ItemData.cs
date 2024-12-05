using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/Items/ItemData")]
public class ItemData : ScriptableObject // TODO: Make this an abstract class.
{
    [Header("Basic Info")]
    public string itemName; // Display name of the item
    public Sprite icon; // Icon for UI representation
    public string description; // Description for tooltips or lore

    [Header("Classification")]
    public ItemType itemType; // Functional role (e.g., Key, Tool, etc.)
    public ItemCategory category; // Organizational grouping (e.g., Quest, Utility)

    [Header("Stacking Rules")]
    public bool isStackable; // Can this item stack in inventory?
    public int maxStackSize; // Maximum stack size (if stackable)

    [Header("Sound & Effects")]
    public AudioClip useSound; // Sound effect when used
    public ParticleSystem useEffect; // Optional visual effect

    [Header("Usage Rules")]
    public bool consumeOnUse; // Determines if the item is consumed after use
}