using System.Collections;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    [SerializeField] private Transform _targetUp, _targetDown;
    [SerializeField] private float _minTime, _maxTime;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Dino _dino;
    [SerializeField] private Transform _obstacleParent;

    // Add a reference to the MiniGameManager
    [Header("Puzzle Integration")]
    [SerializeField] private MiniGameManager _miniGame;

    private void OnEnable()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        _position = Random.Range(0,2) == 0 ? _targetDown.position : _targetUp.position;
        // 0 = Down 
        // 1 = Up

        if (_dino.GetNumberEnemyToSpawn() >= 0)
        {
            // Still have enemies to spawn
            GameObject _obstacle = Instantiate(obstaclePrefab, _position, Quaternion.identity, _obstacleParent);
            _obstacle.GetComponent<Obstacle>().dino = _dino.gameObject;
            _dino.SetNumberEnemyToSpawn(_dino.GetNumberEnemyToSpawn() - 1);
            float _spawnTime = Random.Range(_minTime, _maxTime);
            yield return new WaitForSeconds(_spawnTime);
        }
        else
        {
            // No more enemies to spawn, check if obstacles left
            if (_obstacleParent.childCount == 0)
            {
                // Puzzle conditions met: no more spawning and no obstacles remaining
                // Solve the puzzle via MiniGameManager
                if (_miniGame != null && !_miniGame.IsSolved)
                {
                    _miniGame.Solve();
                }
                
                // // Show panel after solving the puzzle
                // Panel.SetActive(true);
                // Cursor.lockState = CursorLockMode.None;
                // Cursor.visible = true;

            }
            yield return new WaitForSeconds(.5f);
        }
        StartCoroutine(SpawnEnemy());
    }
}
