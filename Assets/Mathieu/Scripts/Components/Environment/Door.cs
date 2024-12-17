using UnityEngine;

/// <summary>
/// A door that can be opened and closed. Requires a Lock component to unlock, either assigned in the inspector or found on the same GameObject.
/// </summary>
public class Door : MonoBehaviour
{
    [Header("Door Properties")]
    public Animator doorAnimator;

    [Tooltip("If your lock is on a different GameObject, assign it here.")]
    [SerializeField] private Lock lockComponent;

    [Header("Handle References")]
    [Tooltip("The mesh renderer of the handle.")]
    [SerializeField] private MeshRenderer handleRenderer;

    [Tooltip("Material used when door is locked.")]
    [SerializeField] private Material lockedMaterial;

    [Tooltip("Material used when door is unlocked.")]
    [SerializeField] private Material unlockedMaterial;

    private void Awake()
    {
        if (lockComponent == null)
        {
            lockComponent = GetComponent<Lock>();
        }

        if (lockComponent != null)
        {
            lockComponent.OnUnlock += OnDoorUnlocked;
        }
        else
        {
            Debug.LogWarning("No Lock component assigned or found on the door.");
        }

        // Set handle to locked material initially if handleRenderer is assigned
        if (handleRenderer != null && lockedMaterial != null)
        {
            handleRenderer.material = lockedMaterial;
        }
    }

    private void OnDestroy()
    {
        if (lockComponent != null)
        {
            lockComponent.OnUnlock -= OnDoorUnlocked;
        }
    }

    private void OnDoorUnlocked()
    {
        if (doorAnimator != null)
        {
            doorAnimator.enabled = true;
            AudioManager.Instance.PlaySound(AudioType.door, AudioSourceType.player);
        }
        else
        {
            // Fallback if no animator
            gameObject.SetActive(false);
        }

        // Change handle material to unlocked
        if (handleRenderer != null && unlockedMaterial != null)
        {
            handleRenderer.material = unlockedMaterial;
        }

        // Show a dialogue indicating the door is now open
        string message = DialogueManager.GetDialogue("Door", "KeyActivated");
        if (!string.IsNullOrEmpty(message))
        {
            DialogueMessageBoxUI.Instance.ShowMessage(message, 3f);
        }
    }

    public void CloseDoor()
    {
        // Implement door closing if needed
    }
}