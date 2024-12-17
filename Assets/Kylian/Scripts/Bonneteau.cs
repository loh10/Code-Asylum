using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bonneteau puzzle: A "shell game" puzzle where the player must guess under which box the ball ends up after shuffling.
/// </summary>
public class Bonneteau : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Settings")]
    [SerializeField] private int _puzzleID;
    [SerializeField] private string _puzzleHint;
    [Tooltip("Number of shuffles before the player can guess.")]
    [SerializeField] private int _shuffleCount = 10;
    [Tooltip("Speed at which boxes move.")]
    [SerializeField] private float speed = 2f;

    [Header("Scene References")]
    [Tooltip("The three boxes (goblets) referenced in the inspector. Position them manually in the scene.")]
    [SerializeField] private Transform[] boxTransforms; // Exactly 3
    [Tooltip("The ball that will appear under one of the boxes.")]
    [SerializeField] private GameObject ball;

    [Header("Positions & Adjustments")]
    [Tooltip("Height at which boxes & ball start (raised to show ball).")]
    [SerializeField] private float raisedHeight = 1.5f;
    [Tooltip("Original ground level (the boxes will move down to this level when shuffling starts).")]
    [SerializeField] private float groundLevel = 0f;

    public bool IsSolved { get; set; }
    public string PuzzleHint { get; set; }
    public int PuzzleID { get; set; }

    private Camera _camera;
    private int _ballIndex;      // Which box currently has the ball
    private bool _isActive;      // Puzzle activated via Activate()
    private bool _isShuffling;   // Are we currently shuffling?
    private bool _canChoose;     // Can the player now choose a box after shuffling?
    private bool _showingResult; // Are we currently showing the result of the player's guess?

    private int _currentShuffleCount;
    private int _currentActiveBoxIndex;
    private int _nextIndex;
    private Vector3 _targetPos;
    private Vector3 _initPos;

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

        // Start with boxes raised
        foreach (Transform box in boxTransforms)
        {
            Vector3 pos = box.position;
            pos.y = raisedHeight; 
            box.position = pos;
        }

        // Ball randomly placed under one of the boxes
        _ballIndex = Random.Range(0, boxTransforms.Length);
        PlaceBallUnderBox(_ballIndex, raisedHeight);
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
                // All shuffles done, wait for player guess
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
            // Player must guess now
            if (Input.GetMouseButtonDown(0))
            {
                GuessBox();
            }
        }
        else
        {
            // Player not shuffling, not choosing, puzzle waiting to start
            if (Input.GetMouseButtonDown(0))
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
        // Debug.Log("Bonneteau puzzle activated. Player sees the ball and boxes raised.");
    }

    public void Solve()
    {
        if (IsSolved) return;
        IsSolved = true;
        PuzzleManager.Instance.SetPuzzleSolved(PuzzleID);
        // Debug.Log($"Puzzle {PuzzleID} solved! The player guessed correctly.");

        _isActive = false;
    }

    private void ResetPuzzle()
    {
        // Debug.Log("Player chose incorrectly. Resetting puzzle...");
        
        // Move boxes back up
        foreach (Transform box in boxTransforms)
        {
            Vector3 pos = box.position;
            pos.y = raisedHeight;
            box.position = pos;
        }

        // Randomize ball position again
        _ballIndex = Random.Range(0, boxTransforms.Length);
        PlaceBallUnderBox(_ballIndex, raisedHeight);
        ball.SetActive(true);

        // Reset states
        _isShuffling = false;
        _canChoose = false;
        _currentShuffleCount = 0;
    }

    private void PlaceBallUnderBox(int index, float yPos)
    {
        Vector3 boxPos = boxTransforms[index].position;
        Vector3 ballPos = ball.transform.position;
        ballPos.x = boxPos.x;
        ballPos.z = boxPos.z;
        ballPos.y = yPos - 0.5f;
        ball.transform.position = ballPos;
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
        // Hide the ball before lowering boxes so player can't track it visually
        ball.SetActive(false);

        float duration = 0.5f;
        float elapsed = 0f;
        Vector3[] startPositions = new Vector3[boxTransforms.Length];
        Vector3[] endPositions = new Vector3[boxTransforms.Length];
        for (int i = 0; i < boxTransforms.Length; i++)
        {
            startPositions[i] = boxTransforms[i].position;
            endPositions[i] = new Vector3(startPositions[i].x, groundLevel, startPositions[i].z);
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
        // Now start shuffling
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
        _currentActiveBoxIndex = _ballIndex; 
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
            if (_ballIndex == _currentActiveBoxIndex) _ballIndex = _nextIndex;
            else if (_ballIndex == _nextIndex) _ballIndex = _currentActiveBoxIndex;

            _currentShuffleCount--;
            if (_currentShuffleCount > 0)
            {
                SetupNextShuffle();
            }
            else
            {
                // done shuffling
                _isShuffling = false;
                _canChoose = true;
                Debug.Log("Shuffling done, choose a box!");
            }
        }
    }

    private void GuessBox()
    {
        if (!PlayerClickedOnBox(out int chosenIndex)) return;
        // Raise boxes and show result
        StartCoroutine(ShowResult(chosenIndex == _ballIndex));
    }

    private IEnumerator ShowResult(bool correct)
    {
        _showingResult = true; // Prevent player from guessing while showing result
        
        // Raise boxes up again to show the ball underneath the correct box
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3[] startPositions = new Vector3[boxTransforms.Length];
        Vector3[] endPositions = new Vector3[boxTransforms.Length];
        for (int i = 0; i < boxTransforms.Length; i++)
        {
            startPositions[i] = boxTransforms[i].position;
            endPositions[i] = new Vector3(startPositions[i].x, raisedHeight, startPositions[i].z);
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

        // Now place ball under the correct box at raised height and show it
        PlaceBallUnderBox(_ballIndex, raisedHeight);
        ball.SetActive(true);

        if (correct)
        {
            // Show success message
            string message = DialogueManager.GetDialogue("Enigma", "BonneteauFinished");
            DialogueMessageBoxUI.Instance.ShowMessage(message, 3f);

            // Solve the puzzle
            Solve();
            _showingResult = false;
        }
        else
        {
            // Show failure message
            string message = DialogueManager.GetDialogue("Enigma", "BonneteauFailed");
            DialogueMessageBoxUI.Instance.ShowMessage(message, 2.5f);
            
            // Player failed, after a small delay reset puzzle
            yield return new WaitForSeconds(2f);
            ResetPuzzle();
            _showingResult = false;
        }
    }
}
