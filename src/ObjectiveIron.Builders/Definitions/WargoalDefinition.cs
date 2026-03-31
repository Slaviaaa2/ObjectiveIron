using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines war goal types.
/// Emits to: common/wargoals/{Id}.txt
/// </summary>
public abstract class WargoalDefinition
{
    public abstract string Id { get; }

    protected abstract void Define(WargoalFileScope s);

    public WargoalFileBuild Build()
    {
        var scope = new WargoalFileScope();
        Define(scope);
        return new WargoalFileBuild(Id, scope.Build());
    }
}

public record WargoalFileBuild(string Id, IReadOnlyList<WargoalEntry> Wargoals);

public class WargoalFileScope
{
    private readonly List<WargoalEntry> _entries = [];

    public WargoalFileScope Add(string id, Action<WargoalBuilder> configure)
    {
        var builder = new WargoalBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<WargoalEntry> Build() => _entries.AsReadOnly();
}

public class WargoalBuilder
{
    private readonly string _id;
    private double? _warDesirability;
    private double? _worldTension;
    private double? _justifyWarGoalCost;
    private bool? _generateCb;
    private Action<TriggerScope>? _available;
    private Action<TriggerScope>? _allowed;
    private Action<EffectScope>? _onAdd;
    private Action<EffectScope>? _onSuccess;

    public WargoalBuilder(string id) => _id = id;

    public WargoalBuilder WarDesirability(double v) { _warDesirability = v; return this; }
    public WargoalBuilder WorldTension(double v) { _worldTension = v; return this; }
    public WargoalBuilder JustifyWarGoalCost(double v) { _justifyWarGoalCost = v; return this; }
    public WargoalBuilder GenerateCb(bool v) { _generateCb = v; return this; }
    public WargoalBuilder Available(Action<TriggerScope> a) { _available = a; return this; }
    public WargoalBuilder Allowed(Action<TriggerScope> a) { _allowed = a; return this; }
    public WargoalBuilder OnAdd(Action<EffectScope> a) { _onAdd = a; return this; }
    public WargoalBuilder OnSuccess(Action<EffectScope> a) { _onSuccess = a; return this; }

    internal WargoalEntry Build()
    {
        Block? availBlock = null, allowedBlock = null, onAddBlock = null, onSuccessBlock = null;
        if (_available != null) { var s = new TriggerScope(); _available(s); availBlock = s.Build(); }
        if (_allowed != null) { var s = new TriggerScope(); _allowed(s); allowedBlock = s.Build(); }
        if (_onAdd != null) { var s = new EffectScope(); _onAdd(s); onAddBlock = s.Build(); }
        if (_onSuccess != null) { var s = new EffectScope(); _onSuccess(s); onSuccessBlock = s.Build(); }

        return new WargoalEntry(_id, _warDesirability, _worldTension, _justifyWarGoalCost, _generateCb,
            availBlock, allowedBlock, onAddBlock, onSuccessBlock);
    }
}

public record WargoalEntry(
    string Id,
    double? WarDesirability,
    double? WorldTension,
    double? JustifyWarGoalCost,
    bool? GenerateCb,
    Block? Available,
    Block? Allowed,
    Block? OnAdd,
    Block? OnSuccess
);
