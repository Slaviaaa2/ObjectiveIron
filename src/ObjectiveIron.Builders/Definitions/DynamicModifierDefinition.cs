using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Base class for defining dynamic modifiers.
/// Dynamic modifiers can be added/removed at runtime and support variable-driven values.
/// </summary>
public abstract class DynamicModifierDefinition
{
    public abstract string Id { get; }
    public virtual string? Icon => null;
    public virtual bool AttackerModifier => false;

    protected virtual void Enable(TriggerScope t) { }
    protected virtual void RemoveTrigger(TriggerScope t) { }
    protected abstract void Modifiers(ModifierScope m);

    public DynamicModifierBuildResult Build()
    {
        var enable = new TriggerScope();
        Enable(enable);
        var enableBlock = enable.Build();

        var remove = new TriggerScope();
        RemoveTrigger(remove);
        var removeBlock = remove.Build();

        var mod = new ModifierScope();
        Modifiers(mod);
        var modBlock = mod.Build();

        return new DynamicModifierBuildResult(
            Id,
            Icon,
            AttackerModifier,
            enableBlock.Entries.Count > 0 ? enableBlock : null,
            removeBlock.Entries.Count > 0 ? removeBlock : null,
            modBlock
        );
    }
}

public record DynamicModifierBuildResult(
    string Id,
    string? Icon,
    bool AttackerModifier,
    Block? Enable,
    Block? RemoveTrigger,
    Block Modifiers
);
