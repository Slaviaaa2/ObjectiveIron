using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

/// <summary>
/// Represents an on_action hook (e.g., on_startup, on_war_declaration).
/// Each hook has an optional effect block and optional random_events list.
/// </summary>
public record OnAction(
    string Hook,
    Block? Effect = null,
    IReadOnlyList<RandomEvent>? RandomEvents = null
);

/// <summary>
/// Weighted random event entry: weight = event_id
/// </summary>
public record RandomEvent(int Weight, string EventId);
