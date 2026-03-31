using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines intelligence operations (La Résistance).
/// Emits to: common/operations/{Id}.txt
/// </summary>
public abstract class OperationDefinition
{
    public abstract string Id { get; }

    protected abstract void Define(OperationFileScope s);

    public OperationFileBuild Build()
    {
        var scope = new OperationFileScope();
        Define(scope);
        return new OperationFileBuild(Id, scope.Build());
    }
}

public record OperationFileBuild(string Id, IReadOnlyList<OperationEntry> Operations);

public class OperationFileScope
{
    private readonly List<OperationEntry> _ops = [];

    public OperationFileScope Add(string id, Action<OperationBuilder> configure)
    {
        var builder = new OperationBuilder(id);
        configure(builder);
        _ops.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<OperationEntry> Build() => _ops.AsReadOnly();
}

public class OperationBuilder
{
    private readonly string _id;
    private string? _icon;
    private string? _mapIcon;
    private int? _priority;
    private int? _days;
    private int? _networkStrength;
    private int? _operativesNeeded;
    private double? _riskChance;
    private double? _experienceGain;
    private double? _cost;
    private Action<TriggerScope>? _visible;
    private Action<TriggerScope>? _available;
    private Action<EffectScope>? _onStart;
    private Action<EffectScope>? _outcome;
    private Action<EffectScope>? _riskOutcome;

    public OperationBuilder(string id) => _id = id;

    public OperationBuilder Icon(string v) { _icon = v; return this; }
    public OperationBuilder MapIcon(string v) { _mapIcon = v; return this; }
    public OperationBuilder Priority(int v) { _priority = v; return this; }
    public OperationBuilder Days(int v) { _days = v; return this; }
    public OperationBuilder NetworkStrength(int v) { _networkStrength = v; return this; }
    public OperationBuilder OperativesNeeded(int v) { _operativesNeeded = v; return this; }
    public OperationBuilder RiskChance(double v) { _riskChance = v; return this; }
    public OperationBuilder ExperienceGain(double v) { _experienceGain = v; return this; }
    public OperationBuilder Cost(double v) { _cost = v; return this; }
    public OperationBuilder Visible(Action<TriggerScope> a) { _visible = a; return this; }
    public OperationBuilder Available(Action<TriggerScope> a) { _available = a; return this; }
    public OperationBuilder OnStart(Action<EffectScope> a) { _onStart = a; return this; }
    public OperationBuilder Outcome(Action<EffectScope> a) { _outcome = a; return this; }
    public OperationBuilder RiskOutcome(Action<EffectScope> a) { _riskOutcome = a; return this; }

    internal OperationEntry Build()
    {
        Block? visBlock = null, availBlock = null, startBlock = null, outcomeBlock = null, riskBlock = null;
        if (_visible != null) { var s = new TriggerScope(); _visible(s); visBlock = s.Build(); }
        if (_available != null) { var s = new TriggerScope(); _available(s); availBlock = s.Build(); }
        if (_onStart != null) { var s = new EffectScope(); _onStart(s); startBlock = s.Build(); }
        if (_outcome != null) { var s = new EffectScope(); _outcome(s); outcomeBlock = s.Build(); }
        if (_riskOutcome != null) { var s = new EffectScope(); _riskOutcome(s); riskBlock = s.Build(); }

        return new OperationEntry(_id, _icon, _mapIcon, _priority, _days, _networkStrength,
            _operativesNeeded, _riskChance, _experienceGain, _cost,
            visBlock, availBlock, startBlock, outcomeBlock, riskBlock);
    }
}

public record OperationEntry(
    string Id, string? Icon, string? MapIcon, int? Priority, int? Days,
    int? NetworkStrength, int? OperativesNeeded, double? RiskChance,
    double? ExperienceGain, double? Cost,
    Block? Visible, Block? Available, Block? OnStart, Block? Outcome, Block? RiskOutcome
);
