using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedPuzzleManager : MonoBehaviour
{

    [SerializeField] private Transform _gameTransform;
    [SerializeField] private Transform _piecePrefab;
    [SerializeField] private Camera _camera;

    private List<Transform> _pieces;
    private int _emptyLocation;
    private int _size;
    private bool _shuffling = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _pieces = new List<Transform>();
        _size = 3;
        CreateGamePieces(0.01f);
    }

    private void CreateGamePieces(float gapThickness)
    // This is the width of each tile
    {
        float width = 1 / (float)_size; 
        for(int row = 0; row < _size; row++)
        {
            for(int col = 0; col < _size; col++)
            {
                Transform piece = Instantiate(_piecePrefab, _gameTransform);
                _pieces.Add(piece);
                // Pieces will be in a game board going from -1 to +1
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                                  +1 - (2 * width * row) + width,
                                                  0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * _size) + col}";
                // We want an empty space in the bottom right
                if((row == _size - 1) && (col == _size - 1))
                {
                    _emptyLocation = (_size * _size) - 1;
                    piece.gameObject.SetActive(false);
                }else
                // We want to map the UV coordinates appropriately, they are 1 -> 0 
                {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    // UV coord order: (0, 1), (1, 1), (0, 0), (1, 0)
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1) - gap), 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
                    // Assign our new UVs to the mesh
                    mesh.uv = uv;
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    // On click send out ray to see if we click a piece
    {
        Debug.DrawRay(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.down * 100, Color.red);
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.down, Mathf.Infinity);
            print(hit.transform);
            if (hit)
            {
                for (int i = 0; i < _pieces.Count; i++)
                {
                    if (_pieces[i] == hit.transform)
                    {
                        // Check each direction to see if valid move
                        // We break out on success so we don't carry on and swap back again
                        if (SwapIfValid(i, -_size, _size)) { break; }
                        if (SwapIfValid(i, +_size, _size)) { break; }
                        if (SwapIfValid(i, -1, 0)) { break; }
                        if (SwapIfValid(i, +1, _size - 1)) { break; }
                    }
                }
            }
        }

        if(!_shuffling && CheckCompletion())
        {
            _shuffling = true;
            StartCoroutine(WaitShuffle(0.5f));
        }
    }

    // colCheck is used to stop horizontal moves wrapping
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % _size) != colCheck) && ((i + offset) == _emptyLocation))
        {
            // Swap them in game state
            (_pieces[i], _pieces[i + offset]) = (_pieces[i + offset], _pieces[i]);
            // Swap their transforms
            (_pieces[i].localPosition, _pieces[i + offset].localPosition) = (_pieces[i + offset].localPosition, _pieces[i].localPosition);
            // Update empty location
            _emptyLocation = i;
            return true;
        }
        return false;
    }

    private bool CheckCompletion()
    {
        for(int i = 0; i < _pieces.Count; i++)
        {
            if (_pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }


    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        _shuffling = false;
    }


    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (_size * _size * _size))
        {
            // Pick a random location
            int rnd = Random.Range(0, _size * _size);
            // Only thing we forbid is undoing the last move
            if(rnd == last) { continue; }
            last = _emptyLocation;
            // Try surrounding spaces looking for valid move
            if(SwapIfValid(rnd, -_size, _size))
            {
                count++;
            }else if(SwapIfValid(rnd, +_size, _size))
            {
                count++;
            }else if(SwapIfValid(rnd, -1, 0)) 
            {
                count++;
            }else if(SwapIfValid(rnd, +1, _size - 1))
            {
                count++;
            }       
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
}
