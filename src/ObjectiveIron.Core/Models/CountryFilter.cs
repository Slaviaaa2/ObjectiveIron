namespace ObjectiveIron.Core.Models;

/// <summary>
/// Defines which country a focus tree or other entity applies to.
/// Maps to the Clausewitz country = { factor = 0  modifier = { add = N  tag = TAG } } pattern.
/// </summary>
public sealed class CountryFilter
{
    /// <summary>Base factor (usually 0 when using specific tag modifiers).</summary>
    public int BaseFactor { get; init; }

    /// <summary>List of country tag modifiers with their priority.</summary>
    public IReadOnlyList<CountryTagModifier> Modifiers { get; init; } = [];
}

public sealed record CountryTagModifier(string Tag, int Add = 10);
