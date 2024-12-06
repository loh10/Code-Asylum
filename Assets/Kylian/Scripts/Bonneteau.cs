using System.Collections.Generic;
using UnityEngine;

public class Bonneteau : MonoBehaviour
{
    public float speed = 2;
    private bool _isPlaying;
    [SerializeField] private int _changeTime = 10;
    private int[] _position;
    [SerializeField] private int _size = 3;
    private int _indexCurrentPos = 0;
    [SerializeField] private GameObject _gobeletPrefab;
    private List<GameObject>_gobeletList = new List<GameObject>();
    int nextIndex = 0;

    private Vector3 _targetPos;
    private Vector3 _initPos;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {

            MoveGobelet();

        }
        if(_isPlaying && _changeTime >= 0) 
        {
            _gobeletList[_indexCurrentPos].transform.position = Vector3.MoveTowards(_gobeletList[_indexCurrentPos].transform.position, _targetPos, Time.deltaTime*speed);
            _gobeletList[nextIndex].transform.position = Vector3.MoveTowards(_gobeletList[nextIndex].transform.position, _initPos, Time.deltaTime * speed);
            if (Vector3.Distance(_gobeletList[_indexCurrentPos].transform.position,_targetPos) < 0.1f)
            {
                _gobeletList[_indexCurrentPos].transform.position = _targetPos;
                _gobeletList[nextIndex].transform.position = _initPos;
                _position[_indexCurrentPos] = 0;
                _position[nextIndex] = 1;
                _indexCurrentPos = nextIndex;
                MoveGobelet();
            }
        }
        if (_changeTime <= 0 && Input.GetMouseButtonDown(0))
        {
            ChooseGobelet();
        }


    }

    void MoveGobelet()
    {
        nextIndex = 0;
        while(nextIndex == _indexCurrentPos)
        {
            nextIndex = Random.Range(0, _size);
        }
        _targetPos = _gobeletList[nextIndex].transform.position;
        _initPos = _gobeletList[_indexCurrentPos].transform.position;
        _isPlaying = true;
        _changeTime --;

    }

    private void Init()
    {
        _position = new int[_size];
        _position[0] = 1;
        SpawnGobelet();

    }

    void SpawnGobelet()
    {
        for (int i = 0;i<_position.Length;i++)
        {
            GameObject newGobelet = Instantiate(_gobeletPrefab,new Vector3(i*2,0,0), Quaternion.identity);
            newGobelet.transform.parent = transform;
            newGobelet.name = "0";
            if(i ==0)
            {
                newGobelet.name = "1";
            }
            _gobeletList.Add(newGobelet);
            _isPlaying = true;
        }
    }

    void ChooseGobelet()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            if (hit.collider.name == 1.ToString())
            {
                Debug.Log("Win");  // to complete (Do)

            }
            else
            {
                Debug.Log("Loose");   // to complete (Do)
            }
        }
    }
}
