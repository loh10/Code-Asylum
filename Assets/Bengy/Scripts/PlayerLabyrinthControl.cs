using TMPro;
using UnityEngine;
public class PlayerLabyrinthControl : MonoBehaviour
{
    public Camera cam;
    public Transform sphere;
    public float distanceFromCamera;
    [SerializeField] private GameObject _panelVictory;
    [SerializeField] private TextMeshProUGUI _textHint;
    [SerializeField] private GameObject _labyrinth;
    private Rigidbody _r;
    private bool _isDragging;

    private void Start()
    {
        distanceFromCamera = Vector3.Distance(sphere.position,cam.transform.position);
        _r = sphere.GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        ProcessInputs();
    }

    private void ProcessInputs()
    { 
        if(Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            pos.z = distanceFromCamera;
            pos = cam.ScreenToWorldPoint(pos);
            _r.linearVelocity = (pos - sphere.position) * 10;
            _isDragging = true;
        }
        else if (!Input.GetMouseButton(0) && _isDragging)
        {
            _isDragging = false;
            _r.linearVelocity = Vector3.zero;
        }

        if(Input.GetMouseButtonDown(0))
            AudioManager.Instance.PlaySound(AudioType.labyrinth, AudioSourceType.player);
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
    }
}
