using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

// ─── Static Modifiers ────────────────────────────────────────
// common/static_modifiers/{Id}.txt

public abstract class StaticModifierDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(StaticModifierFileScope s);

    public StaticModifierFileBuild Build()
    {
        var scope = new StaticModifierFileScope();
        Define(scope);
        return new StaticModifierFileBuild(Id, scope.Build());
    }
}

public record StaticModifierFileBuild(string Id, IReadOnlyList<StaticModifierEntry> Modifiers);

public class StaticModifierFileScope
{
    private readonly List<StaticModifierEntry> _entries = [];

    public StaticModifierFileScope Add(string id, Action<Dictionary<string, double>> configure)
    {
        var mods = new Dictionary<string, double>();
        configure(mods);
        _entries.Add(new StaticModifierEntry(id, mods));
        return this;
    }

    internal IReadOnlyList<StaticModifierEntry> Build() => _entries.AsReadOnly();
}

public record StaticModifierEntry(string Id, Dictionary<string, double> Modifiers);

// ─── Game Rules ──────────────────────────────────────────────
// common/game_rules/{Id}.txt

public abstract class GameRuleDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(GameRuleFileScope s);

    public GameRuleFileBuild Build()
    {
        var scope = new GameRuleFileScope();
        Define(scope);
        return new GameRuleFileBuild(Id, scope.Build());
    }
}

public record GameRuleFileBuild(string Id, IReadOnlyList<GameRuleEntry> Rules);

public class GameRuleFileScope
{
    private readonly List<GameRuleEntry> _rules = [];

    public GameRuleFileScope Add(string id, string defaultOption, Action<GameRuleOptionScope> configure)
    {
        var scope = new GameRuleOptionScope();
        configure(scope);
        _rules.Add(new GameRuleEntry(id, defaultOption, scope.Build()));
        return this;
    }

    internal IReadOnlyList<GameRuleEntry> Build() => _rules.AsReadOnly();
}

public class GameRuleOptionScope
{
    private readonly List<GameRuleOption> _options = [];

    public GameRuleOptionScope Option(string name, string? text = null, string? desc = null, bool? allowAchievements = null)
    {
        _options.Add(new GameRuleOption(name, text, desc, allowAchievements));
        return this;
    }

    internal IReadOnlyList<GameRuleOption> Build() => _options.AsReadOnly();
}

public record GameRuleEntry(string Id, string DefaultOption, IReadOnlyList<GameRuleOption> Options);
public record GameRuleOption(string Name, string? Text, string? Desc, bool? AllowAchievements);

// ─── Terrain ─────────────────────────────────────────────────
// common/terrain/{Id}.txt

public abstract class TerrainDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(TerrainFileScope s);

    public TerrainFileBuild Build()
    {
        var scope = new TerrainFileScope();
        Define(scope);
        return new TerrainFileBuild(Id, scope.Build());
    }
}

public record TerrainFileBuild(string Id, IReadOnlyList<TerrainEntry> Terrains);

public class TerrainFileScope
{
    private readonly List<TerrainEntry> _entries = [];

    public TerrainFileScope Add(string id, Action<TerrainBuilder> configure)
    {
        var builder = new TerrainBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<TerrainEntry> Build() => _entries.AsReadOnly();
}

public class TerrainBuilder
{
    private readonly string _id;
    private int? _color;
    private double? _movementCost;
    private double? _attrition;
    private bool? _isWater;
    private readonly Dictionary<string, double> _modifiers = new();

    public TerrainBuilder(string id) => _id = id;

    public TerrainBuilder Color(int v) { _color = v; return this; }
    public TerrainBuilder MovementCost(double v) { _movementCost = v; return this; }
    public TerrainBuilder Attrition(double v) { _attrition = v; return this; }
    public TerrainBuilder IsWater(bool v) { _isWater = v; return this; }
    public TerrainBuilder Modifier(string key, double val) { _modifiers[key] = val; return this; }

    internal TerrainEntry Build() => new(_id, _color, _movementCost, _attrition, _isWater,
        _modifiers.Count > 0 ? new Dictionary<string, double>(_modifiers) : null);
}

public record TerrainEntry(string Id, int? Color, double? MovementCost, double? Attrition, bool? IsWater,
    Dictionary<string, double>? Modifiers);

// ─── State Categories ────────────────────────────────────────
// common/state_category/{Id}.txt

public abstract class StateCategoryDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(StateCategoryFileScope s);

    public StateCategoryFileBuild Build()
    {
        var scope = new StateCategoryFileScope();
        Define(scope);
        return new StateCategoryFileBuild(Id, scope.Build());
    }
}

public record StateCategoryFileBuild(string Id, IReadOnlyList<StateCategoryEntry> Categories);

