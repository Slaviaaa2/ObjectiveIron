using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines an Order of Battle (OOB) file for a country.
/// Emits to: history/units/{FileName}.txt
/// </summary>
public abstract class OobDefinition
{
    /// <summary>OOB filename (e.g., "EXA_1936"). No extension.</summary>
    public abstract string FileName { get; }

    /// <summary>Define division templates.</summary>
    protected virtual void Templates(TemplateScope t) { }

    /// <summary>Define deployed units.</summary>
    protected virtual void Units(UnitScope u) { }

    /// <summary>Define instant effects (equipment production, etc.).</summary>
    protected virtual void InstantEffect(EffectScope e) { }

    public OobBuildResult Build()
    {
        var templates = new TemplateScope();
        Templates(templates);

        var units = new UnitScope();
        Units(units);

        var instant = new EffectScope();
        InstantEffect(instant);
        var instantBlock = instant.Build();

        return new OobBuildResult(
            FileName,
            templates.Build(),
            units.Build(),
            instantBlock.Entries.Count > 0 ? instantBlock : null
        );
    }
}

public record OobBuildResult(
    string FileName,
    IReadOnlyList<DivisionTemplateBuild>? Templates,
    IReadOnlyList<DeployedDivisionBuild>? Divisions,
    Block? InstantEffect
);

// ─── Template Scope ─────────────────────────────────────────────

public class TemplateScope
{
    private readonly List<DivisionTemplateBuild> _templates = [];

    public TemplateScope Add(string name, Action<DivisionTemplateBuilder> configure)
    {
        var builder = new DivisionTemplateBuilder(name);
        configure(builder);
        _templates.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<DivisionTemplateBuild>? Build()
        => _templates.Count > 0 ? _templates.AsReadOnly() : null;
}

public class DivisionTemplateBuilder
{
    private readonly string _name;
    private string? _namesGroup;
    private readonly List<RegimentEntry> _regiments = [];
    private readonly List<RegimentEntry> _support = [];

    public DivisionTemplateBuilder(string name) => _name = name;

    public DivisionTemplateBuilder DivisionNamesGroup(string group)
    {
        _namesGroup = group;
        return this;
    }

    public DivisionTemplateBuilder Regiment(string type, int x, int y)
    {
        _regiments.Add(new RegimentEntry(type, x, y));
        return this;
    }

    public DivisionTemplateBuilder Support(string type, int x, int y)
    {
        _support.Add(new RegimentEntry(type, x, y));
        return this;
    }

    internal DivisionTemplateBuild Build() => new(
        _name,
        _namesGroup,
        _regiments.Count > 0 ? _regiments.AsReadOnly() : null,
        _support.Count > 0 ? _support.AsReadOnly() : null
    );
}

public record DivisionTemplateBuild(
    string Name,
    string? DivisionNamesGroup,
    IReadOnlyList<RegimentEntry>? Regiments,
    IReadOnlyList<RegimentEntry>? Support
);

public record RegimentEntry(string Type, int X, int Y);

// ─── Unit Scope ─────────────────────────────────────────────────

public class UnitScope
{
    private readonly List<DeployedDivisionBuild> _divisions = [];

    public UnitScope Division(string template, int location,
        double? experience = null, int? nameOrder = null)
    {
        _divisions.Add(new DeployedDivisionBuild(template, location, experience, nameOrder));
        return this;
    }

    internal IReadOnlyList<DeployedDivisionBuild>? Build()
        => _divisions.Count > 0 ? _divisions.AsReadOnly() : null;
}

public record DeployedDivisionBuild(
    string Template,
    int Location,
    double? StartExperienceFactor,
    int? NameOrder
);

// ─── Naval OOB Definition ──────────────────────────────────────

/// <summary>
/// Defines a naval Order of Battle. Emits to: history/units/{FileName}.txt
/// </summary>
public abstract class NavalOobDefinition
{
    public abstract string FileName { get; }
    protected abstract void Fleets(NavalFleetScope f);

