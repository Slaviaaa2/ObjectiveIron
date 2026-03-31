using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

/// <summary>
/// AST model for a single HoI4 decision.
/// </summary>
public sealed class Decision
{
    public required string Id { get; init; }
    
    /// <summary>Localization key for the name.</summary>
    public required string NameIdentifier { get; init; }
    
    /// <summary>GFX sprite key for the icon.</summary>
    public string? Icon { get; init; }
    
    /// <summary>Political power cost.</summary>
    public int? Cost { get; init; }
    
    public bool? IsVisible { get; init; }

    /// <summary>Trigger block for when the decision is visible.</summary>
    public Block? Visible { get; init; }

    /// <summary>Trigger block for when the decision is selectable.</summary>
    public Block? Available { get; init; }

    /// <summary>Effect block when clicking the button.</summary>
    public Block? CompleteEffect { get; init; }

    /// <summary>Effect block if the decision times out.</summary>
    public Block? TimeoutEffect { get; init; }

    /// <summary>Effect block when the decision is removed (e.g. by expiration).</summary>
    public Block? RemoveEffect { get; init; }

    /// <summary>AI weighting.</summary>
    public Block? AiWillDo { get; init; }
    
    /// <summary>Days until removal/timeout.</summary>
    public int? DaysToRemove { get; init; }

    /// <summary>Modifier block active during decision.</summary>
    public Block? Modifier { get; init; }

    /// <summary>Dynamic title scripted loc name.</summary>
    public string? DynamicName { get; init; }

    /// <summary>Dynamic description scripted loc name.</summary>
    public string? DynamicDescription { get; init; }
}

/// <summary>
/// AST model for a decision category.
/// </summary>
public sealed class DecisionCategory
{
    public required string Id { get; init; }
    
    /// <summary>Localization key for the name.</summary>
    public required string NameIdentifier { get; init; }

    public string? Icon { get; init; }
    
    public Block? Visible { get; init; }
    public Block? Available { get; init; }

    /// <summary>List of decisions assigned to this category.</summary>
    public List<Decision> Decisions { get; init; } = [];
}