public class StateCategoryFileScope
{
    private readonly List<StateCategoryEntry> _entries = [];

    public StateCategoryFileScope Add(string id, int localBuildingSlots, int? color = null)
    {
        _entries.Add(new StateCategoryEntry(id, localBuildingSlots, color));
        return this;
    }

    internal IReadOnlyList<StateCategoryEntry> Build() => _entries.AsReadOnly();
}

public record StateCategoryEntry(string Id, int LocalBuildingSlots, int? Color);

// ─── Resource Definitions ────────────────────────────────────
// common/resources/{Id}.txt

public abstract class ResourceDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(ResourceDefFileScope s);

    public ResourceDefFileBuild Build()
    {
        var scope = new ResourceDefFileScope();
        Define(scope);
        return new ResourceDefFileBuild(Id, scope.Build());
    }
}

public record ResourceDefFileBuild(string Id, IReadOnlyList<ResourceDefEntry> Resources);

public class ResourceDefFileScope
{
    private readonly List<ResourceDefEntry> _entries = [];

    public ResourceDefFileScope Add(string id, int? iconFrame = null, double? cic = null)
    {
        _entries.Add(new ResourceDefEntry(id, iconFrame, cic));
        return this;
    }

    internal IReadOnlyList<ResourceDefEntry> Build() => _entries.AsReadOnly();
}

public record ResourceDefEntry(string Id, int? IconFrame, double? Cic);

// ─── Tech Sharing ────────────────────────────────────────────
// common/technology_sharing/{Id}.txt

public abstract class TechSharingDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(TechSharingFileScope s);

    public TechSharingFileBuild Build()
    {
        var scope = new TechSharingFileScope();
        Define(scope);
        return new TechSharingFileBuild(Id, scope.Build());
    }
}

public record TechSharingFileBuild(string Id, IReadOnlyList<TechSharingGroupEntry> Groups);

public class TechSharingFileScope
{
    private readonly List<TechSharingGroupEntry> _groups = [];

    public TechSharingFileScope Add(string id, Action<TechSharingGroupBuilder> configure)
    {
        var builder = new TechSharingGroupBuilder(id);
        configure(builder);
        _groups.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<TechSharingGroupEntry> Build() => _groups.AsReadOnly();
}

public class TechSharingGroupBuilder
{
    private readonly string _id;
    private readonly List<string> _members = [];
    private string? _category;
    private double? _researchBonus;

    public TechSharingGroupBuilder(string id) => _id = id;

    public TechSharingGroupBuilder Member(string tag) { _members.Add(tag); return this; }
    public TechSharingGroupBuilder Category(string v) { _category = v; return this; }
    public TechSharingGroupBuilder ResearchBonus(double v) { _researchBonus = v; return this; }

    internal TechSharingGroupEntry Build() => new(_id, _members.AsReadOnly(), _category, _researchBonus);
}

public record TechSharingGroupEntry(string Id, IReadOnlyList<string> Members, string? Category, double? ResearchBonus);

// ─── Ideology Definitions ────────────────────────────────────
// common/ideologies/{Id}.txt

public abstract class IdeologyDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(IdeologyFileScope s);

    public IdeologyFileBuild Build()
    {
        var scope = new IdeologyFileScope();
        Define(scope);
        return new IdeologyFileBuild(Id, scope.Build());
    }
}

public record IdeologyFileBuild(string Id, IReadOnlyList<IdeologyEntry> Ideologies);

public class IdeologyFileScope
{
    private readonly List<IdeologyEntry> _entries = [];

    public IdeologyFileScope Add(string id, Action<IdeologyBuilder> configure)
    {
        var builder = new IdeologyBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<IdeologyEntry> Build() => _entries.AsReadOnly();
}

public class IdeologyBuilder
{
    private readonly string _id;
    private (int R, int G, int B)? _color;
    private bool? _canHostGovernmentInExile;
    private bool? _warImpact;
    private bool? _canBeBoosted;
    private readonly List<string> _types = [];
    private readonly Dictionary<string, double> _modifiers = new();

    public IdeologyBuilder(string id) => _id = id;

    public IdeologyBuilder Color(int r, int g, int b) { _color = (r, g, b); return this; }
    public IdeologyBuilder CanHostGovernmentInExile(bool v) { _canHostGovernmentInExile = v; return this; }
    public IdeologyBuilder WarImpact(bool v) { _warImpact = v; return this; }
    public IdeologyBuilder CanBeBoosted(bool v) { _canBeBoosted = v; return this; }
    public IdeologyBuilder SubType(string type) { _types.Add(type); return this; }
    public IdeologyBuilder Modifier(string key, double val) { _modifiers[key] = val; return this; }

