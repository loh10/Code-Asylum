using UnityEngine;

/// <summary>
/// A door that can be opened and closed. Requires a Lock component to unlock, either assigned in the inspector or found on the same GameObject.
/// </summary>
public class Door : MonoBehaviour
{
    [Header("Door Properties")]
    [Tooltip("Animator for the door. Used for opening/closing animations.")]
    public Animator doorAnimator;

    [Tooltip("If your lock is on a different GameObject, assign it here.")]
    [SerializeField] private Lock lockComponent;

    private void Awake()
    {
        // Try to get the lock component from the same GameObject if not assigned in the inspector
        if (lockComponent == null)
        {
            lockComponent = GetComponent<Lock>();
        }

        // Subscribe to the OnUnlock event if a lock component is found
        if (lockComponent != null)
        {
            lockComponent.OnUnlock += OpenDoor;
        }
        else
        {
            Debug.LogWarning("No Lock component assigned or found on the door.");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnUnlock event
        if (lockComponent != null)
        {
            lockComponent.OnUnlock -= OpenDoor;
        }
    }

    private void OpenDoor()
    {
        if (doorAnimator != null)
        {
            // Trigger the door opening animation
            doorAnimator.enabled = true;
        }
        else
        {
            Debug.LogWarning("No Animator component assigned to the door. Disabling the door GameObject as a fallback.");
            gameObject.SetActive(false);
        }
    }

    public void CloseDoor()
    {
        if (doorAnimator != null)
        {
            // Trigger the door closing animation
            // doorAnimator.SetTrigger("Close"); // TODO: Uncomment this line when animation is ready
        }
        else
        {
            // Debug.LogWarning("No Animator component assigned to the door. Enabling the door GameObject as a fallback.");
            gameObject.SetActive(true); // TODO: Remove this line when animation is implemented
        }
    }
}