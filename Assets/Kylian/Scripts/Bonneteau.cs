using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonneteau : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Settings")]
    [SerializeField] private int _puzzleID;
    [SerializeField] private string _puzzleHint;
    [SerializeField] private int _shuffleCount = 10;
    [SerializeField] private float speed = 2f;

    [Header("Scene References")]
    [SerializeField] private Transform[] boxTransforms; // Exactly 3
    [SerializeField] private GameObject ball;

    [Header("Positions & Adjustments")]
    [Tooltip("How much higher than their initial position the boxes start. For example, if you put 1.5, that means boxes start 1.5 units higher than their original Y.")]
    [SerializeField] private float raiseOffset = 1.5f;

    [Tooltip("How much lower than their initial position the boxes go for the shuffle. For example, 0 might mean going back to original level, or a negative number to go below original level.")]
    [SerializeField] private float lowerOffset = 0f;

    public bool IsSolved { get; set; }
    public string PuzzleHint { get; set; }
    public int PuzzleID { get; set; }

    private Camera _camera;
    private int _ballIndex;
    private bool _isActive;
    private bool _isShuffling;
    private bool _canChoose;
    private bool _showingResult;

    private int _currentShuffleCount;
    private int _currentActiveBoxIndex;
    private int _nextIndex;
    private Vector3 _targetPos;
    private Vector3 _initPos;

    // We'll store the initial world positions of each box
    private Vector3[] initialPositions;

    private void Awake()
    {
        PuzzleID = _puzzleID;
        PuzzleHint = _puzzleHint;

        if (boxTransforms.Length != 3)
        {
            Debug.LogError("Bonneteau requires exactly 3 boxes assigned.");
        }
    }

    private void Start()
    {
        _camera = Camera.main;

        // Store initial positions
        initialPositions = new Vector3[boxTransforms.Length];
        for (int i = 0; i < boxTransforms.Length; i++)
        {
            initialPositions[i] = boxTransforms[i].position;
        }

        // Raise boxes by raiseOffset from their initial position
        for (int i = 0; i < boxTransforms.Length; i++)
        {
            Vector3 pos = initialPositions[i];
            pos.y += raiseOffset;
            boxTransforms[i].position = pos;
        }

        // Ball randomly placed under one of the boxes
        _ballIndex = Random.Range(0, boxTransforms.Length);
        PlaceBallUnderBox(_ballIndex);
        ball.SetActive(true);
    }

    private void Update()
    {
        if (!_isActive || IsSolved || _showingResult) return;

        if (_isShuffling)
        {
            if (_currentShuffleCount > 0)
            {
                MoveBoxesDuringShuffle();
            }
            else
            {
                if (!_canChoose)
                {
                    _canChoose = true;
                    _isShuffling = false;
                    Debug.Log("Shuffling complete! Choose a box.");
                }
            }
        }
        else if (_canChoose)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GuessBox();
            }
        }
        else
        {
            // Puzzle not started shuffling yet
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (PlayerClickedOnBox(out int clickedIndex))
                {
                    StartCoroutine(LowerBoxesAndStartShuffle());
                }
            }
        }
    }

    public void Activate()
    {
        if (IsSolved) return;
        _isActive = true;
    }

    public void Solve()
    {
        if (IsSolved) return;
        IsSolved = true;
        PuzzleManager.Instance.SetPuzzleSolved(PuzzleID);
        _isActive = false;
    }

    private void ResetPuzzle()
    {
        // Move boxes back up to initialPositions + raiseOffset
        for (int i = 0; i < boxTransforms.Length; i++)
        {
            Vector3 pos = initialPositions[i];
            pos.y += raiseOffset;
            boxTransforms[i].position = pos;
        }

        // Randomize ball position again
        _ballIndex = Random.Range(0, boxTransforms.Length);
        PlaceBallUnderBox(_ballIndex);
        ball.SetActive(true);

        _isShuffling = false;
        _canChoose = false;
        _currentShuffleCount = 0;
    }

    private void PlaceBallUnderBox(int index)
    {
        Vector3 boxPos = boxTransforms[index].position;
        Vector3 ballPos = ball.transform.position;
        ballPos.x = boxPos.x-0.2f;
        ballPos.z = boxPos.z+0.2f;
        ball.transform.position = ballPos;
        ball.transform.SetParent(boxTransforms[index]);
        _ballIndex = index;
    }

    private bool PlayerClickedOnBox(out int boxIndex)
    {
        boxIndex = -1;
        if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity)) return false;
        for (int i = 0; i < boxTransforms.Length; i++)
        {
            if (hit.collider.transform == boxTransforms[i])
            {
                boxIndex = i;
                return true;
            }
        }
        return false;
    }

    private IEnumerator LowerBoxesAndStartShuffle()
    {
        // Hide the ball
        ball.SetActive(false);

        float duration = 0.5f;
        float elapsed = 0f;
        Vector3[] startPositions = new Vector3[boxTransforms.Length];
        Vector3[] endPositions = new Vector3[boxTransforms.Length];
        for (int i = 0; i < boxTransforms.Length; i++)
        {
            startPositions[i] = boxTransforms[i].position;
            // Lower to initialPositions[i].y + lowerOffset
            Vector3 endPos = initialPositions[i];
            endPos.y += lowerOffset;
            endPositions[i] = endPos;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            for (int i = 0; i < boxTransforms.Length; i++)
            {
                boxTransforms[i].position = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }
            yield return null;
        }

        StartShuffle();
    }

    private void StartShuffle()
    {
        _isShuffling = true;
        _canChoose = false;
        _currentShuffleCount = _shuffleCount;
        Debug.Log("Shuffling started...");
        SetupNextShuffle();
    }

    private void SetupNextShuffle()
    {
        _currentActiveBoxIndex = Random.Range(0, boxTransforms.Length);
        _nextIndex = _currentActiveBoxIndex;
        while (_nextIndex == _currentActiveBoxIndex)
        {
            _nextIndex = Random.Range(0, boxTransforms.Length);
        }

        _targetPos = boxTransforms[_nextIndex].position;
        _initPos = boxTransforms[_currentActiveBoxIndex].position;
    }

    private void MoveBoxesDuringShuffle()
    {
        boxTransforms[_currentActiveBoxIndex].position = Vector3.MoveTowards(
            boxTransforms[_currentActiveBoxIndex].position, _targetPos, Time.deltaTime * speed);

        boxTransforms[_nextIndex].position = Vector3.MoveTowards(
            boxTransforms[_nextIndex].position, _initPos, Time.deltaTime * speed);

        if (Vector3.Distance(boxTransforms[_currentActiveBoxIndex].position, _targetPos) < 0.1f)
        {
            // Swap completed
            Vector3 finalPosA = _targetPos;
            Vector3 finalPosB = _initPos;
            boxTransforms[_currentActiveBoxIndex].position = finalPosA;
            boxTransforms[_nextIndex].position = finalPosB;

            // Update ball index
            // if (_ballIndex == _currentActiveBoxIndex) _ballIndex = _nextIndex;
            // else if (_ballIndex == _nextIndex) _ballIndex = _currentActiveBoxIndex;

            _currentShuffleCount--;
            if (_currentShuffleCount > 0)
            {
                SetupNextShuffle();
            }
            else
            {
                _isShuffling = false;
                _canChoose = true;
                Debug.Log("Shuffling done, choose a box!");
            }
        }
    }

    private void GuessBox()
    {
        if (!PlayerClickedOnBox(out int chosenIndex)) return;
        StartCoroutine(ShowResult(chosenIndex == _ballIndex));
    }

    private IEnumerator ShowResult(bool correct)
    {
        _showingResult = true;

        // Raise boxes up again (to initialPositions + raiseOffset)
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3[] startPositions = new Vector3[boxTransforms.Length];
        Vector3[] endPositions = new Vector3[boxTransforms.Length];

        for (int i = 0; i < boxTransforms.Length; i++)
        {
            startPositions[i] = boxTransforms[i].position;
            Vector3 endPos = startPositions[i];
            endPos.y += raiseOffset;
            endPositions[i] = endPos;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            for (int i = 0; i < boxTransforms.Length; i++)
            {
                boxTransforms[i].position = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }
            yield return null;
        }

        // Place ball under correct box at raised height
        PlaceBallUnderBox(_ballIndex);
        ball.SetActive(true);

        if (correct)
        {
            string message = DialogueManager.GetDialogue("Enigma", "BonneteauFinished");
            if (!string.IsNullOrEmpty(message))
            {
                DialogueMessageBoxUI.Instance.ShowMessage(message, 3f);
            }
            Solve();
        }
        else
        {
            string message = DialogueManager.GetDialogue("Enigma", "BonneteauFailed");
            if (!string.IsNullOrEmpty(message))
            {
                DialogueMessageBoxUI.Instance.ShowMessage(message, 2.5f);
            }

            yield return new WaitForSeconds(2f);
            ResetPuzzle();
        }

        _showingResult = false;
    }
}