    internal IdeologyEntry Build() => new(_id, _color, _canHostGovernmentInExile, _warImpact, _canBeBoosted,
        _types.Count > 0 ? _types.AsReadOnly() : null,
        _modifiers.Count > 0 ? new Dictionary<string, double>(_modifiers) : null);
}

public record IdeologyEntry(
    string Id, (int R, int G, int B)? Color, bool? CanHostGovernmentInExile,
    bool? WarImpact, bool? CanBeBoosted,
    IReadOnlyList<string>? SubTypes, Dictionary<string, double>? Modifiers
);

// ─── Autonomy States ─────────────────────────────────────────
// common/autonomous_states/{Id}.txt

public abstract class AutonomyStateDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(AutonomyStateFileScope s);

    public AutonomyStateFileBuild Build()
    {
        var scope = new AutonomyStateFileScope();
        Define(scope);
        return new AutonomyStateFileBuild(Id, scope.Build());
    }
}

public record AutonomyStateFileBuild(string Id, IReadOnlyList<AutonomyStateEntry> States);

public class AutonomyStateFileScope
{
    private readonly List<AutonomyStateEntry> _entries = [];

    public AutonomyStateFileScope Add(string id, Action<AutonomyStateBuilder> configure)
    {
        var builder = new AutonomyStateBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<AutonomyStateEntry> Build() => _entries.AsReadOnly();
}

public class AutonomyStateBuilder
{
    private readonly string _id;
    private int? _minFreedomLevel;
    private bool? _isSubject;
    private bool? _canForceGovernment;
    private readonly Dictionary<string, double> _modifiers = new();

    public AutonomyStateBuilder(string id) => _id = id;

    public AutonomyStateBuilder MinFreedomLevel(int v) { _minFreedomLevel = v; return this; }
    public AutonomyStateBuilder IsSubject(bool v) { _isSubject = v; return this; }
    public AutonomyStateBuilder CanForceGovernment(bool v) { _canForceGovernment = v; return this; }
    public AutonomyStateBuilder Modifier(string key, double val) { _modifiers[key] = val; return this; }

    internal AutonomyStateEntry Build() => new(_id, _minFreedomLevel, _isSubject, _canForceGovernment,
        _modifiers.Count > 0 ? new Dictionary<string, double>(_modifiers) : null);
}

public record AutonomyStateEntry(string Id, int? MinFreedomLevel, bool? IsSubject, bool? CanForceGovernment,
    Dictionary<string, double>? Modifiers);

// ─── Difficulty Settings ─────────────────────────────────────
// common/difficulty_settings/{Id}.txt

public abstract class DifficultySettingDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(DifficultyFileScope s);

    public DifficultyFileBuild Build()
    {
        var scope = new DifficultyFileScope();
        Define(scope);
        return new DifficultyFileBuild(Id, scope.Build());
    }
}

public record DifficultyFileBuild(string Id, IReadOnlyList<DifficultyEntry> Settings);

public class DifficultyFileScope
{
    private readonly List<DifficultyEntry> _entries = [];

    public DifficultyFileScope Add(string id, string? name = null, bool? isDefault = null,
        Dictionary<string, double>? modifiers = null)
    {
        _entries.Add(new DifficultyEntry(id, name, isDefault, modifiers));
        return this;
    }

    internal IReadOnlyList<DifficultyEntry> Build() => _entries.AsReadOnly();
}

public record DifficultyEntry(string Id, string? Name, bool? IsDefault, Dictionary<string, double>? Modifiers);

// ─── Names (Country/Unit Names) ──────────────────────────────
// common/names/{Id}.txt

public abstract class NameDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(NameFileScope s);

    public NameFileBuild Build()
    {
        var scope = new NameFileScope();
        Define(scope);
        return new NameFileBuild(Id, scope.Build());
    }
}

public record NameFileBuild(string Id, IReadOnlyList<NameGroupEntry> Groups);

public class NameFileScope
{
    private readonly List<NameGroupEntry> _groups = [];

    public NameFileScope Group(string tag, Action<NameGroupBuilder> configure)
    {
        var builder = new NameGroupBuilder(tag);
        configure(builder);
        _groups.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<NameGroupEntry> Build() => _groups.AsReadOnly();
}

public class NameGroupBuilder
{
    private readonly string _tag;
    private readonly Dictionary<string, List<string>> _nameGroups = new();

    public NameGroupBuilder(string tag) => _tag = tag;

    public NameGroupBuilder DivisionNames(string key, params string[] names)
    {
        _nameGroups[key] = [..names];
        return this;
    }

    internal NameGroupEntry Build() => new(_tag,
        _nameGroups.ToDictionary(kv => kv.Key, kv => (IReadOnlyList<string>)kv.Value.AsReadOnly()));
}

public record NameGroupEntry(string Tag, Dictionary<string, IReadOnlyList<string>> NameGroups);
