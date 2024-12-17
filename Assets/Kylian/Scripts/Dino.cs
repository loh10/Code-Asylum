using UnityEngine;

public class Dino : MonoBehaviour
{
    
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private int numberEnemyToSpawn;
    [SerializeField] private Transform _targetUp, _targetDown;
    
    private int _tempNumberEnemyToSpawn;
    private bool _isUp;
    private bool _move;
    private Vector3 _endPosition;

    
    

    private void Start()
    {
        _tempNumberEnemyToSpawn = numberEnemyToSpawn;


    }


    private void Update()
    {
        if (_move)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPosition, _speed * Time.deltaTime);
            if ( Vector3.Distance(transform.position, _endPosition) < 0.01f)
            {
                _move = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)&& _move == false)
        {
            ChangePosition();
        }
    }
     
    private void ChangePosition()
    {
        if (_isUp)
        {
            _endPosition = _targetDown.position;
            _isUp = false;
        }
        else
        {
            _endPosition = _targetUp.position;
            _isUp = true;
        }
        _move = true;



        AudioManager.Instance.PlaySound(AudioType.dino, AudioSourceType.player);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Obstacle>(out Obstacle obstacle)) return;
        
        transform.position = _targetDown.position;
        _tempNumberEnemyToSpawn = numberEnemyToSpawn;
        Destroy(obstacle.gameObject);
        Debug.Log(_tempNumberEnemyToSpawn);
        _isUp = false;
    }
    
    public int GetNumberEnemyToSpawn() => _tempNumberEnemyToSpawn;
    public void SetNumberEnemyToSpawn(int number) => _tempNumberEnemyToSpawn = number;

}
