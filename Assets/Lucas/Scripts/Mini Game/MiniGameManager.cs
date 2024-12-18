using System;
using UnityEngine;

public class MiniGameManager : MonoBehaviour, IPuzzle
{
    [SerializeField] private GameObject _miniGame;
    [SerializeField] private string _puzzleHint;
    [SerializeField] private int _puzzleID;
    public bool IsSolved { get; set; }
    public string PuzzleHint { get; set; }
    public int PuzzleID { get; set; }
    
    public static GameObject currentMiniGame;

    private void Awake()
    {
        PuzzleID = _puzzleID;
        PuzzleHint = _puzzleHint;
    }
    public void Activate()
    {
        if (IsSolved) return;
        
        _miniGame.SetActive(true);
        currentMiniGame = _miniGame;
    }
    public void Solve()
    {
        _miniGame.SetActive(false);
        
        IsSolved = true;
        PuzzleManager.Instance.SetPuzzleSolved(PuzzleID);
        AudioManager.Instance.PlaySound(AudioType.button);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
