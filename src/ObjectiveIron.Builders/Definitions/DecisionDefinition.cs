using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// A single decision that the player can take.
/// Must specify a Category ID.
/// </summary>
public abstract class DecisionDefinition
{
    public abstract string Id { get; }
    public abstract LocalizedText Name { get; }
    public virtual LocalizedText? Description => null;
    
    /// <summary>
    /// The category this decision belongs to. Must match a Category ID.
    /// </summary>
    public abstract string Category { get; }

    public virtual GfxSprite? Icon => null;

    /// <summary>Cost in Political Power.</summary>
    public virtual int? Cost => null;

    /// <summary>Days until removal/timeout.</summary>
    public virtual int? DaysToRemove => null;

    public virtual bool VisibleByDefault => true;

    // Optional Blocks
    protected virtual void Visible(TriggerScope t) { }
    protected virtual void Available(TriggerScope t) { }
    protected virtual void CompleteEffect(EffectScope e) { }
    protected virtual void TimeoutEffect(EffectScope e) { }
    protected virtual void RemoveEffect(EffectScope e) { }
    protected virtual void AiWillDo(AiScope ai) { }
    protected virtual void Modifier(ModifierScope m) { }

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

    public Block? CompileVisible()
    {
        var scope = new TriggerScope();
        Visible(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    public Block? CompileAvailable()
    {
        var scope = new TriggerScope();
        Available(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    public Block? CompileComplete()
    {
        var scope = new EffectScope();
        CompleteEffect(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    public Block? CompileAi()
    {
        var scope = new AiScope();
        AiWillDo(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    public Block? CompileTimeout()
    {
        var scope = new EffectScope();
        TimeoutEffect(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }
}
