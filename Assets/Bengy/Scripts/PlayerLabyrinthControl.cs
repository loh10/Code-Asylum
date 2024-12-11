using TMPro;
using UnityEngine;
public class PlayerLabyrinthControl : MonoBehaviour, IPuzzle
{
    public Camera cam;
    public Transform sphere;
    public float distanceFromCamera;
    [SerializeField] private GameObject _panelVictory;
    [SerializeField] private TextMeshProUGUI _textHint;
    [SerializeField] private GameObject _labyrinth;
    private Rigidbody _r;
    private bool _isDragging;
    
    public bool IsSolved { get; set; }
    public string PuzzleHint { get; set; } = "You won a crowbar";
    public int PuzzleID { get; }

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
    
    public void Activate()
    {
        
    }
    public void Solve()
    {
        _panelVictory.SetActive(true);
        _labyrinth.layer = LayerMask.NameToLayer("Default");
        IsSolved = true;
        _textHint.text = PuzzleHint;
    }
}
