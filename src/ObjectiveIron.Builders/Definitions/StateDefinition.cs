using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines a map state with provinces, buildings, and ownership.
/// Emits to: history/states/{Id}-{Name}.txt
/// </summary>
public abstract class StateDefinition
{
    /// <summary>State numeric ID.</summary>
    public abstract int Id { get; }

    /// <summary>State name identifier (used for file name and STATE_{Id} loc key).</summary>
    public abstract string StateName { get; }

    /// <summary>Localized display name for this state.</summary>
    public virtual LocalizedText? DisplayName => null;

    /// <summary>Population/manpower value.</summary>
    public abstract int Manpower { get; }

    /// <summary>State category (e.g., "town", "city", "rural", "megalopolis").</summary>
    public virtual string StateCategory => "rural";

    /// <summary>Province IDs in this state.</summary>
    public abstract int[] Provinces { get; }

    /// <summary>Local supplies ratio.</summary>
    public virtual double? LocalSupplies => null;

    /// <summary>Max building level factor override.</summary>
    public virtual double? BuildingsMaxLevelFactor => null;

    /// <summary>Whether this state is impassable terrain.</summary>
    public virtual bool Impassable => false;

    /// <summary>Resources in this state (e.g., oil, steel, etc.).</summary>
    protected virtual void Resources(ResourceScope r) { }

    /// <summary>Define state history (owner, cores, VP, buildings).</summary>
    protected abstract void History(StateHistoryScope h);

    public StateBuildResult Build()
    {
        var resources = new ResourceScope();
        Resources(resources);

        var history = new StateHistoryScope();
        History(history);

        return new StateBuildResult(
            Id,
            StateName,
            Manpower,
            StateCategory,
            Provinces,
            Impassable,
            LocalSupplies,
            BuildingsMaxLevelFactor,
            resources.Build(),
            history.Build()
        );
    }
}

public record StateBuildResult(
    int Id,
    string StateName,
    int Manpower,
    string StateCategory,
    int[] Provinces,
    bool Impassable,
    double? LocalSupplies,
    double? BuildingsMaxLevelFactor,
    IReadOnlyList<ResourceEntry>? Resources,
    StateHistoryBuildResult History
);

// ─── Resource Scope ─────────────────────────────────────────────

public class ResourceScope
{
    private readonly List<ResourceEntry> _entries = [];

    public ResourceScope Oil(int amount) => Add("oil", amount);
    public ResourceScope Aluminium(int amount) => Add("aluminium", amount);
    public ResourceScope Rubber(int amount) => Add("rubber", amount);
    public ResourceScope Tungsten(int amount) => Add("tungsten", amount);
    public ResourceScope Steel(int amount) => Add("steel", amount);
    public ResourceScope Chromium(int amount) => Add("chromium", amount);

    public ResourceScope Add(string resource, int amount)
    {
        _entries.Add(new ResourceEntry(resource, amount));
        return this;
    }

    internal IReadOnlyList<ResourceEntry>? Build()
        => _entries.Count > 0 ? _entries.AsReadOnly() : null;
}

public record ResourceEntry(string Resource, int Amount);

// ─── State History Scope ────────────────────────────────────────

public class StateHistoryScope
{
    private string? _owner;
    private string? _controller;
    private readonly List<string> _cores = [];
    private readonly List<string> _claims = [];
    private readonly List<VictoryPointEntry> _victoryPoints = [];
    private readonly List<StateBuildingEntry> _buildings = [];
    private readonly List<ProvinceBuildingEntry> _provinceBuildings = [];
    private readonly List<DateHistoryEntry> _dateEntries = [];
    private readonly List<EffectEntry> _effects = [];

    public StateHistoryScope Owner(string tag)
    {
        _owner = tag;
        return this;
    }

    public StateHistoryScope Controller(string tag)
    {
        _controller = tag;
        return this;
    }

