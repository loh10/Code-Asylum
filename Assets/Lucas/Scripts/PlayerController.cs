using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _inputDirection = Vector2.zero;
    private Vector2 _inputRotation = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _rotation = Vector2.zero;
    private const float _yRotationLimit = 88f;
    
    private Transform _transform;
    private Transform _camera;
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private PlayerInput _playerInput;
    private bool _freezeInput;
    
    [Range(0f, 1f)][SerializeField] private float _crouchMultiplier = 0.5f;
    [SerializeField] private float _speed = 750f;
    [SerializeField] private float _sensitivity = 20f;

    private void Start()
    {
        Cursor.visible = false;
        _transform = transform;
        _camera = Camera.main.transform;
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _playerInput = GetComponentInChildren<PlayerInput>();
    }
    private void Update()
    {
        if (_freezeInput) return;
        
        RotateCamera();
        Move();
    }

    public void GetInputPlayer(InputAction.CallbackContext ctx)
    {
        _inputDirection = ctx.ReadValue<Vector2>();
        
        if (ctx.canceled)
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y,0);
    }
    
    public void GetMouseDelta(InputAction.CallbackContext ctx)
    {
        _inputRotation = ctx.ReadValue<Vector2>() * _sensitivity;
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _capsuleCollider.height *= _crouchMultiplier;
            _capsuleCollider.center -= new Vector3(0, 1 - _crouchMultiplier, 0);
            _camera.position -= new Vector3(0, 1 - _crouchMultiplier, 0);
        }

        else if (ctx.canceled)
        {
            _capsuleCollider.height /= _crouchMultiplier;
            _capsuleCollider.center += new Vector3(0, 1 - _crouchMultiplier, 0);
            _camera.position += new Vector3(0, 1 - _crouchMultiplier, 0);
        }
    }
    
    private void RotateCamera()
    {
        _rotation.x += _inputRotation.x * Time.deltaTime;
        _rotation.y -= _inputRotation.y * Time.deltaTime;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);
        
        _transform.localEulerAngles = new Vector3(0, _rotation.x, 0);
        _camera.localEulerAngles = new Vector3(_rotation.y, 0, 0);
    }
    
    private void Move()
    {
        if (_rb == null || _inputDirection == Vector2.zero) return;

        float curSpeedX = _speed * _inputDirection.y * Time.deltaTime;
        float curSpeedY = _speed * _inputDirection.x * Time.deltaTime;

        _moveDirection = _transform.forward * curSpeedX + _transform.right * curSpeedY;

        _rb.linearVelocity = _moveDirection + new Vector3(0, _rb.linearVelocity.y,0);
    }
    
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.01f);
    }
    
    public void FreezeInput(bool value, int indexActionMap = 0)
    {
        _freezeInput = value;
        
        InputActionMap map = _playerInput.actions.actionMaps[indexActionMap];
        if (value)
        {
            map.Disable();
        }
        else
        {
            map.Enable();
        }
    }
    
    #region Get and Set
    public bool GetFreezeInput() => _freezeInput;
    public float GetSpeed() => _speed;
    public void SetSpeed(float value) => _speed = value;
    public void SetSensitivity(float value) => _sensitivity = value;
    #endregion
}
