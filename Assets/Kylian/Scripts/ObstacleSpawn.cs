using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleSpawn : MonoBehaviour
{
    private float _speed = 1;
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
    public GameObject Panel;
    [SerializeField]
    private Transform _obstacleParent;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
        Panel.SetActive(false);
    }

    IEnumerator SpawnEnemy()
    {
        if ( Random.Range(0,2) == 0)   // 0 = Down 
        {
            _position = _targetDown.position;

        }
        else                           // 1 = Up
        {
            _position = _targetUp.position;
        }

        if (_dino._tempNumberEnemyToSpawn >= 0)
        {
            GameObject _obstacle = Instantiate(obstaclePrefab, _position, Quaternion.identity, _obstacleParent);
            _obstacle.GetComponent<Obstacle>().dino = _dino.gameObject;
            _dino._tempNumberEnemyToSpawn--;
            float _spawnTime = Random.Range(_minTime, _maxTime);                             //random entre min time et max time 
            yield return new WaitForSeconds(_spawnTime);
            StartCoroutine(SpawnEnemy());
        }
        else
        {
            Debug.Log(_obstacleParent.childCount);
            if (_obstacleParent.childCount == 0)
            {
                Panel.SetActive(true);

            }
            yield return new WaitForSeconds(.5f);
            StartCoroutine(SpawnEnemy());
        }
    }

}
