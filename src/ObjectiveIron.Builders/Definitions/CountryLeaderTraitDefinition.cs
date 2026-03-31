using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines country leader traits.
/// Emits to: common/country_leader/traits/{Id}.txt
/// </summary>
public abstract class CountryLeaderTraitDefinition
{
    public abstract string Id { get; }

    protected abstract void Define(LeaderTraitFileScope s);

    public LeaderTraitFileBuild Build()
    {
        var scope = new LeaderTraitFileScope();
        Define(scope);
        return new LeaderTraitFileBuild(Id, scope.Build());
    }
}

/// <summary>
/// Defines unit leader traits (generals/admirals).
/// Emits to: common/unit_leader/{Id}.txt
/// </summary>
public abstract class UnitLeaderTraitDefinition
{
    public abstract string Id { get; }

    protected abstract void Define(LeaderTraitFileScope s);

    public LeaderTraitFileBuild Build()
    {
        var scope = new LeaderTraitFileScope();
        Define(scope);
        return new LeaderTraitFileBuild(Id, scope.Build());
    }
}

public record LeaderTraitFileBuild(string Id, IReadOnlyList<LeaderTraitEntry> Traits);

public class LeaderTraitFileScope
{
    private readonly List<LeaderTraitEntry> _traits = [];

    public LeaderTraitFileScope Add(string id, Action<LeaderTraitBuilder> configure)
    {
        var builder = new LeaderTraitBuilder(id);
        configure(builder);
        _traits.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<LeaderTraitEntry> Build() => _traits.AsReadOnly();
}

public class LeaderTraitBuilder
{
    private readonly string _id;
    private string? _type; // "land", "navy", "all", "corps_commander", "field_marshal"
    private readonly Dictionary<string, double> _modifiers = new();
    private readonly Dictionary<string, double> _nonStackable = new();
    private Action<TriggerScope>? _allowed;
    private Action<TriggerScope>? _visible;
    private string? _sprite;
    private bool? _randomAssign;
    private readonly List<string> _traitXp = [];

    public LeaderTraitBuilder(string id) => _id = id;

    public LeaderTraitBuilder Type(string v) { _type = v; return this; }
    public LeaderTraitBuilder Sprite(string v) { _sprite = v; return this; }
    public LeaderTraitBuilder RandomAssign(bool v) { _randomAssign = v; return this; }
    public LeaderTraitBuilder Modifier(string key, double value) { _modifiers[key] = value; return this; }
    public LeaderTraitBuilder NonStackableModifier(string key, double value) { _nonStackable[key] = value; return this; }
    public LeaderTraitBuilder Allowed(Action<TriggerScope> a) { _allowed = a; return this; }
    public LeaderTraitBuilder Visible(Action<TriggerScope> a) { _visible = a; return this; }
    public LeaderTraitBuilder TraitXp(params string[] skills) { _traitXp.AddRange(skills); return this; }

    // Common trait modifiers
    public LeaderTraitBuilder Attack(double v) => Modifier("attack", v);
    public LeaderTraitBuilder Defense(double v) => Modifier("defense", v);
    public LeaderTraitBuilder Planning(double v) => Modifier("planning", v);
    public LeaderTraitBuilder Logistics(double v) => Modifier("logistics", v);
    public LeaderTraitBuilder Speed(double v) => Modifier("speed", v);
    public LeaderTraitBuilder MaxOrganisation(double v) => Modifier("max_organisation_factor", v);
    public LeaderTraitBuilder Entrenchment(double v) => Modifier("entrenchment", v);

    internal LeaderTraitEntry Build()
    {
        Block? allowedBlock = null, visibleBlock = null;
        if (_allowed != null) { var s = new TriggerScope(); _allowed(s); allowedBlock = s.Build(); }
        if (_visible != null) { var s = new TriggerScope(); _visible(s); visibleBlock = s.Build(); }

        return new LeaderTraitEntry(
            _id, _type, _sprite, _randomAssign,
            _modifiers.Count > 0 ? new Dictionary<string, double>(_modifiers) : null,
            _nonStackable.Count > 0 ? new Dictionary<string, double>(_nonStackable) : null,
            allowedBlock, visibleBlock,
            _traitXp.Count > 0 ? _traitXp.AsReadOnly() : null
        );
    }
}

public record LeaderTraitEntry(
    string Id,
    string? Type,
    string? Sprite,
    bool? RandomAssign,
    Dictionary<string, double>? Modifiers,
    Dictionary<string, double>? NonStackableModifiers,
    Block? Allowed,
    Block? Visible,
    IReadOnlyList<string>? TraitXp
);
