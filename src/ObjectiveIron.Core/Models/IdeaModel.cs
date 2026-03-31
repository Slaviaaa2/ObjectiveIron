using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

/// <summary>
/// AST model for an idea (National Spirit, Law, etc.).
/// </summary>
public sealed class Idea
{
    public required string Id { get; init; }
    public LocalizedText? Name { get; init; }
    public LocalizedText? Description { get; init; }
    public string? Picture { get; init; }
    public string? Icon { get; init; }
    public Block? Modifier { get; init; }
    public Block? EquipmentBonus { get; init; }
    public Block? Rule { get; init; }
    
    // For sorting/categorization in emission
    public string Category { get; init; } = "national_spirit";

    /// <summary>Dynamic title scripted loc name.</summary>
    public string? DynamicName { get; init; }

    /// <summary>Dynamic description scripted loc name.</summary>
    public string? DynamicDescription { get; init; }

    public Block? Visible { get; init; }
    public Block? Available { get; init; }
}

/// <summary>
/// AST model for an idea category.
/// </summary>
public sealed class IdeaCategory
{
    public required string Id { get; init; }
    public List<Idea> Ideas { get; init; } = [];
}
