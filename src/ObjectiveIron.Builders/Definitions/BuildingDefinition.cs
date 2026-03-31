using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines custom building types.
/// Emits to: common/buildings/{Id}.txt
/// </summary>
public abstract class BuildingDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(BuildingFileScope s);

    public BuildingFileBuild Build()
    {
        var scope = new BuildingFileScope();
        Define(scope);
        return new BuildingFileBuild(Id, scope.Build());
    }
}

public record BuildingFileBuild(string Id, IReadOnlyList<BuildingEntry> Buildings);

public class BuildingFileScope
{
    private readonly List<BuildingEntry> _entries = [];

    public BuildingFileScope Add(string id, Action<BuildingBuilder> configure)
    {
        var builder = new BuildingBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<BuildingEntry> Build() => _entries.AsReadOnly();
}

public class BuildingBuilder
{
    private readonly string _id;
    private int? _maxLevel;
    private int? _baseHealthPerLevel;
    private double? _value;
    private string? _iconFrame;
    private bool? _provincial;
    private bool? _perState;
    private bool? _onlyCoastal;
    private readonly Dictionary<string, double> _modifiers = new();

    public BuildingBuilder(string id) => _id = id;

    public BuildingBuilder MaxLevel(int v) { _maxLevel = v; return this; }
    public BuildingBuilder BaseHealthPerLevel(int v) { _baseHealthPerLevel = v; return this; }
    public BuildingBuilder Value(double v) { _value = v; return this; }
    public BuildingBuilder IconFrame(string v) { _iconFrame = v; return this; }
    public BuildingBuilder Provincial(bool v) { _provincial = v; return this; }
    public BuildingBuilder PerState(bool v) { _perState = v; return this; }
    public BuildingBuilder OnlyCoastal(bool v) { _onlyCoastal = v; return this; }
    public BuildingBuilder Modifier(string key, double val) { _modifiers[key] = val; return this; }

    internal BuildingEntry Build() => new(_id, _maxLevel, _baseHealthPerLevel, _value, _iconFrame,
        _provincial, _perState, _onlyCoastal,
        _modifiers.Count > 0 ? new Dictionary<string, double>(_modifiers) : null);
}

public record BuildingEntry(
    string Id, int? MaxLevel, int? BaseHealthPerLevel, double? Value, string? IconFrame,
    bool? Provincial, bool? PerState, bool? OnlyCoastal,
    Dictionary<string, double>? Modifiers
);
