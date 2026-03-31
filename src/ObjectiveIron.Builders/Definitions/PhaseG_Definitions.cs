using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

// ─── Military Industrial Organization (MIO) ──────────────────
// common/military_industrial_organization/organizations/{Id}.txt

public abstract class MioDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(MioFileScope s);

    public MioFileBuild Build()
    {
        var scope = new MioFileScope();
        Define(scope);
        return new MioFileBuild(Id, scope.Build());
    }
}

public record MioFileBuild(string Id, IReadOnlyList<MioEntry> Organizations);

public class MioFileScope
{
    private readonly List<MioEntry> _entries = [];

    public MioFileScope Add(string id, Action<MioBuilder> configure)
    {
        var builder = new MioBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<MioEntry> Build() => _entries.AsReadOnly();
}

public class MioBuilder
{
    private readonly string _id;
    private string? _name;
    private string? _icon;
    private string? _ownerTag;
    private readonly List<string> _equipmentTypes = [];
    private readonly List<string> _researchCategories = [];
    private readonly Dictionary<string, double> _initialTraitModifiers = new();
    private Action<TriggerScope>? _allowed;
    private Action<TriggerScope>? _visible;
    private Action<TriggerScope>? _available;
    private readonly List<MioTraitEntry> _traits = [];

    public MioBuilder(string id) => _id = id;

    public MioBuilder Name(string v) { _name = v; return this; }
    public MioBuilder Icon(string v) { _icon = v; return this; }
    public MioBuilder OwnerTag(string v) { _ownerTag = v; return this; }
    public MioBuilder EquipmentType(params string[] types) { _equipmentTypes.AddRange(types); return this; }
    public MioBuilder ResearchCategory(params string[] cats) { _researchCategories.AddRange(cats); return this; }
    public MioBuilder InitialModifier(string key, double val) { _initialTraitModifiers[key] = val; return this; }
    public MioBuilder Allowed(Action<TriggerScope> a) { _allowed = a; return this; }
    public MioBuilder Visible(Action<TriggerScope> a) { _visible = a; return this; }
    public MioBuilder Available(Action<TriggerScope> a) { _available = a; return this; }

    public MioBuilder Trait(string id, Action<MioTraitBuilder> configure)
    {
        var builder = new MioTraitBuilder(id);
        configure(builder);
        _traits.Add(builder.Build());
        return this;
    }

    internal MioEntry Build()
    {
        Block? allowedBlock = null, visBlock = null, availBlock = null;
        if (_allowed != null) { var s = new TriggerScope(); _allowed(s); allowedBlock = s.Build(); }
        if (_visible != null) { var s = new TriggerScope(); _visible(s); visBlock = s.Build(); }
        if (_available != null) { var s = new TriggerScope(); _available(s); availBlock = s.Build(); }

        return new MioEntry(_id, _name, _icon, _ownerTag,
            _equipmentTypes.Count > 0 ? _equipmentTypes.AsReadOnly() : null,
            _researchCategories.Count > 0 ? _researchCategories.AsReadOnly() : null,
            _initialTraitModifiers.Count > 0 ? new Dictionary<string, double>(_initialTraitModifiers) : null,
            allowedBlock, visBlock, availBlock,
            _traits.Count > 0 ? _traits.AsReadOnly() : null);
    }
}

public class MioTraitBuilder
{
    private readonly string _id;
    private string? _icon;
    private string? _parent;
    private int? _position;
    private readonly Dictionary<string, double> _modifiers = new();

    public MioTraitBuilder(string id) => _id = id;

    public MioTraitBuilder Icon(string v) { _icon = v; return this; }
    public MioTraitBuilder Parent(string v) { _parent = v; return this; }
    public MioTraitBuilder Position(int v) { _position = v; return this; }
    public MioTraitBuilder Modifier(string key, double val) { _modifiers[key] = val; return this; }

