using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// Determines whether the object is currently interactable.
    /// </summary>
    bool IsInteractable { get; }

    /// <summary>
    /// The hint displayed to the player when they can interact with the object.
    /// </summary>
    string InteractionHint { get; }

    /// <summary>
    /// Executes the interaction logic when the player interacts with the object.
    /// </summary>
    void Interact(GameObject interactor);
}
