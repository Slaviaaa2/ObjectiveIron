namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines a battalion/company sub-unit type.
/// Emits to: common/units/{FileName}.txt
/// </summary>
public abstract class SubUnitDefinition
{
    /// <summary>Output filename (without extension).</summary>
    public virtual string FileName => Id;

    /// <summary>Sub-unit type ID (e.g., "infantry", "light_armor").</summary>
    public abstract string Id { get; }

    /// <summary>Define sub-unit entries in this file.</summary>
    protected abstract void Define(SubUnitScope s);

    public SubUnitFileBuildResult Build()
    {
        var scope = new SubUnitScope();
        Define(scope);
        return new SubUnitFileBuildResult(FileName, scope.Build());
    }
}

public record SubUnitFileBuildResult(
    string FileName,
    IReadOnlyList<SubUnitEntry> Units
);

public class SubUnitScope
{
    private readonly List<SubUnitEntry> _entries = [];

    public SubUnitScope Add(string id, Action<SubUnitBuilder> configure)
    {
        var builder = new SubUnitBuilder(id);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<SubUnitEntry> Build() => _entries.AsReadOnly();
}

public class SubUnitBuilder
{
    private readonly string _id;
    private string? _abbreviation;
    private string? _sprite;
    private string? _mapIconCategory;
    private int? _priority;
    private int? _aiPriority;
    private bool? _active;
    private readonly List<string> _types = [];
    private string? _group;
    private readonly List<string> _categories = [];
    private double? _combatWidth;
    private double? _maxStrength;
    private double? _maxOrganisation;
    private double? _defaultMorale;
    private int? _manpower;
    private int? _trainingTime;
    private double? _suppression;
    private double? _weight;
    private double? _supplyConsumption;
    private double? _breakthrough;
    private double? _defense;
    private double? _softAttack;
    private double? _hardAttack;
    private double? _entrenchment;
    private double? _maximumSpeed;
    private bool? _affectsSpeed;
    private bool? _isArtilleryBrigade;
    private bool? _specialForces;
    private bool? _canBeParachuted;
    private readonly List<string> _essential = [];
    private readonly Dictionary<string, int> _need = new();
    private readonly List<TerrainModifierEntry> _terrainModifiers = [];
    private readonly List<BattalionMultEntry> _battalionMults = [];

    public SubUnitBuilder(string id) => _id = id;

    public SubUnitBuilder Abbreviation(string v) { _abbreviation = v; return this; }
    public SubUnitBuilder Sprite(string v) { _sprite = v; return this; }
    public SubUnitBuilder MapIconCategory(string v) { _mapIconCategory = v; return this; }
    public SubUnitBuilder Priority(int v) { _priority = v; return this; }
    public SubUnitBuilder AiPriority(int v) { _aiPriority = v; return this; }
    public SubUnitBuilder Active(bool v) { _active = v; return this; }
    public SubUnitBuilder Type(params string[] types) { _types.AddRange(types); return this; }
    public SubUnitBuilder Group(string v) { _group = v; return this; }
    public SubUnitBuilder Categories(params string[] cats) { _categories.AddRange(cats); return this; }
    public SubUnitBuilder CombatWidth(double v) { _combatWidth = v; return this; }
    public SubUnitBuilder MaxStrength(double v) { _maxStrength = v; return this; }
    public SubUnitBuilder MaxOrganisation(double v) { _maxOrganisation = v; return this; }
    public SubUnitBuilder DefaultMorale(double v) { _defaultMorale = v; return this; }
    public SubUnitBuilder Manpower(int v) { _manpower = v; return this; }
    public SubUnitBuilder TrainingTime(int v) { _trainingTime = v; return this; }
    public SubUnitBuilder Suppression(double v) { _suppression = v; return this; }
    public SubUnitBuilder Weight(double v) { _weight = v; return this; }
    public SubUnitBuilder SupplyConsumption(double v) { _supplyConsumption = v; return this; }
    public SubUnitBuilder Breakthrough(double v) { _breakthrough = v; return this; }
    public SubUnitBuilder Defense(double v) { _defense = v; return this; }
    public SubUnitBuilder SoftAttack(double v) { _softAttack = v; return this; }
    public SubUnitBuilder HardAttack(double v) { _hardAttack = v; return this; }
    public SubUnitBuilder Entrenchment(double v) { _entrenchment = v; return this; }
    public SubUnitBuilder MaximumSpeed(double v) { _maximumSpeed = v; return this; }
    public SubUnitBuilder AffectsSpeed(bool v) { _affectsSpeed = v; return this; }
    public SubUnitBuilder IsArtilleryBrigade(bool v) { _isArtilleryBrigade = v; return this; }
    public SubUnitBuilder SpecialForces(bool v) { _specialForces = v; return this; }
    public SubUnitBuilder CanBeParachuted(bool v) { _canBeParachuted = v; return this; }
    public SubUnitBuilder Essential(params string[] equipment) { _essential.AddRange(equipment); return this; }
    public SubUnitBuilder Need(string equipment, int amount) { _need[equipment] = amount; return this; }

