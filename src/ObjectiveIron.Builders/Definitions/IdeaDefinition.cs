using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// A reusable national spirit definition.
/// Emission: /common/ideas/*.txt
/// </summary>
public abstract class IdeaDefinition
{
    public abstract string Id { get; }
    
    public virtual LocalizedText Name { get; } = new();
    public virtual LocalizedText Description { get; } = new();
    
    /// <summary>Sprite name (e.g., GFX_idea_my_spirit).</summary>
    public virtual string? Picture { get; }
    
    /// <summary>Category in /common/ideas/ (usually national_spirit).</summary>
    public virtual string Category => "national_spirit";

    protected virtual void Modifier(ModifierScope m) { }
    
    protected virtual void EquipmentBonus(ModifierScope m) { }
    
    protected virtual void Rule(RuleScope r) { }
    
    protected virtual void DynamicName(DynamicLocScope l) { }
    protected virtual void DynamicDescription(DynamicLocScope l) { }

    public IReadOnlyList<(Action<TriggerScope>? Trigger, LocalizedText Text)> CompileDynamicName()
    {
        var scope = new DynamicLocScope();
        DynamicName(scope);
        return scope.Build();
    }

    public IReadOnlyList<(Action<TriggerScope>? Trigger, LocalizedText Text)> CompileDynamicDescription()
    {
        var scope = new DynamicLocScope();
        DynamicDescription(scope);
        return scope.Build();
    }

    public Block CompileModifier()
    {
        var scope = new ModifierScope();
        Modifier(scope);
        return scope.Build();
    }
}

/// <summary>
/// Simple rule scope for ideas (e.g., can_not_join_factions).
/// </summary>
public sealed class RuleScope
{
    private readonly List<BlockEntry> _entries = [];
    
    public RuleScope CanNotJoinFactions(bool value = true)
        => AddBool("can_not_join_factions", value);
        
    public RuleScope CanCreateFactions(bool value = true)
        => AddBool("can_create_factions", value);

    internal Block Build() => new(_entries);

    private RuleScope AddBool(string key, bool value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.BoolValue(value))));
        return this;
    }
}
