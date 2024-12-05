/// <summary>
/// Defines contextual conditions required for an item to be used.
/// </summary>

// TODO: Conceptual implementation of item conditions, to be expanded and discussed with the team.
public enum Condition
{
    /// <summary>
    /// No condition is required for the item to be used.
    /// </summary>
    None,

    /// <summary>
    /// The item can only be used during nighttime.
    /// </summary>
    Nighttime,

    /// <summary>
    /// The item can only be used within a specific zone.
    /// </summary>
    ZoneRestricted,

    /// <summary>
    /// The item can only be used after completing a specific puzzle.
    /// </summary>
    PuzzleComplete,

    /// <summary>
    /// The item can only be used when the player is near a target.
    /// </summary>
    InProximity,
    
    /// <summary>
    /// The item can only be used if the power has been turned back on.
    /// </summary>
    PowerOn
}