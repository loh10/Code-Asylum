using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages symbol assignment and retrieval of ItemConfigs for symbols.
/// </summary>
public class SymbolManager : MonoBehaviour
{
    public static SymbolManager Instance { get; private set; }

    // Predefined symbols and their values
    // We extend this to also store the ItemConfig for each symbol.
    [System.Serializable]
    public struct SymbolDefinition
    {
        public string symbolChar; // e.g. "Φ"
        public int value; // e.g. 12
        public ItemConfig symbolItemConfig; // The item representing this symbol
    }

    [Header("Symbol Definitions")]
    public SymbolDefinition[] allSymbols; // Assign in Inspector

    private Dictionary<string, SymbolDefinition> symbolDictionary = new Dictionary<string, SymbolDefinition>();

    // We'll pick 3 random symbols for X, Y, Z puzzles
    private string symbolX, symbolY, symbolZ;
    private bool assigned = false;

    // Suppose we know the puzzleIDs in advance:
    [Header("Puzzle IDs that reward symbols")]
    public string puzzleID_X; 
    public string puzzleID_Y; 
    public string puzzleID_Z;

    private Dictionary<string, string> puzzleToSymbol = new Dictionary<string, string>(); 
    // e.g. puzzleToSymbol[puzzleID_X] = symbolX

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

    private void Start()
    {
        AssignSymbols();
    }

    /// <summary>
    /// Assigns 3 random distinct symbols for the three puzzleIDs.
    /// </summary>
    private void AssignSymbols()
    {
        foreach (var symDef in allSymbols)
        {
            symbolDictionary[symDef.symbolChar] = symDef;
        }

        List<string> keys = new List<string>(symbolDictionary.Keys);
        // Shuffle keys
        for (int i = 0; i < keys.Count; i++)
        {
            int rand = Random.Range(i, keys.Count);
            (keys[i], keys[rand]) = (keys[rand], keys[i]);
        }

        symbolX = keys[0];
        symbolY = keys[1];
        symbolZ = keys[2];

        puzzleToSymbol[puzzleID_X] = symbolX;
        puzzleToSymbol[puzzleID_Y] = symbolY;
        puzzleToSymbol[puzzleID_Z] = symbolZ;

        assigned = true;
        Debug.Log($"Symbols assigned: X={symbolX}, Y={symbolY}, Z={symbolZ}");
    }

    public int GetSymbolValue(string symbol)
    {
        return symbolDictionary[symbol].value;
    }

    public ItemConfig GetSymbolItemConfigForPuzzleID(string puzzleID)
    {
        if (!assigned || !puzzleToSymbol.ContainsKey(puzzleID))
            return null;

        string sym = puzzleToSymbol[puzzleID];
        return symbolDictionary[sym].symbolItemConfig;
    }

    // For equations (Terminal code), we can also expose:
    public string GetSymbolX() => symbolX;
    public string GetSymbolY() => symbolY;
    public string GetSymbolZ() => symbolZ;
}
