using UnityEngine;

/// <summary>
/// Activates the Bonneteau puzzle when the player enters the trigger area.
/// </summary>
public class BonneteauTrigger : MonoBehaviour
{
    [Header("Puzzle Reference")]
    [Tooltip("Reference to the Bonneteau puzzle manager.")]
    [SerializeField] private Bonneteau bonneteauPuzzle;
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            if (bonneteauPuzzle != null)
            {
                // Activate the Bonneteau puzzle
                bonneteauPuzzle.Activate();

                // Show success message
                string message = DialogueManager.GetDialogue("Enigma", "BonneteauActivated");
                DialogueMessageBoxUI.Instance.ShowMessage(message, 2.5f);
            }
            else
            {
                Debug.LogError("Bonneteau puzzle reference is not set on the trigger.");
            }

            // Optionally, disable the trigger so it only works once
            gameObject.SetActive(false);
        }
    }
}