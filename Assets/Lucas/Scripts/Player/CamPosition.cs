using UnityEngine;
using UnityEngine.InputSystem;

public class CamPosition : MonoBehaviour
{
    
    private Vector2 _inputRotation = Vector2.zero;
    private float _xRotation;
    private float _yRotation;
    private const float _yRotationLimit = 88f;
    private Transform _transform;
    
    [SerializeField] private float _sensitivity = 5f;
    [SerializeField] private Transform _orientation;

    private void Start()
    {
        _transform = transform;
    }
    private void Update()
    {
        if (PlayerController.freezeInput) return;
        
        RotateCamera();
    }
    public void GetMouseDelta(InputAction.CallbackContext ctx)
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
            _sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        
        _inputRotation = ctx.ReadValue<Vector2>();
    }
    private void RotateCamera()
    {
        float mouseX = _inputRotation.x * _sensitivity * Time.deltaTime;
        float mouseY = _inputRotation.y * _sensitivity * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -_yRotationLimit, _yRotationLimit);
        
        _transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
