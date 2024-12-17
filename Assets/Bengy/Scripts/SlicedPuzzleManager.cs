using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedPuzzleManager : MonoBehaviour
{
    [Header("MiniGameManager Reference")]
    [SerializeField] private MiniGameManager miniGameManager;
    
    [Header("Puzzle Setup")]
    [SerializeField] private Transform _gameTransform;
    [SerializeField] private Transform _piecePrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private Sprite[] _sprites;

    private List<Transform> _pieces;
    private int _emptyLocation;
    private int _size;
    private bool _solved; // Track if the puzzle is solved

    private void Start()
    {
        _pieces = new List<Transform>();
        _size = 3;
        CreateGamePieces(0.01f);

        // Optional: Shuffle at start so puzzle isn't already solved
        Shuffle();
    }

    private void CreateGamePieces(float gapThickness)
    {
        float width = 1f / _size; 
        for(int row = 0; row < _size; row++)
        {
            for(int col = 0; col < _size; col++)
            {
                Transform piece = Instantiate(_piecePrefab, _gameTransform);
                _pieces.Add(piece);
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                                  +1 - (2 * width * row) + width,
                                                  0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                int currentPiece = (row * _size) + col;
                piece.name = currentPiece.ToString();
                // The last piece is the empty spot
                if ((row == _size - 1) && (col == _size - 1))
                {
                    _emptyLocation = (_size * _size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    piece.GetComponent<SpriteRenderer>().sprite = _sprites[currentPiece];
                }
            }
        }
    }

    private void Update()
    {
        if (_solved) return; // If already solved, no need to do anything

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.down, Mathf.Infinity);
            if (hit)
            {
                for (int i = 0; i < _pieces.Count; i++)
                {
                    if (_pieces[i] == hit.transform)
                    {
                        // Attempt moves
                        if (SwapIfValid(i, -_size, _size)) { break; }
                        if (SwapIfValid(i, +_size, _size)) { break; }
                        if (SwapIfValid(i, -1, 0)) { break; }
                        if (SwapIfValid(i, +1, _size - 1)) { break; }
                    }
                }
                AudioManager.Instance.PlaySound(AudioType.slicedPuzzle, AudioSourceType.player);

                // Check after a move if completed
                if (CheckCompletion())
                {
                    SolvePuzzle();
                }
            }
        }
    }

    // colCheck is used to stop horizontal moves wrapping
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % _size) != colCheck) && ((i + offset) == _emptyLocation))
        {
            (_pieces[i], _pieces[i + offset]) = (_pieces[i + offset], _pieces[i]);
            (_pieces[i].localPosition, _pieces[i + offset].localPosition) = 
                (_pieces[i + offset].localPosition, _pieces[i].localPosition);
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

    private void SolvePuzzle()
    {
        _solved = true;
        // Call the MiniGameManager to solve the puzzle
        if (miniGameManager != null)
        {
            miniGameManager.Solve();
        }
        else
        {
            Debug.LogWarning("MiniGameManager reference not set on SlicedPuzzleManager.");
        }
    }

    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        // Shuffle puzzle at the start so it's not already solved
        while (count < (_size * _size * _size))
        {
            int rnd = Random.Range(0, _size * _size);
            if (rnd == last) { continue; }
            last = _emptyLocation;

            if (SwapIfValid(rnd, -_size, _size))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +_size, _size))
            {
                count++;
            }
            else if (SwapIfValid(rnd, -1, 0))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +1, _size - 1))
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
