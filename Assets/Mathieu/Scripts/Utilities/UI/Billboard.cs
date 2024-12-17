using UnityEngine;

/// <summary>
/// Makes the object this script is attached to always face the camera.
/// Useful for world-space UI elements like floating text or icons.
/// </summary>
public class Billboard : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Start()
    {
        // Find the main camera
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("No Main Camera found. Billboard script requires a camera to face.");
        }
    }

    private void LateUpdate()
    {
        if (_cameraTransform != null)
        {
            // Make the object face the camera
            transform.LookAt(transform.position + _cameraTransform.forward);
        }
    }
}