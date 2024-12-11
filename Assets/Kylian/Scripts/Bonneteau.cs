using System.Collections.Generic;
using UnityEngine;

public class Bonneteau : MonoBehaviour
{
    public float speed = 2;
    
    [SerializeField] private int _changeTime = 10;
    [SerializeField] private int _size = 3;
    [SerializeField] private GameObject _gobletPrefab;
    
    private bool _isPlaying;
    private readonly List<GameObject> _gobletList = new List<GameObject>();
    private int[] _position;
    private int _indexCurrentPos;
    private int nextIndex;
    private Vector3 _targetPos;
    private Vector3 _initPos;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        Init();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 3f) && hit.transform.CompareTag("Goblet"))
            {
                _isPlaying = true;
                MoveGoblet();
            }
        }
        
        if (!_isPlaying) return;
        
        if(_changeTime >= 0) 
        {
            _gobletList[_indexCurrentPos].transform.position = Vector3.MoveTowards(_gobletList[_indexCurrentPos].transform.position, _targetPos, Time.deltaTime*speed);
            _gobletList[nextIndex].transform.position = Vector3.MoveTowards(_gobletList[nextIndex].transform.position, _initPos, Time.deltaTime * speed);
            if (Vector3.Distance(_gobletList[_indexCurrentPos].transform.position,_targetPos) < 0.1f)
            {
                _gobletList[_indexCurrentPos].transform.position = _targetPos;
                _gobletList[nextIndex].transform.position = _initPos;
                _position[_indexCurrentPos] = 0;
                _position[nextIndex] = 1;
                _indexCurrentPos = nextIndex;
                MoveGoblet();
            }
        }
        if (_changeTime <= 0 && Input.GetMouseButtonDown(0))
        {
            ChooseGoblet();
        }
    }

    private void MoveGoblet()
    {
        nextIndex = 0;
        while(nextIndex == _indexCurrentPos)
        {
            nextIndex = Random.Range(0, _size);
        }
        _targetPos = _gobletList[nextIndex].transform.position;
        _initPos = _gobletList[_indexCurrentPos].transform.position;
        _isPlaying = true;
        _changeTime --;
    }

    private void Init()
    {
        _position = new int[_size];
        _position[0] = 1;
        SpawnGoblet();

    }

    private void SpawnGoblet()
    {
        for (int i = 0;i<_position.Length;i++)
        {
            GameObject newGoblet = Instantiate(_gobletPrefab,transform.position + new Vector3(0, 0, i * 2), Quaternion.identity);
            newGoblet.transform.parent = transform;
            newGoblet.name = "0";
            if(i == 0)
            {
                newGoblet.name = "1";
            }
            _gobletList.Add(newGoblet);
        }
    }

    private void ChooseGoblet()
    {
        if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity)) return;
            
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