    /// <summary>Add arbitrary effects to the history block.</summary>
    public StateHistoryScope Effect(Action<EffectScope> configure)
    {
        var scope = new EffectScope();
        configure(scope);
        _effects.Add(new EffectEntry(scope.Build()));
        return this;
    }

    public StateHistoryScope AddCoreOf(string tag)
    {
        _cores.Add(tag);
        return this;
    }

    public StateHistoryScope AddClaimBy(string tag)
    {
        _claims.Add(tag);
        return this;
    }

    public StateHistoryScope VictoryPoints(int provinceId, int value)
    {
        _victoryPoints.Add(new VictoryPointEntry(provinceId, value));
        return this;
    }

    public StateHistoryScope Building(string type, int level)
    {
        _buildings.Add(new StateBuildingEntry(type, level));
        return this;
    }

    public StateHistoryScope Infrastructure(int level) => Building("infrastructure", level);
    public StateHistoryScope ArmsFactory(int count) => Building("arms_factory", count);
    public StateHistoryScope IndustrialComplex(int count) => Building("industrial_complex", count);
    public StateHistoryScope AirBase(int level) => Building("air_base", level);
    public StateHistoryScope AntiAir(int level) => Building("anti_air_building", level);
    public StateHistoryScope Dockyard(int count) => Building("dockyard", count);
    public StateHistoryScope SyntheticRefinery(int count) => Building("synthetic_refinery", count);
    public StateHistoryScope FuelSilo(int count) => Building("fuel_silo", count);
    public StateHistoryScope Radar(int level) => Building("radar_station", level);
    public StateHistoryScope RocketSite(int level) => Building("rocket_site", level);
    public StateHistoryScope NuclearReactor(int level) => Building("nuclear_reactor", level);

    public StateHistoryScope ProvinceBuilding(int provinceId, string type, int level)
    {
        _provinceBuildings.Add(new ProvinceBuildingEntry(provinceId, type, level));
        return this;
    }

    public StateHistoryScope NavalBase(int provinceId, int level)
        => ProvinceBuilding(provinceId, "naval_base", level);

    public StateHistoryScope SupplyNode(int provinceId, int level)
        => ProvinceBuilding(provinceId, "supply_node", level);

    public StateHistoryScope DateEntry(string date, Action<EffectScope> configure)
    {
        var scope = new EffectScope();
        configure(scope);
        _dateEntries.Add(new DateHistoryEntry(date, scope.Build()));
        return this;
    }

    internal StateHistoryBuildResult Build()
    {
        return new StateHistoryBuildResult(
            _owner,
            _controller,
            _cores.Count > 0 ? _cores.AsReadOnly() : null,
            _claims.Count > 0 ? _claims.AsReadOnly() : null,
            _victoryPoints.Count > 0 ? _victoryPoints.AsReadOnly() : null,
            _buildings.Count > 0 ? _buildings.AsReadOnly() : null,
            _provinceBuildings.Count > 0 ? _provinceBuildings.AsReadOnly() : null,
            _dateEntries.Count > 0 ? _dateEntries.AsReadOnly() : null,
            _effects.Count > 0 ? _effects.AsReadOnly() : null
        );
    }
}

public record StateHistoryBuildResult(
    string? Owner,
    string? Controller,
    IReadOnlyList<string>? Cores,
    IReadOnlyList<string>? Claims,
    IReadOnlyList<VictoryPointEntry>? VictoryPoints,
    IReadOnlyList<StateBuildingEntry>? Buildings,
    IReadOnlyList<ProvinceBuildingEntry>? ProvinceBuildings,
    IReadOnlyList<DateHistoryEntry>? DateEntries,
    IReadOnlyList<EffectEntry>? Effects
);

public record VictoryPointEntry(int ProvinceId, int Value);
public record StateBuildingEntry(string Type, int Level);
public record ProvinceBuildingEntry(int ProvinceId, string Type, int Level);
public record EffectEntry(Block EffectBlock);

