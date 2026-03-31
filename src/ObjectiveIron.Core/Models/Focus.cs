using System.Collections.Generic;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

/// <summary>
/// Represents a single national focus node within a focus tree.
/// 
/// Clausewitz output example:
/// <code>
/// focus = {
///     id = GER_industrial_effort
///     icon = GFX_goal_generic_production
///     x = 10
///     y = 0
///     cost = 10
///     prerequisite = { focus = GER_rhineland }
///     mutually_exclusive = { focus = GER_alt_path }
///     available = { ... }
///     completion_reward = { ... }
/// }
/// </code>
/// </summary>
public sealed class Focus
{
    /// <summary>Unique identifier for this focus.</summary>
    public required string Id { get; init; }

    /// <summary>GFX sprite reference for the focus icon.</summary>
    public string? Icon { get; init; }

    /// <summary>X position in the focus tree grid.</summary>
    public int X { get; init; }

    /// <summary>Y position in the focus tree grid.</summary>
    public int Y { get; init; }

    /// <summary>Cost in weeks (each unit = 7 days). Default: 10 (70 days).</summary>
    public int Cost { get; init; } = 10;

    /// <summary>
    /// Relative position anchor. If set, X and Y are relative to this focus.
    /// Maps to relative_position_id in Clausewitz.
    /// </summary>
    public string? RelativePositionId { get; init; }

    /// <summary>
    /// Prerequisite groups. Each inner list is an OR-group; all groups must be satisfied (AND between groups).
    /// prerequisite = { focus = A focus = B }  → A OR B
    /// prerequisite = { focus = C }            → AND C
    /// </summary>
    public IReadOnlyList<IReadOnlyList<string>> Prerequisites { get; init; } = [];

    /// <summary>Mutually exclusive focuses (cannot both be completed).</summary>
    public IReadOnlyList<string> MutuallyExclusive { get; init; } = [];

    /// <summary>Trigger block: conditions for the focus to be available.</summary>
    public Block? Available { get; init; }

    /// <summary>Trigger block: conditions to auto-bypass the focus.</summary>
    public Block? Bypass { get; init; }

    /// <summary>Effect block: executed when the focus is completed.</summary>
    public Block? CompletionReward { get; init; }

    /// <summary>Effect block: executed when the focus is selected.</summary>
    public Block? SelectEffect { get; init; }

    /// <summary>Effect block: executed when the focus is cancelled.</summary>
    public Block? CancelEffect { get; init; }

    /// <summary>AI weight block for this focus.</summary>
    public Block? AiWillDo { get; init; }

    /// <summary>Whether to cancel the focus if conditions are no longer met.</summary>
    public bool CancelIfInvalid { get; init; } = false;

    /// <summary>Whether this focus can be taken even during a civil war.</summary>
    public bool AvailableIfCapitulated { get; init; } = false;

    /// <summary>Search filters for the focus tree search feature.</summary>
    public IReadOnlyList<string> SearchFilters { get; init; } = [];

    /// <summary>Whether this focus uses dynamic updates for icon/text (dynamic = yes).</summary>
    public bool Dynamic { get; init; } = false;

    /// <summary>Dynamic icons that evaluate triggers to override the base icon.</summary>
    public IReadOnlyList<DynamicIcon> DynamicIcons { get; init; } = [];
}

/// <summary>
/// Represents a dynamic icon block within a focus.
/// </summary>
public sealed class DynamicIcon
{
    /// <summary>The condition under which this icon should be shown. If null, acts as default.</summary>
    public Block? Trigger { get; init; }
    
    /// <summary>The GFX name to show.</summary>
    public required string Value { get; init; }
}
