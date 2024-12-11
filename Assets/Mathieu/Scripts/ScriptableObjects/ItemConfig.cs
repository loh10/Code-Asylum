using UnityEngine;

/// <summary>
/// Base scriptable object defining general item properties.
/// </summary>
[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/Items/ItemConfig")]
public class ItemConfig : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName; 
    public Sprite icon; 
    public string description;

    [Header("Classification")]
    public ItemType itemType; 
    public ItemCategory category;

    [Header("Stacking Rules")]
    public bool isStackable;
    public int maxStackSize;

    [Header("Sound & Effects")]
    public AudioClip useSound; 
    public ParticleSystem useEffect;

    [Header("Usage Rules")]
    public bool consumeOnUse;
}