    internal MioTraitEntry Build() => new(_id, _icon, _parent, _position,
        _modifiers.Count > 0 ? new Dictionary<string, double>(_modifiers) : null);
}

public record MioEntry(
    string Id, string? Name, string? Icon, string? OwnerTag,
    IReadOnlyList<string>? EquipmentTypes, IReadOnlyList<string>? ResearchCategories,
    Dictionary<string, double>? InitialModifiers,
    Block? Allowed, Block? Visible, Block? Available,
    IReadOnlyList<MioTraitEntry>? Traits
);

public record MioTraitEntry(string Id, string? Icon, string? Parent, int? Position,
    Dictionary<string, double>? Modifiers);

// ─── Intelligence Agency ─────────────────────────────────────
// common/intelligence_agencies/{Id}.txt

public abstract class IntelligenceAgencyDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(IntelAgencyFileScope s);

    public IntelAgencyFileBuild Build()
    {
        var scope = new IntelAgencyFileScope();
        Define(scope);
        return new IntelAgencyFileBuild(Id, scope.Build());
    }
}

public record IntelAgencyFileBuild(string Id, IReadOnlyList<IntelAgencyEntry> Agencies);

public class IntelAgencyFileScope
{
    private readonly List<IntelAgencyEntry> _entries = [];

    public IntelAgencyFileScope Add(string id, Action<IntelAgencyBuilder> configure)
    {
        var builder = new IntelAgencyBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<IntelAgencyEntry> Build() => _entries.AsReadOnly();
}

public class IntelAgencyBuilder
{
    private readonly string _id;
    private string? _picture;
    private string? _names;
    private readonly List<IntelAgencyUpgradeRef> _upgrades = [];

    public IntelAgencyBuilder(string id) => _id = id;

    public IntelAgencyBuilder Picture(string v) { _picture = v; return this; }
    public IntelAgencyBuilder Names(string v) { _names = v; return this; }
    public IntelAgencyBuilder DefaultUpgrade(string upgradeId) { _upgrades.Add(new IntelAgencyUpgradeRef(upgradeId)); return this; }

    internal IntelAgencyEntry Build() => new(_id, _picture, _names,
        _upgrades.Count > 0 ? _upgrades.AsReadOnly() : null);
}

public record IntelAgencyEntry(string Id, string? Picture, string? Names, IReadOnlyList<IntelAgencyUpgradeRef>? Upgrades);
public record IntelAgencyUpgradeRef(string UpgradeId);

// ─── Peace Conference ────────────────────────────────────────
// common/peace_conference/{Id}.txt

public abstract class PeaceConferenceDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(PeaceConferenceFileScope s);

    public PeaceConferenceFileBuild Build()
    {
        var scope = new PeaceConferenceFileScope();
        Define(scope);
        return new PeaceConferenceFileBuild(Id, scope.Build());
    }
}

public record PeaceConferenceFileBuild(string Id, IReadOnlyList<PeaceActionEntry> Actions);

public class PeaceConferenceFileScope
{
    private readonly List<PeaceActionEntry> _actions = [];

    public PeaceConferenceFileScope Add(string id, Action<PeaceActionBuilder> configure)
    {
        var builder = new PeaceActionBuilder(id);
        configure(builder);
        _actions.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<PeaceActionEntry> Build() => _actions.AsReadOnly();
}

public class PeaceActionBuilder
{
    private readonly string _id;
    private double? _cost;
    private Action<TriggerScope>? _available;
    private Action<EffectScope>? _effect;

    public PeaceActionBuilder(string id) => _id = id;

    public PeaceActionBuilder Cost(double v) { _cost = v; return this; }
    public PeaceActionBuilder Available(Action<TriggerScope> a) { _available = a; return this; }
    public PeaceActionBuilder Effect(Action<EffectScope> a) { _effect = a; return this; }

