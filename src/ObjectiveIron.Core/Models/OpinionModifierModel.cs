namespace ObjectiveIron.Core.Models;

/// <summary>
/// Represents an opinion modifier that affects country relations.
/// </summary>
public record OpinionModifier(
    string Id,
    int Value,
    double? Decay = null,
    int? MinTrust = null,
    int? MaxTrust = null,
    int? Days = null,
    int? Months = null,
    int? Years = null,
    bool? Trade = null
);
