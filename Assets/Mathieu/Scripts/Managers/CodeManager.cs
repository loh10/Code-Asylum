using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages entered codes. Stores which codes the player has successfully entered.
/// </summary>
public class CodeManager : MonoBehaviour
{
    public static CodeManager Instance { get; private set; }

    // Store entered codes
    private readonly HashSet<string> _enteredCodes = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Records that the player has successfully entered the given code.
    /// </summary>
    public void RecordCode(string code)
    {
        _enteredCodes.Add(code);
    }

    /// <summary>
    /// Checks if the given code has been entered by the player.
    /// </summary>
    public bool HasEnteredCode(string code)
    {
        return _enteredCodes.Contains(code);
    }
}