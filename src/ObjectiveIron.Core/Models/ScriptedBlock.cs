using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

/// <summary>
/// AST model for a named scripted trigger.
/// </summary>
public sealed class ScriptedTrigger
{
    public required string Id { get; init; }
    public required Block Body { get; init; }
}

/// <summary>
/// AST model for a named scripted effect.
/// </summary>
public sealed class ScriptedEffect
{
    public required string Id { get; init; }
    public required Block Body { get; init; }
}
