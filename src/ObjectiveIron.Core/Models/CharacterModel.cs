using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

/// <summary>
/// Unified Character system introduced in v1.11.
/// Emission: /common/characters/*.txt
/// </summary>
public sealed record Character(
    string Id,
    LocalizedText Name,
    IReadOnlyList<CharacterRole> Roles,
    CharacterPortraits? Portraits = null
);

public abstract record CharacterRole;

/// <summary>Country leader (rules the nation).</summary>
public sealed record CountryLeaderRole(
    string Ideology,
    IReadOnlyList<string> Traits,
    LocalizedText? Desc = null
) : CharacterRole;

/// <summary>Advisor (hired for Political Power).</summary>
public sealed record AdvisorRole(
    string Slot, // e.g., political_advisor, theorist
    IReadOnlyList<string> Traits,
    int Cost = 150,
    Block? Visible = null,
    Block? Available = null,
    Block? AiWillDo = null
) : CharacterRole;

/// <summary>Military commander (General or Field Marshal).</summary>
public sealed record CommanderRole(
    CommanderType Type,
    int Skill = 1,
    int Attack = 1,
    int Defense = 1,
    int Planning = 1,
    int Logistics = 1,
    IReadOnlyList<string> Traits = null
) : CharacterRole;

public enum CommanderType
{
    CorpsCommander,
    FieldMarshal,
    Admiral
}

public sealed record CharacterPortraits(
    string? ArmyLarge = null,
    string? ArmySmall = null,
    string? CivilianLarge = null,
    string? CivilianSmall = null
);