    public SubUnitBuilder Terrain(string terrain, double? attack = null, double? defence = null, double? movement = null)
    {
        _terrainModifiers.Add(new TerrainModifierEntry(terrain, attack, defence, movement));
        return this;
    }

    public SubUnitBuilder BattalionMult(string category, double value, string property, bool add = false)
    {
        _battalionMults.Add(new BattalionMultEntry(category, property, value, add));
        return this;
    }

    internal SubUnitEntry Build() => new(
        _id, _abbreviation, _sprite, _mapIconCategory, _priority, _aiPriority, _active,
        _types.Count > 0 ? _types.AsReadOnly() : null,
        _group,
        _categories.Count > 0 ? _categories.AsReadOnly() : null,
        _combatWidth, _maxStrength, _maxOrganisation, _defaultMorale,
        _manpower, _trainingTime, _suppression, _weight, _supplyConsumption,
        _breakthrough, _defense, _softAttack, _hardAttack, _entrenchment,
        _maximumSpeed, _affectsSpeed, _isArtilleryBrigade, _specialForces, _canBeParachuted,
        _essential.Count > 0 ? _essential.AsReadOnly() : null,
        _need.Count > 0 ? new Dictionary<string, int>(_need) : null,
        _terrainModifiers.Count > 0 ? _terrainModifiers.AsReadOnly() : null,
        _battalionMults.Count > 0 ? _battalionMults.AsReadOnly() : null
    );
}

public record SubUnitEntry(
    string Id,
    string? Abbreviation,
    string? Sprite,
    string? MapIconCategory,
    int? Priority,
    int? AiPriority,
    bool? Active,
    IReadOnlyList<string>? Types,
    string? Group,
    IReadOnlyList<string>? Categories,
    double? CombatWidth,
    double? MaxStrength,
    double? MaxOrganisation,
    double? DefaultMorale,
    int? Manpower,
    int? TrainingTime,
    double? Suppression,
    double? Weight,
    double? SupplyConsumption,
    double? Breakthrough,
    double? Defense,
    double? SoftAttack,
    double? HardAttack,
    double? Entrenchment,
    double? MaximumSpeed,
    bool? AffectsSpeed,
    bool? IsArtilleryBrigade,
    bool? SpecialForces,
    bool? CanBeParachuted,
    IReadOnlyList<string>? Essential,
    Dictionary<string, int>? Need,
    IReadOnlyList<TerrainModifierEntry>? TerrainModifiers,
    IReadOnlyList<BattalionMultEntry>? BattalionMults
);

public record TerrainModifierEntry(string Terrain, double? Attack, double? Defence, double? Movement);
public record BattalionMultEntry(string Category, string Property, double Value, bool Add);
