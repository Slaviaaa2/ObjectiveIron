using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines leader abilities (divisional commander abilities).
/// Emits to: common/abilities/{Id}.txt
/// </summary>
public abstract class AbilityDefinition
{
    public abstract string Id { get; }

    protected abstract void Define(AbilityFileScope s);

    public AbilityFileBuild Build()
    {
        var scope = new AbilityFileScope();
        Define(scope);
        return new AbilityFileBuild(Id, scope.Build());
    }
}

public record AbilityFileBuild(string Id, IReadOnlyList<AbilityEntry> Abilities);

public class AbilityFileScope
{
    private readonly List<AbilityEntry> _abilities = [];

    public AbilityFileScope Add(string id, Action<AbilityBuilder> configure)
    {
        var builder = new AbilityBuilder(id);
        configure(builder);
        _abilities.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<AbilityEntry> Build() => _abilities.AsReadOnly();
}

public class AbilityBuilder
{
    private readonly string _id;
    private string? _name;
    private string? _desc;
    private string? _icon;
    private string? _sound;
    private string? _type; // army_leader, navy_leader
    private double? _cost;
    private int? _duration;
    private int? _cooldown;
    private Action<TriggerScope>? _allowed;
    private Action<TriggerScope>? _available;
    private readonly Dictionary<string, double> _unitModifiers = new();
    private Action<EffectScope>? _onActivate;
    private Action<EffectScope>? _cancelEffect;
    private Action<TriggerScope>? _cancelTrigger;

    public AbilityBuilder(string id) => _id = id;

    public AbilityBuilder Name(string v) { _name = v; return this; }
    public AbilityBuilder Desc(string v) { _desc = v; return this; }
    public AbilityBuilder Icon(string v) { _icon = v; return this; }
    public AbilityBuilder Sound(string v) { _sound = v; return this; }
    public AbilityBuilder AbilityType(string v) { _type = v; return this; }
    public AbilityBuilder Cost(double v) { _cost = v; return this; }
    public AbilityBuilder Duration(int v) { _duration = v; return this; }
    public AbilityBuilder Cooldown(int v) { _cooldown = v; return this; }
    public AbilityBuilder UnitModifier(string key, double value) { _unitModifiers[key] = value; return this; }
    public AbilityBuilder Allowed(Action<TriggerScope> a) { _allowed = a; return this; }
    public AbilityBuilder Available(Action<TriggerScope> a) { _available = a; return this; }
    public AbilityBuilder OnActivate(Action<EffectScope> a) { _onActivate = a; return this; }
    public AbilityBuilder CancelEffect(Action<EffectScope> a) { _cancelEffect = a; return this; }
    public AbilityBuilder CancelTrigger(Action<TriggerScope> a) { _cancelTrigger = a; return this; }

    internal AbilityEntry Build()
    {
        Block? allowedBlock = null, availBlock = null, activateBlock = null, cancelEffBlock = null, cancelTrigBlock = null;
        if (_allowed != null) { var s = new TriggerScope(); _allowed(s); allowedBlock = s.Build(); }
        if (_available != null) { var s = new TriggerScope(); _available(s); availBlock = s.Build(); }
        if (_onActivate != null) { var s = new EffectScope(); _onActivate(s); activateBlock = s.Build(); }
        if (_cancelEffect != null) { var s = new EffectScope(); _cancelEffect(s); cancelEffBlock = s.Build(); }
        if (_cancelTrigger != null) { var s = new TriggerScope(); _cancelTrigger(s); cancelTrigBlock = s.Build(); }

        return new AbilityEntry(_id, _name, _desc, _icon, _sound, _type, _cost, _duration, _cooldown,
            _unitModifiers.Count > 0 ? new Dictionary<string, double>(_unitModifiers) : null,
            allowedBlock, availBlock, activateBlock, cancelEffBlock, cancelTrigBlock);
    }
}

public record AbilityEntry(
    string Id, string? Name, string? Desc, string? Icon, string? Sound,
    string? Type, double? Cost, int? Duration, int? Cooldown,
    Dictionary<string, double>? UnitModifiers,
    Block? Allowed, Block? Available, Block? OnActivate, Block? CancelEffect, Block? CancelTrigger
);
