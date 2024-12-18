using System;
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
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && _move == false)
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



        AudioManager.Instance.PlaySound(AudioType.dino);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Obstacle>(out Obstacle obstacle)) return;
        
        ResetGame();
        Destroy(obstacle.gameObject);
    }
    
    public int GetNumberEnemyToSpawn() => _tempNumberEnemyToSpawn;
    public void SetNumberEnemyToSpawn(int number) => _tempNumberEnemyToSpawn = number;

    private void OnDisable()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        transform.position = _targetDown.position;
        _tempNumberEnemyToSpawn = numberEnemyToSpawn;
        _isUp = false;
    }
}
