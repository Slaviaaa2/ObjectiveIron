using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// A unified HoI4 character (v1.11+).
/// Emission: /common/characters/*.txt
/// </summary>
public abstract class CharacterDefinition
{
    public abstract string Id { get; }
    public abstract LocalizedText Name { get; }

    /// <summary>
    /// Icon/Portrait (e.g., GFX_portrait_mikhail_large).
    /// If left null, uses generic portraits.
    /// </summary>
    public virtual string? PortraitLarge { get; }
    public virtual string? PortraitSmall { get; }

    /// <summary>
    /// Define roles for this character (country_leader, advisor, etc.).
    /// </summary>
    protected virtual void DefineRoles(RoleScope r) { }

    internal IReadOnlyList<CharacterRole> CompileRoles()
    {
        var scope = new RoleScope();
        DefineRoles(scope);
        return scope.Build();
    }
}

public sealed class RoleScope
{
    private readonly List<CharacterRole> _roles = [];

    public RoleScope AddCountryLeader(string ideology, params string[] traits)
    {
        _roles.Add(new CountryLeaderRole(ideology, traits));
        return this;
    }

    public RoleScope AddAdvisor(string slot, int cost = 150, params string[] traits)
    {
        _roles.Add(new AdvisorRole(slot, traits, cost));
        return this;
    }

    public RoleScope AddGeneral(int skill = 1, int attack = 1, int defense = 1, params string[] traits)
    {
        _roles.Add(new CommanderRole(CommanderType.CorpsCommander, skill, attack, defense, 1, 1, traits));
        return this;
    }

    internal IReadOnlyList<CharacterRole> Build() => _roles.AsReadOnly();
}
