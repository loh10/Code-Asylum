﻿/// <summary>
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
    /// A clue item that provides narrative or informational content.
    /// </summary>
    Clue,

    /// <summary>
    /// Miscellaneous items.
    /// </summary>
    Generic
}