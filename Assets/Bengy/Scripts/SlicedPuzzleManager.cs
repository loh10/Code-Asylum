using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlicedPuzzleManager : MonoBehaviour
{

    [SerializeField] private Transform _gameTransform;
    [SerializeField] private Transform _piecePrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private Sprite[] _sprites;
    private List<Transform> _pieces;
    private int _emptyLocation;
    private int _size;
    private bool _shuffling;

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
                int currentPiece = (row * _size) + col;
                piece.name = currentPiece.ToString();
                // We want an empty space in the bottom right
                if((row == _size - 1) && (col == _size - 1))
                {
                    _emptyLocation = (_size * _size) - 1;
                    piece.gameObject.SetActive(false);
                }else

                {
                    // Set the sprite
                    piece.GetComponent<SpriteRenderer>().sprite = _sprites[currentPiece];
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    // On click send out ray to see if we click a piece
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.down, Mathf.Infinity);
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
                AudioManager.Instance.PlaySound(AudioType.slicedPuzzle, AudioSourceType.player);
            }
        }

        if (!_shuffling && CheckCompletion())
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
