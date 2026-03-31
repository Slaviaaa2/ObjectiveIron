using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Base class for defining a Paradox technology.
/// Supports paths, categories, equipment/subunit unlocks, and unit-category modifiers.
/// </summary>
public abstract class TechnologyDefinition
{
    public abstract string Id { get; }
    public virtual LocalizedText? Name => null;
    public virtual LocalizedText? Description => null;
    public virtual int? StartYear => null;
    public virtual double? ResearchCost => null;
    public virtual string? Folder => null;
    public virtual (int X, int Y)? Position => null;

    protected virtual void Allow(TriggerScope t) { }
    protected virtual void AiWillDo(AiScope ai) { }
    protected virtual void Modifier(ModifierScope m) { }
    protected virtual void EnableEquipment(EquipmentScope e) { }
    protected virtual void Prerequisites(PrerequisiteScope p) { }
    protected virtual void Paths(PathScope p) { }
    protected virtual void Categories(CategoryScope c) { }
    protected virtual void UnitModifiers(UnitCategoryModifierScope u) { }
    protected virtual void EnableSubunits(SubunitScope s) { }
    protected virtual void EnableEquipmentModules(EquipmentModuleScope m) { }

    public Technology Build()
    {
        var allow = new TriggerScope();
        Allow(allow);
        var allowBlock = allow.Build();

        var aiWillDo = new AiScope();
        AiWillDo(aiWillDo);
        var aiBlock = aiWillDo.Build();

        var modifier = new ModifierScope();
        Modifier(modifier);
        var modBlock = modifier.Build();

        var equip = new EquipmentScope();
        EnableEquipment(equip);

        var prereqs = new PrerequisiteScope();
        Prerequisites(prereqs);

        var paths = new PathScope();
        Paths(paths);

        var cats = new CategoryScope();
        Categories(cats);

        var unitMods = new UnitCategoryModifierScope();
        UnitModifiers(unitMods);

        var subunits = new SubunitScope();
        EnableSubunits(subunits);

        var modules = new EquipmentModuleScope();
        EnableEquipmentModules(modules);

        return new Technology(
            Id,
            StartYear,
            ResearchCost,
            Folder,
            Position.HasValue ? new TechnologyPosition(Position.Value.X, Position.Value.Y) : null,
            prereqs.Build(),
            equip.Build(),
            subunits.Build(),
            modules.Build(),
            paths.Build(),
            cats.Build(),
            unitMods.Build(),
            allowBlock.Entries.Count > 0 ? allowBlock : null,
            aiBlock.Entries.Count > 0 ? aiBlock : null,
            modBlock.Entries.Count > 0 ? modBlock : null
        );
    }
}

// ─── Scopes ──────────────────────────────────────────────────────

public class EquipmentScope
{
    private readonly List<EquipmentUnlock> _unlocks = [];
    public void Unlock(string equipmentId) => _unlocks.Add(new EquipmentUnlock(equipmentId));
    internal IReadOnlyList<EquipmentUnlock>? Build() => _unlocks.Count > 0 ? _unlocks.AsReadOnly() : null;
}

public class PrerequisiteScope
{
    private readonly List<string> _prereqs = [];
    public void Require(string techId) => _prereqs.Add(techId);
    internal IReadOnlyList<string>? Build() => _prereqs.Count > 0 ? _prereqs.AsReadOnly() : null;
}

public class PathScope
{
    private readonly List<TechnologyPath> _paths = [];
    public void LeadsTo(string techId, double costCoeff = 1.0) => _paths.Add(new TechnologyPath(techId, costCoeff));
    internal IReadOnlyList<TechnologyPath>? Build() => _paths.Count > 0 ? _paths.AsReadOnly() : null;
}

public class CategoryScope
{
    private readonly List<string> _categories = [];
    public void Add(string category) => _categories.Add(category);
    internal IReadOnlyList<string>? Build() => _categories.Count > 0 ? _categories.AsReadOnly() : null;
}

public class SubunitScope
{
    private readonly List<string> _subunits = [];
    public void Enable(string subunit) => _subunits.Add(subunit);
    internal IReadOnlyList<string>? Build() => _subunits.Count > 0 ? _subunits.AsReadOnly() : null;
}

public class EquipmentModuleScope
{
    private readonly List<string> _modules = [];
    public void Enable(string module) => _modules.Add(module);
    internal IReadOnlyList<string>? Build() => _modules.Count > 0 ? _modules.AsReadOnly() : null;
}

public class UnitCategoryModifierScope
{
    private readonly List<UnitCategoryModifier> _modifiers = [];

    public void ForCategory(string category, Action<ModifierScope> configure)
    {
        var scope = new ModifierScope();
        configure(scope);
        _modifiers.Add(new UnitCategoryModifier(category, scope.Build()));
    }

    internal IReadOnlyList<UnitCategoryModifier>? Build() => _modifiers.Count > 0 ? _modifiers.AsReadOnly() : null;
}
