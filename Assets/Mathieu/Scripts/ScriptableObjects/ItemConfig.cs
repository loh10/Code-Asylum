using UnityEngine;

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

    [Header("3D Model")]
    public GameObject modelPrefab;
}