    internal PeaceActionEntry Build()
    {
        Block? availBlock = null, effectBlock = null;
        if (_available != null) { var s = new TriggerScope(); _available(s); availBlock = s.Build(); }
        if (_effect != null) { var s = new EffectScope(); _effect(s); effectBlock = s.Build(); }
        return new PeaceActionEntry(_id, _cost, availBlock, effectBlock);
    }
}

public record PeaceActionEntry(string Id, double? Cost, Block? Available, Block? Effect);

// ─── Balance of Power ────────────────────────────────────────
// common/bop/{Id}.txt

public abstract class BalanceOfPowerDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(BopFileScope s);

    public BopFileBuild Build()
    {
        var scope = new BopFileScope();
        Define(scope);
        return new BopFileBuild(Id, scope.Build());
    }
}

public record BopFileBuild(string Id, IReadOnlyList<BopEntry> Entries);

public class BopFileScope
{
    private readonly List<BopEntry> _entries = [];

    public BopFileScope Add(string id, Action<BopBuilder> configure)
    {
        var builder = new BopBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<BopEntry> Build() => _entries.AsReadOnly();
}

public class BopBuilder
{
    private readonly string _id;
    private double? _initialValue;
    private string? _leftSide;
    private string? _rightSide;
    private readonly List<BopRangeEntry> _ranges = [];

    public BopBuilder(string id) => _id = id;

    public BopBuilder InitialValue(double v) { _initialValue = v; return this; }
    public BopBuilder LeftSide(string v) { _leftSide = v; return this; }
    public BopBuilder RightSide(string v) { _rightSide = v; return this; }

    public BopBuilder Range(double min, double max, Action<Dictionary<string, double>>? modifiers = null)
    {
        var mods = new Dictionary<string, double>();
        modifiers?.Invoke(mods);
        _ranges.Add(new BopRangeEntry(min, max, mods.Count > 0 ? mods : null));
        return this;
    }

    internal BopEntry Build() => new(_id, _initialValue, _leftSide, _rightSide,
        _ranges.Count > 0 ? _ranges.AsReadOnly() : null);
}

public record BopEntry(string Id, double? InitialValue, string? LeftSide, string? RightSide,
    IReadOnlyList<BopRangeEntry>? Ranges);
public record BopRangeEntry(double Min, double Max, Dictionary<string, double>? Modifiers);

// ─── Scripted Diplomatic Actions ─────────────────────────────
// common/scripted_diplomatic_actions/{Id}.txt

public abstract class ScriptedDiplomaticActionDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(ScriptedDiploFileScope s);

    public ScriptedDiploFileBuild Build()
    {
        var scope = new ScriptedDiploFileScope();
        Define(scope);
        return new ScriptedDiploFileBuild(Id, scope.Build());
    }
}

public record ScriptedDiploFileBuild(string Id, IReadOnlyList<ScriptedDiploEntry> Actions);

public class ScriptedDiploFileScope
{
    private readonly List<ScriptedDiploEntry> _entries = [];

    public ScriptedDiploFileScope Add(string id, Action<ScriptedDiploBuilder> configure)
    {
        var builder = new ScriptedDiploBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<ScriptedDiploEntry> Build() => _entries.AsReadOnly();
}

public class ScriptedDiploBuilder
{
    private readonly string _id;
    private Action<TriggerScope>? _visible;
    private Action<TriggerScope>? _available;
    private Action<EffectScope>? _completeEffect;
    private Action<EffectScope>? _removeEffect;
    private double? _cost;

    public ScriptedDiploBuilder(string id) => _id = id;

    public ScriptedDiploBuilder Visible(Action<TriggerScope> a) { _visible = a; return this; }
    public ScriptedDiploBuilder Available(Action<TriggerScope> a) { _available = a; return this; }
    public ScriptedDiploBuilder CompleteEffect(Action<EffectScope> a) { _completeEffect = a; return this; }
    public ScriptedDiploBuilder RemoveEffect(Action<EffectScope> a) { _removeEffect = a; return this; }
    public ScriptedDiploBuilder Cost(double v) { _cost = v; return this; }

