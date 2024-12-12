/// <summary>
/// Defines the functional role of an item in the game.
/// </summary>
public enum ItemType
{
    /// <summary>
    /// Used to unlock objects.
    /// </summary>
    Key,

    /// <summary>
    /// Used to interact with the environment.
    /// </summary>
    Tool,

    /// <summary>
    /// Provides a one-time effect.
    /// </summary>
    Consumable,

    /// <summary>
    /// Symbols required to solve the Terminal Code puzzle.
    /// </summary>
    Symbol,

    /// <summary>
    /// Miscellaneous items.
    /// </summary>
    Generic
}