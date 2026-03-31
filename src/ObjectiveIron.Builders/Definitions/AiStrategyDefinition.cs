using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines AI strategy entries for a country.
/// Emits to: common/ai_strategy/{Id}.txt
/// </summary>
public abstract class AiStrategyDefinition
{
    /// <summary>File identifier / filename.</summary>
    public abstract string Id { get; }

    /// <summary>Define AI strategy entries.</summary>
    protected abstract void Define(AiStrategyFileScope s);

    public AiStrategyFileBuild Build()
    {
        var scope = new AiStrategyFileScope();
        Define(scope);
        return new AiStrategyFileBuild(Id, scope.Build());
    }
}

public record AiStrategyFileBuild(
    string Id,
    IReadOnlyList<AiStrategyEntryBuild> Entries
);

public class AiStrategyFileScope
{
    private readonly List<AiStrategyEntryBuild> _entries = [];

    public AiStrategyFileScope Add(string id, Action<AiStrategyEntryBuilder> configure)
    {
        var builder = new AiStrategyEntryBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<AiStrategyEntryBuild> Build() => _entries.AsReadOnly();
}

public class AiStrategyEntryBuilder
{
    private readonly string _id;
    private Action<TriggerScope>? _allowed;
    private Action<TriggerScope>? _enable;
    private Action<TriggerScope>? _abort;
    private readonly List<AiStrategyValue> _strategies = [];

    public AiStrategyEntryBuilder(string id) => _id = id;

    public AiStrategyEntryBuilder Allowed(Action<TriggerScope> a) { _allowed = a; return this; }
    public AiStrategyEntryBuilder Enable(Action<TriggerScope> a) { _enable = a; return this; }
    public AiStrategyEntryBuilder Abort(Action<TriggerScope> a) { _abort = a; return this; }

    public AiStrategyEntryBuilder Strategy(string type, string targetId, int value)
    {
        _strategies.Add(new AiStrategyValue(type, targetId, value));
        return this;
    }

    internal AiStrategyEntryBuild Build()
    {
        Block? allowedBlock = null, enableBlock = null, abortBlock = null;
        if (_allowed != null) { var s = new TriggerScope(); _allowed(s); allowedBlock = s.Build(); }
        if (_enable != null) { var s = new TriggerScope(); _enable(s); enableBlock = s.Build(); }
        if (_abort != null) { var s = new TriggerScope(); _abort(s); abortBlock = s.Build(); }

        return new AiStrategyEntryBuild(_id, allowedBlock, enableBlock, abortBlock, _strategies.AsReadOnly());
    }
}

public record AiStrategyEntryBuild(
    string Id,
    Block? Allowed,
    Block? Enable,
    Block? Abort,
    IReadOnlyList<AiStrategyValue> Strategies
);

public record AiStrategyValue(string Type, string TargetId, int Value);
