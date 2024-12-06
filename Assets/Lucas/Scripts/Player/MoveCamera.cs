using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform _cameraPosition;
    private void Update()
    {
        transform.position = _cameraPosition.position;
    }
}
