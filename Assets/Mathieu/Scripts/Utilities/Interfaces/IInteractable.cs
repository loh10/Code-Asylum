using UnityEngine;

/// <summary>
/// Interface for objects the player can interact with (e.g., doors, locks, etc.)
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Determines whether the object is currently interactable.
    /// </summary>
    bool IsInteractable { get; }

    /// <summary>
    /// The hint displayed to the player when looking at this interactable object.
    /// </summary>
    string InteractionHint { get; }

    /// <summary>
    /// Executes interaction logic when the player interacts with the object.
    /// </summary>
    void Interact(GameObject interactor);
}