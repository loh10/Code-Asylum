using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _inputDirection = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private Transform _camera;
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private bool _isCrouched;
    private GameObject _itemHolder;
    private SaveReload _saveReload;
    private Light flashingLight;
    
    public static bool freezeInput;
    
    [Range(0f, 1f)][SerializeField] private float _crouchMultiplier = 0.5f;
    [SerializeField] private float _walkSpeed = 750f;
    [SerializeField] private float _crouchSpeed = 300f;
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _rangeInteraction = 3f;
    [SerializeField] private InputActionAsset _actionAsset;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _saveReload = GetComponent<SaveReload>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _camera = Camera.main.transform;
        
        flashingLight = GetComponentInChildren<Light>();

        AudioManager.Instance.PlaySound(AudioType.atmosphere, AudioSourceType.player);
    }
    
    private void Update()
    {
        if (freezeInput) return;
        
        Movement();
        
        if (IsGrounded()) return;
        
        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity)) return;
    }

    public void GetInputPlayer(InputAction.CallbackContext ctx)
    {
        _inputDirection = ctx.ReadValue<Vector2>();
        
        if (ctx.canceled)
        {
            _rb.linearVelocity = Vector3.zero;
            AudioManager.Instance.StopSound(AudioType.walk,AudioSourceType.player);
        }
        else
        {

            AudioManager.Instance.PlaySound(AudioType.walk,AudioSourceType.player);
        }
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
            _capsuleCollider.center += new Vector3(0, 1 - _crouchMultiplier, 0);
            _capsuleCollider.height /= _crouchMultiplier;
            _camera.position += new Vector3(0, 1 - _crouchMultiplier, 0);
            _isCrouched = false;
        }
    }
    public void FlashLight(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;
        
        if (flashingLight != null)
        {
            flashingLight.enabled = !flashingLight.enabled;
        }
    }   
    
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled) return;

        if (!Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _rangeInteraction, 1 << LayerMask.NameToLayer("Interactable")))
            return;
        
        ICollectable collectable = hit.collider.GetComponent<ICollectable>();
        if (collectable != null && collectable.CanCollect)
        {
            collectable.OnCollect(gameObject);
            return;
        }

        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        if (interactable != null && interactable.IsInteractable)
        {
            interactable.Interact(gameObject);
        }
            
        IPuzzle puzzle = hit.collider.GetComponent<IPuzzle>();
        if (puzzle is { IsSolved: false } && !Cursor.visible)
        {
            puzzle.Activate();
        }
    }
    
    private void Movement()
    {
        if (_rb == null || _inputDirection == Vector2.zero) return;
        
        float speed = _isCrouched ? _crouchSpeed : _walkSpeed;
        float curSpeedX = speed * _inputDirection.y;
        float curSpeedY = speed * _inputDirection.x;

        _moveDirection = _orientation.forward * curSpeedX + _orientation.right * curSpeedY;

        _rb.linearVelocity = _moveDirection + new Vector3(0, _rb.linearVelocity.y, 0);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.04f);
    }
    
    public void FreezeInput(bool value, int indexActionMap = 0)
    {
        freezeInput = value;
        
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
        if (other.gameObject.layer != LayerMask.NameToLayer("AI"))
            return;
        
        freezeInput = true;
        _saveReload.IsDead();
    }
    

    #region Get and Set
    public float GetSpeed() => _walkSpeed;
    public void SetSpeed(float value) => _walkSpeed = value;
    #endregion
}
