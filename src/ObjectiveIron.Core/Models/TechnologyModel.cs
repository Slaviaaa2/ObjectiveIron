using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

/// <summary>
/// Represents a technology node in a HOI4 research tree.
/// </summary>
public record Technology(
    string Id,
    int? Year = null,
    double? Cost = null,
    string? Folder = null,
    TechnologyPosition? Position = null,
    IReadOnlyList<string>? Prerequisites = null,
    IReadOnlyList<EquipmentUnlock>? EquipmentUnlocks = null,
    IReadOnlyList<string>? EnableSubunits = null,
    IReadOnlyList<string>? EnableEquipmentModules = null,
    IReadOnlyList<TechnologyPath>? Paths = null,
    IReadOnlyList<string>? Categories = null,
    IReadOnlyList<UnitCategoryModifier>? UnitCategoryModifiers = null,
    Block? Allow = null,
    Block? AiWillDo = null,
    Block? Modifier = null
);

/// <summary>Position within a technology folder.</summary>
public record TechnologyPosition(int X, int Y);

/// <summary>Equipment unlocked by researching this technology.</summary>
public record EquipmentUnlock(string EquipmentId);

/// <summary>Defines a visible arrow path to the next technology.</summary>
public record TechnologyPath(string LeadsToTech, double ResearchCostCoeff = 1.0);

/// <summary>Technology folder definition for screen organization.</summary>
public record TechnologyFolder(string Id, int TabIndex);

/// <summary>Unit-category-specific modifier block (e.g., category_light_infantry = { soft_attack = 0.05 }).</summary>
public record UnitCategoryModifier(string Category, Block Modifiers);
