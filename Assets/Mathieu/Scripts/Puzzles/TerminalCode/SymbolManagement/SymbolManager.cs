using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages symbol assignment and retrieval of ItemConfigs for symbols.
/// </summary>
public class SymbolManager : MonoBehaviour
{
    public static SymbolManager Instance { get; private set; }

    [System.Serializable]
    public struct SymbolDefinition
    {
        public string symbolChar;    // e.g. "Φ"
        public int value;            // e.g. 12
        public ItemConfig symbolItemConfig; // The item representing this symbol
    }

    [Header("Symbol Definitions")]
    public SymbolDefinition[] allSymbols; // Assign in Inspector

    private Dictionary<string, SymbolDefinition> _symbolDictionary = new Dictionary<string, SymbolDefinition>();

    // We'll pick 3 random symbols for X, Y, Z puzzles
    private string _symbolX, _symbolY, _symbolZ;
    private bool _assigned = false;

    [Header("Puzzle IDs that reward symbols (X, Y, Z)")]
    public int puzzleID_X; 
    public int puzzleID_Y; 
    public int puzzleID_Z;

    // Map from puzzleID to the assigned symbol char (e.g. puzzleID_X -> symbolX)
    private Dictionary<int, string> _puzzleToSymbol = new Dictionary<int, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        AssignSymbols();
    }

    /// <summary>
    /// Assigns 3 random distinct symbols for the three puzzleIDs.
    /// </summary>
    private void AssignSymbols()
    {
        // Populate symbolDictionary
        foreach (var symDef in allSymbols)
        {
            _symbolDictionary[symDef.symbolChar] = symDef;
        }

        // Get a list of symbol chars
        List<string> keys = new List<string>(_symbolDictionary.Keys);

        // Shuffle keys to get random assignment
        for (int i = 0; i < keys.Count; i++)
        {
            int rand = Random.Range(i, keys.Count);
            (keys[i], keys[rand]) = (keys[rand], keys[i]);
        }

        // Assign the first three unique symbols to X, Y, Z
        // Assuming we have at least 3 symbols defined
        _symbolX = keys[0];
        _symbolY = keys[1];
        _symbolZ = keys[2];

        _puzzleToSymbol[puzzleID_X] = _symbolX;
        _puzzleToSymbol[puzzleID_Y] = _symbolY;
        _puzzleToSymbol[puzzleID_Z] = _symbolZ;

        _assigned = true;
        Debug.Log($"Symbols assigned: X={_symbolX}, Y={_symbolY}, Z={_symbolZ}");
    }

    public int GetSymbolValue(string symbol)
    {
        return _symbolDictionary[symbol].value;
    }

    /// <summary>
    /// Retrieves the ItemConfig for the symbol associated with a given puzzleID.
    /// </summary>
    public ItemConfig GetSymbolItemConfigForPuzzleID(int puzzleID)
    {
        if (!_assigned || !_puzzleToSymbol.ContainsKey(puzzleID))
            return null;

        string sym = _puzzleToSymbol[puzzleID];
        return _symbolDictionary[sym].symbolItemConfig;
    }

    // For equations (Terminal code) access:
    public string GetSymbolX() => _symbolX;
    public string GetSymbolY() => _symbolY;
    public string GetSymbolZ() => _symbolZ;
}
