using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines occupation laws.
/// Emits to: common/occupation_laws/{Id}.txt
/// </summary>
public abstract class OccupationLawDefinition
{
    public abstract string Id { get; }

    protected abstract void Define(OccupationLawFileScope s);

    public OccupationLawFileBuild Build()
    {
        var scope = new OccupationLawFileScope();
        Define(scope);
        return new OccupationLawFileBuild(Id, scope.Build());
    }
}

public record OccupationLawFileBuild(string Id, IReadOnlyList<OccupationLawEntry> Laws);

public class OccupationLawFileScope
{
    private readonly List<OccupationLawEntry> _laws = [];

    public OccupationLawFileScope Add(string id, Action<OccupationLawBuilder> configure)
    {
        var builder = new OccupationLawBuilder(id);
        configure(builder);
        _laws.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<OccupationLawEntry> Build() => _laws.AsReadOnly();
}

public class OccupationLawBuilder
{
    private readonly string _id;
    private string? _icon;
    private string? _sound;
    private readonly Dictionary<string, double> _modifiers = new();
    private Action<TriggerScope>? _visible;
    private Action<TriggerScope>? _available;
    private double? _stateFrame;
    private double? _guiOrder;

    public OccupationLawBuilder(string id) => _id = id;

    public OccupationLawBuilder Icon(string v) { _icon = v; return this; }
    public OccupationLawBuilder Sound(string v) { _sound = v; return this; }
    public OccupationLawBuilder StateFrame(double v) { _stateFrame = v; return this; }
    public OccupationLawBuilder GuiOrder(double v) { _guiOrder = v; return this; }
    public OccupationLawBuilder Modifier(string key, double value) { _modifiers[key] = value; return this; }
    public OccupationLawBuilder Visible(Action<TriggerScope> a) { _visible = a; return this; }
    public OccupationLawBuilder Available(Action<TriggerScope> a) { _available = a; return this; }

    internal OccupationLawEntry Build()
    {
        Block? visBlock = null, availBlock = null;
        if (_visible != null) { var s = new TriggerScope(); _visible(s); visBlock = s.Build(); }
        if (_available != null) { var s = new TriggerScope(); _available(s); availBlock = s.Build(); }

        return new OccupationLawEntry(
            _id, _icon, _sound, _stateFrame, _guiOrder,
            _modifiers.Count > 0 ? new Dictionary<string, double>(_modifiers) : null,
            visBlock, availBlock
        );
    }
}

public record OccupationLawEntry(
    string Id,
    string? Icon,
    string? Sound,
    double? StateFrame,
    double? GuiOrder,
    Dictionary<string, double>? Modifiers,
    Block? Visible,
    Block? Available
);