    public NavalOobBuildResult Build()
    {
        var scope = new NavalFleetScope();
        Fleets(scope);
        return new NavalOobBuildResult(FileName, scope.Build());
    }
}

public record NavalOobBuildResult(string FileName, IReadOnlyList<FleetBuild>? Fleets);

public class NavalFleetScope
{
    private readonly List<FleetBuild> _fleets = [];

    public NavalFleetScope Fleet(string name, int navalBase, Action<FleetBuilder> configure)
    {
        var builder = new FleetBuilder(name, navalBase);
        configure(builder);
        _fleets.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<FleetBuild>? Build()
        => _fleets.Count > 0 ? _fleets.AsReadOnly() : null;
}

public class FleetBuilder
{
    private readonly string _name;
    private readonly int _navalBase;
    private readonly List<TaskForceBuild> _taskForces = [];

    public FleetBuilder(string name, int navalBase)
    {
        _name = name;
        _navalBase = navalBase;
    }

    public FleetBuilder TaskForce(string name, int location, Action<TaskForceBuilder> configure)
    {
        var builder = new TaskForceBuilder(name, location);
        configure(builder);
        _taskForces.Add(builder.Build());
        return this;
    }

    internal FleetBuild Build() => new(_name, _navalBase, _taskForces.Count > 0 ? _taskForces.AsReadOnly() : null);
}

public record FleetBuild(string Name, int NavalBase, IReadOnlyList<TaskForceBuild>? TaskForces);

public class TaskForceBuilder
{
    private readonly string _name;
    private readonly int _location;
    private readonly List<ShipEntry> _ships = [];

    public TaskForceBuilder(string name, int location)
    {
        _name = name;
        _location = location;
    }

    public TaskForceBuilder Ship(string name, string definition, string equipment, string version)
    {
        _ships.Add(new ShipEntry(name, definition, equipment, version, null));
        return this;
    }

    public TaskForceBuilder Ship(string name, string definition, string equipment, string version, double? experience)
    {
        _ships.Add(new ShipEntry(name, definition, equipment, version, experience));
        return this;
    }

    internal TaskForceBuild Build() => new(_name, _location, _ships.Count > 0 ? _ships.AsReadOnly() : null);
}

public record TaskForceBuild(string Name, int Location, IReadOnlyList<ShipEntry>? Ships);
public record ShipEntry(string Name, string Definition, string Equipment, string Version, double? Experience);

// ─── Air OOB Definition ────────────────────────────────────────

/// <summary>
/// Defines an air Order of Battle. Emits to: history/units/{FileName}.txt
/// </summary>
public abstract class AirOobDefinition
{
    public abstract string FileName { get; }
    protected abstract void AirWings(AirWingScope a);

    public AirOobBuildResult Build()
    {
        var scope = new AirWingScope();
        AirWings(scope);
        return new AirOobBuildResult(FileName, scope.Build());
    }
}

public record AirOobBuildResult(string FileName, IReadOnlyList<AirWingBuild>? Wings);

public class AirWingScope
{
    private readonly List<AirWingBuild> _wings = [];

    public AirWingScope Wing(int stateId, Action<AirWingBuilder> configure)
    {
        var builder = new AirWingBuilder(stateId);
        configure(builder);
        _wings.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<AirWingBuild>? Build()
        => _wings.Count > 0 ? _wings.AsReadOnly() : null;
}

public class AirWingBuilder
{
    private readonly int _stateId;
    private readonly List<AirWingEntry> _entries = [];

    public AirWingBuilder(int stateId) => _stateId = stateId;

    public AirWingBuilder Add(string type, string equipment, int amount, string? name = null)
    {
        _entries.Add(new AirWingEntry(type, equipment, amount, name));
        return this;
    }

    internal AirWingBuild Build() => new(_stateId, _entries.Count > 0 ? _entries.AsReadOnly() : null);
}

public record AirWingBuild(int StateId, IReadOnlyList<AirWingEntry>? Entries);
public record AirWingEntry(string Type, string Equipment, int Amount, string? Name);
