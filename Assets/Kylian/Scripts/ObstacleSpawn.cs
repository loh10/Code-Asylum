using System.Collections;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    [SerializeField]
    private Transform _targetUp, _targetDown;
    [SerializeField]
    private float _minTime, _maxTime;
    [SerializeField]
    private GameObject obstaclePrefab;
    [SerializeField]
    private Vector3 _position;
    [SerializeField]
    private Dino _dino;
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private Transform _obstacleParent;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
        Panel.SetActive(false);
    }

    private IEnumerator SpawnEnemy()
    {
        _position = Random.Range(0,2) == 0 ? _targetDown.position : _targetUp.position;
            // 0 = Down 
            // 1 = Up
            

        if (_dino.GetNumberEnemyToSpawn() >= 0)
        {
            GameObject _obstacle = Instantiate(obstaclePrefab, _position, Quaternion.identity, _obstacleParent);
            _obstacle.GetComponent<Obstacle>().dino = _dino.gameObject;
            _dino.SetNumberEnemyToSpawn(_dino.GetNumberEnemyToSpawn() - 1);
            float _spawnTime = Random.Range(_minTime, _maxTime);                             //random entre min time et max time 
            yield return new WaitForSeconds(_spawnTime);
        }
        else
        {
            Debug.Log(_obstacleParent.childCount);
            if (_obstacleParent.childCount == 0)
            {
                Panel.SetActive(true);
            }
            yield return new WaitForSeconds(.5f);
        }
        StartCoroutine(SpawnEnemy());
    }

}
