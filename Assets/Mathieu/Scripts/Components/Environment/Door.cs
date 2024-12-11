using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Properties")]
    [SerializeField] private Animator doorAnimator; // TODO: Create a door opening animation

    private Lock _lockComponent;

    private void Awake()
    {
        _lockComponent = GetComponent<Lock>();
        if (_lockComponent != null)
        {
            _lockComponent.OnUnlock += OpenDoor;
        }
        else
        {
            //Debug.LogWarning("No Lock component found on the door.");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event
        if (_lockComponent != null)
        {
            _lockComponent.OnUnlock -= OpenDoor;
        }
    }
    
    private void OpenDoor()
    {
        //Debug.Log("Door is now open!");
        // Trigger the door opening animation
        if (doorAnimator != null)
        {
            doorAnimator.enabled = true;
        }
        else
        {
            gameObject.SetActive(false); // TODO: Remove this line when animation is ready
            //Debug.LogWarning("No Animator component assigned to the door.");
        }
    }
    
    public void CloseDoor()
    {
        if (doorAnimator != null)
        {
            // doorAnimator.SetTrigger("Close"); // TODO: Uncomment this line when animation is ready
        }
        else
        {
            //Debug.LogWarning("No Animator component assigned to the door.");
        }
    }
}