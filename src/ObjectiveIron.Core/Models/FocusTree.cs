namespace ObjectiveIron.Core.Models;

/// <summary>
/// Represents a complete national focus tree definition.
/// Each focus tree is tied to one or more countries and contains an ordered collection of focuses.
/// 
/// Clausewitz output:
/// <code>
/// focus_tree = {
///     id = my_focus_tree
///     country = { ... }
///     default = no
///     focus = { ... }
///     focus = { ... }
/// }
/// </code>
/// </summary>
public sealed class FocusTree
{
    /// <summary>Unique identifier for this focus tree.</summary>
    public required string Id { get; init; }

    /// <summary>Country filter determining which countries use this tree.</summary>
    public required CountryFilter Country { get; init; }

    /// <summary>Whether this is the default focus tree (usually no for mods).</summary>
    public bool IsDefault { get; init; } = false;

    /// <summary>All focus nodes in this tree.</summary>
    public IReadOnlyList<Focus> Focuses { get; init; } = [];

    /// <summary>Shared focus references (focus IDs from shared_focus definitions).</summary>
    public IReadOnlyList<string> SharedFocuses { get; init; } = [];

    /// <summary>Whether continuous focus is enabled for this tree.</summary>
    public bool ContinuousFocusHasCountryTreeOverride { get; init; } = false;

    /// <summary>Initial show position for the tree viewport.</summary>
    public (int X, int Y)? InitialShowPosition { get; init; }
}
