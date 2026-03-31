namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines equipment types and variants.
/// Emits to: common/units/equipment/{FileName}.txt
/// </summary>
public abstract class EquipmentDefinition
{
    /// <summary>Output filename (without extension).</summary>
    public virtual string FileName => Id;

    /// <summary>Equipment definition set ID.</summary>
    public abstract string Id { get; }

    /// <summary>Define equipment entries.</summary>
    protected abstract void Define(EquipmentSetScope s);

    public EquipmentFileBuildResult Build()
    {
        var scope = new EquipmentSetScope();
        Define(scope);
        return new EquipmentFileBuildResult(FileName, scope.Build());
    }
}

public record EquipmentFileBuildResult(
    string FileName,
    IReadOnlyList<EquipmentEntry> Equipment
);

public class EquipmentSetScope
{
    private readonly List<EquipmentEntry> _entries = [];

    /// <summary>Define an archetype (base equipment definition).</summary>
    public EquipmentSetScope Archetype(string id, Action<EquipmentBuilder> configure)
    {
        var builder = new EquipmentBuilder(id, isArchetype: true);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    /// <summary>Define an equipment variant inheriting from an archetype.</summary>
    public EquipmentSetScope Variant(string id, string archetype, Action<EquipmentBuilder> configure)
    {
        var builder = new EquipmentBuilder(id, isArchetype: false);
        builder.SetArchetype(archetype);
        configure(builder);
        _entries.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<EquipmentEntry> Build() => _entries.AsReadOnly();
}

public class EquipmentBuilder
{
    private readonly string _id;
    private readonly bool _isArchetype;
    private string? _archetype;
    private string? _parent;
    private int? _year;
    private bool? _isBuildable;
    private bool? _active;
    private string? _picture;
    private readonly List<string> _types = [];
    private string? _groupBy;
    private string? _interfaceCategory;
    private int? _priority;
    private int? _visualLevel;

    // Combat stats
    private double? _reliability;
    private double? _maximumSpeed;
    private double? _defense;
    private double? _breakthrough;
    private double? _hardness;
    private double? _armorValue;
    private double? _softAttack;
    private double? _hardAttack;
    private double? _apAttack;
    private double? _airAttack;

    // Logistics
    private double? _lendLeaseCost;
    private double? _buildCostIc;
    private double? _fuelConsumption;
    private readonly Dictionary<string, int> _resources = new();

    public EquipmentBuilder(string id, bool isArchetype)
    {
        _id = id;
        _isArchetype = isArchetype;
    }

    internal void SetArchetype(string archetype) => _archetype = archetype;

    public EquipmentBuilder Year(int v) { _year = v; return this; }
    public EquipmentBuilder Parent(string v) { _parent = v; return this; }
    public EquipmentBuilder IsBuildable(bool v) { _isBuildable = v; return this; }
    public EquipmentBuilder Active(bool v) { _active = v; return this; }
    public EquipmentBuilder Picture(string v) { _picture = v; return this; }
    public EquipmentBuilder Type(params string[] types) { _types.AddRange(types); return this; }
    public EquipmentBuilder GroupBy(string v) { _groupBy = v; return this; }
    public EquipmentBuilder InterfaceCategory(string v) { _interfaceCategory = v; return this; }
    public EquipmentBuilder Priority(int v) { _priority = v; return this; }
    public EquipmentBuilder VisualLevel(int v) { _visualLevel = v; return this; }

    public EquipmentBuilder Reliability(double v) { _reliability = v; return this; }
    public EquipmentBuilder MaximumSpeed(double v) { _maximumSpeed = v; return this; }
    public EquipmentBuilder Defense(double v) { _defense = v; return this; }
    public EquipmentBuilder Breakthrough(double v) { _breakthrough = v; return this; }
    public EquipmentBuilder Hardness(double v) { _hardness = v; return this; }
    public EquipmentBuilder ArmorValue(double v) { _armorValue = v; return this; }
    public EquipmentBuilder SoftAttack(double v) { _softAttack = v; return this; }
    public EquipmentBuilder HardAttack(double v) { _hardAttack = v; return this; }
    public EquipmentBuilder ApAttack(double v) { _apAttack = v; return this; }
    public EquipmentBuilder AirAttack(double v) { _airAttack = v; return this; }
    public EquipmentBuilder LendLeaseCost(double v) { _lendLeaseCost = v; return this; }
    public EquipmentBuilder BuildCostIc(double v) { _buildCostIc = v; return this; }
    public EquipmentBuilder FuelConsumption(double v) { _fuelConsumption = v; return this; }

    public EquipmentBuilder Resource(string type, int amount)
    {
        _resources[type] = amount;
        return this;
    }

    public EquipmentBuilder Steel(int v) => Resource("steel", v);
    public EquipmentBuilder Tungsten(int v) => Resource("tungsten", v);
    public EquipmentBuilder Chromium(int v) => Resource("chromium", v);
    public EquipmentBuilder Rubber(int v) => Resource("rubber", v);
    public EquipmentBuilder Aluminium(int v) => Resource("aluminium", v);

    internal EquipmentEntry Build() => new(
        _id, _isArchetype, _archetype, _parent, _year, _isBuildable, _active,
        _picture,
        _types.Count > 0 ? _types.AsReadOnly() : null,
        _groupBy, _interfaceCategory, _priority, _visualLevel,
        _reliability, _maximumSpeed, _defense, _breakthrough, _hardness, _armorValue,
        _softAttack, _hardAttack, _apAttack, _airAttack,
        _lendLeaseCost, _buildCostIc, _fuelConsumption,
        _resources.Count > 0 ? new Dictionary<string, int>(_resources) : null
    );
}

public record EquipmentEntry(
    string Id,
    bool IsArchetype,
    string? Archetype,
    string? Parent,
    int? Year,
    bool? IsBuildable,
    bool? Active,
    string? Picture,
    IReadOnlyList<string>? Types,
    string? GroupBy,
    string? InterfaceCategory,
    int? Priority,
    int? VisualLevel,
    double? Reliability,
    double? MaximumSpeed,
    double? Defense,
    double? Breakthrough,
    double? Hardness,
    double? ArmorValue,
    double? SoftAttack,
    double? HardAttack,
    double? ApAttack,
    double? AirAttack,
    double? LendLeaseCost,
    double? BuildCostIc,
    double? FuelConsumption,
    Dictionary<string, int>? Resources
);
