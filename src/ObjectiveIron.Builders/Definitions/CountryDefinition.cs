using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines a new country with its tag, color, and graphical culture.
/// Emits to: common/country_tags/ and common/countries/
/// </summary>
public abstract class CountryDefinition
{
    /// <summary>3-letter country tag (e.g., "GER", "SOV").</summary>
    public abstract string Tag { get; }

    public virtual LocalizedText? Name => null;

    /// <summary>Map color as RGB tuple.</summary>
    public abstract (int R, int G, int B) Color { get; }

    /// <summary>Optional UI color (brighter variant).</summary>
    public virtual (int R, int G, int B)? ColorUi => null;

    /// <summary>Graphical culture for unit models (e.g., "western_european_gfx").</summary>
    public virtual string GraphicalCulture => "western_european_gfx";

    /// <summary>Secondary graphical culture.</summary>
    public virtual string? GraphicalCulture2d => null;

    /// <summary>Flag color names for procedural flags (e.g., "blue", "white", "red").</summary>
    public virtual string[]? FlagColors => null;

    /// <summary>Cosmetic tags for alternate name/flag variants.</summary>
    public virtual string[]? CosmeticTags => null;
}
