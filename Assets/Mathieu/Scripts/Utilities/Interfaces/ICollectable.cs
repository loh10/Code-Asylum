using UnityEngine;

/// <summary>
/// Interface for objects that can be collected and added to the inventory.
/// </summary>
public interface ICollectable
{
    /// <summary>
    /// Determines if the object can currently be collected.
    /// </summary>
    bool CanCollect { get; }

    /// <summary>
    /// Called when the player collects this object.
    /// </summary>
    void OnCollect(GameObject collector);

    /// <summary>
    /// The hint displayed to the player when looking at this collectable object.
    /// </summary>
    string CollectHint { get; }
}