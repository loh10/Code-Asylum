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
    
    private bool _freezeInput;
    private bool _isCrouched;
    
    private GameObject _itemHolder;
    
    [Range(0f, 1f)][SerializeField] private float _crouchMultiplier = 0.5f;
    [SerializeField] private float _walkSpeed = 750f;
    [SerializeField] private float _crouchSpeed = 300f;
    [SerializeField] private float _sensitivity = 20f;
    [SerializeField] private float _rangeInteraction = 3f;
    [SerializeField] private InputActionAsset _actionAsset;

    //public Inventory _inventory;

    private void Start()
    {
        _transform = transform;
        _camera = Camera.main.transform;
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        
        if (PlayerPrefs.HasKey("Sensitivity"))
            _sensitivity = PlayerPrefs.GetFloat("Sensitivity");
    }
    
    private void Update()
    {
        if (_freezeInput) return;
        
        RotateCamera();
        Movement();
    }

    public void GetInputPlayer(InputAction.CallbackContext ctx)
    {
        _inputDirection = ctx.ReadValue<Vector2>();
        
        if (ctx.canceled)
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y,0);
    }
    
    public void GetMouseDelta(InputAction.CallbackContext ctx)
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
            _sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        
        _inputRotation = ctx.ReadValue<Vector2>() * _sensitivity;
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _capsuleCollider.height *= _crouchMultiplier;
            _capsuleCollider.center -= new Vector3(0, 1 - _crouchMultiplier, 0);
            _camera.position -= new Vector3(0, 1 - _crouchMultiplier, 0);
            _isCrouched = true;
        }

        else if (ctx.canceled)
        {
            _capsuleCollider.height /= _crouchMultiplier;
            _capsuleCollider.center += new Vector3(0, 1 - _crouchMultiplier, 0);
            _camera.position += new Vector3(0, 1 - _crouchMultiplier, 0);
            _isCrouched = false;
        }
    }
    
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled) return;

        if (!Physics.Raycast(_camera.position, _camera.forward, 
            out RaycastHit hit, _rangeInteraction, 1 << LayerMask.NameToLayer("Interactable"))) return;

        // Lock locker = hit.collider.GetComponent<Lock>();
        // if (locker == null) return;
        //
        // locker.Interact(this);
    }
    
    private void RotateCamera()
    {
        _rotation.x += _inputRotation.x * Time.deltaTime;
        _rotation.y -= _inputRotation.y * Time.deltaTime;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);
        
        _transform.localEulerAngles = new Vector3(0, _rotation.x, 0);
        _camera.localEulerAngles = new Vector3(_rotation.y, 0, 0);
    }
    
    private void Movement()
    {
        if (_rb == null || _inputDirection == Vector2.zero) return;
        
        float speed = _isCrouched ? _crouchSpeed : _walkSpeed;
        float curSpeedX = speed * _inputDirection.y * Time.deltaTime;
        float curSpeedY = speed * _inputDirection.x * Time.deltaTime;

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
        
        InputActionMap map = _actionAsset.actionMaps[indexActionMap];
        if (value)
        {
            map.Disable();
        }
        else
        {
            map.Enable();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("AI")) return;
        
        //Dead
    }
    

    #region Get and Set
    public bool GetFreezeInput() => _freezeInput;
    public float GetSpeed() => _walkSpeed;
    public void SetSpeed(float value) => _walkSpeed = value;
    public void SetSensitivity(float value) => _sensitivity = value;
    #endregion
}