    internal ScriptedDiploEntry Build()
    {
        Block? visBlock = null, availBlock = null, compBlock = null, remBlock = null;
        if (_visible != null) { var s = new TriggerScope(); _visible(s); visBlock = s.Build(); }
        if (_available != null) { var s = new TriggerScope(); _available(s); availBlock = s.Build(); }
        if (_completeEffect != null) { var s = new EffectScope(); _completeEffect(s); compBlock = s.Build(); }
        if (_removeEffect != null) { var s = new EffectScope(); _removeEffect(s); remBlock = s.Build(); }
        return new ScriptedDiploEntry(_id, _cost, visBlock, availBlock, compBlock, remBlock);
    }
}

public record ScriptedDiploEntry(string Id, double? Cost, Block? Visible, Block? Available,
    Block? CompleteEffect, Block? RemoveEffect);

// ─── Scripted GUI ────────────────────────────────────────────
// common/scripted_guis/{Id}.txt

public abstract class ScriptedGuiDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(ScriptedGuiFileScope s);

    public ScriptedGuiFileBuild Build()
    {
        var scope = new ScriptedGuiFileScope();
        Define(scope);
        return new ScriptedGuiFileBuild(Id, scope.Build());
    }
}

public record ScriptedGuiFileBuild(string Id, IReadOnlyList<ScriptedGuiEntry> Guis);

public class ScriptedGuiFileScope
{
    private readonly List<ScriptedGuiEntry> _entries = [];

    public ScriptedGuiFileScope Add(string id, Action<ScriptedGuiBuilder> configure)
    {
        var builder = new ScriptedGuiBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<ScriptedGuiEntry> Build() => _entries.AsReadOnly();
}

public class ScriptedGuiBuilder
{
    private readonly string _id;
    private string? _context;
    private string? _window;
    private Action<TriggerScope>? _visible;
    private readonly List<ScriptedGuiEffect> _effects = [];
    private readonly List<ScriptedGuiTrigger> _triggers = [];
    private readonly List<ScriptedGuiProperty> _properties = [];

    public ScriptedGuiBuilder(string id) => _id = id;

    public ScriptedGuiBuilder Context(string v) { _context = v; return this; }
    public ScriptedGuiBuilder Window(string v) { _window = v; return this; }
    public ScriptedGuiBuilder Visible(Action<TriggerScope> a) { _visible = a; return this; }

    public ScriptedGuiBuilder Effect(string name, Action<EffectScope> configure)
    {
        var scope = new EffectScope();
        configure(scope);
        _effects.Add(new ScriptedGuiEffect(name, scope.Build()));
        return this;
    }

    public ScriptedGuiBuilder Trigger(string name, Action<TriggerScope> configure)
    {
        var scope = new TriggerScope();
        configure(scope);
        _triggers.Add(new ScriptedGuiTrigger(name, scope.Build()));
        return this;
    }

    public ScriptedGuiBuilder Property(string name, string value)
    {
        _properties.Add(new ScriptedGuiProperty(name, value));
        return this;
    }

    internal ScriptedGuiEntry Build()
    {
        Block? visBlock = null;
        if (_visible != null) { var s = new TriggerScope(); _visible(s); visBlock = s.Build(); }
        return new ScriptedGuiEntry(_id, _context, _window, visBlock,
            _effects.Count > 0 ? _effects.AsReadOnly() : null,
            _triggers.Count > 0 ? _triggers.AsReadOnly() : null,
            _properties.Count > 0 ? _properties.AsReadOnly() : null);
    }
}

public record ScriptedGuiEntry(string Id, string? Context, string? Window, Block? Visible,
    IReadOnlyList<ScriptedGuiEffect>? Effects, IReadOnlyList<ScriptedGuiTrigger>? Triggers,
    IReadOnlyList<ScriptedGuiProperty>? Properties);
public record ScriptedGuiEffect(string Name, Block Block);
public record ScriptedGuiTrigger(string Name, Block Block);
public record ScriptedGuiProperty(string Name, string Value);
