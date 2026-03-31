using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Base class for defining a single national focus.
/// Inherit this class and override properties/methods to define your focus.
/// 
/// <example>
/// <code>
/// public class IndustrialEffort : FocusDefinition
/// {
///     public override string Id => "GER_industrial_effort";
///     public override GfxSprite Icon => Icons.GenericProduction;
///     public override FocusPosition Position => new(1, 0);
///
///     protected override void CompletionReward(EffectScope e)
///     {
///         e.AddBuildingConstruction(BuildingType.IndustrialComplex, 2, instantBuild: true);
///         e.AddPoliticalPower(50);
///     }
/// }
/// </code>
/// </example>
/// </summary>
public abstract class FocusDefinition
{
    /// <summary>Unique identifier for this focus.</summary>
    public abstract string Id { get; }

    /// <summary>GFX sprite icon for this focus. Null = no icon.</summary>
    public virtual GfxSprite? Icon => null;

    /// <summary>Position on the focus tree grid. Can be overridden by Layout.</summary>
    public virtual FocusPosition Position => FocusPosition.Origin;

    /// <summary>Cost in weeks (each = 7 days). Default: 10 (70 days).</summary>
    public virtual int Cost => 10;

    /// <summary>Whether to cancel the focus if conditions become invalid.</summary>
    public virtual bool CancelIfInvalid => false;

    /// <summary>Whether this focus can be taken even if capitulated.</summary>
    public virtual bool AvailableIfCapitulated => false;

    /// <summary>Search filter tags for the focus tree search.</summary>
    public virtual IReadOnlyList<string> SearchFilters => [];

    // ─── Localisation ─────────────────────────────────────────────

    /// <summary>
    /// Display name of this focus. Supports multiple languages.
    /// If null, no localisation entry is generated (use raw ID in-game).
    /// Can also be set with a plain string for English-only: Title => "My Focus";
    /// </summary>
    public virtual LocalizedText? Title => null;

    /// <summary>
    /// Description shown when hovering this focus. Supports multiple languages.
    /// Supports Dynamic Localisation tokens like [Root.GetName].
    /// </summary>
    public virtual LocalizedText? Description => null;

    // ─── Dynamic Icons & Text ──────────────────────────────────────

    /// <summary>Evaluate logic to display a dynamic icon over the default Icon.</summary>
    protected virtual void DynamicIcon(DynamicGfxScope i) { }

    /// <summary>Evaluate logic to display dynamic title over the default Title.</summary>
    protected virtual void DynamicTitle(DynamicLocScope l) { }

    /// <summary>Evaluate logic to display dynamic desc over the default Description.</summary>
    protected virtual void DynamicDescription(DynamicLocScope l) { }

    // ─── Override these to define triggers/effects ─────────────────

    /// <summary>Conditions for this focus to be selectable.</summary>
    protected virtual void Available(TriggerScope t) { }

    /// <summary>Conditions to auto-bypass this focus.</summary>
    protected virtual void Bypass(TriggerScope t) { }

    /// <summary>Effects executed when this focus is completed.</summary>
    protected virtual void CompletionReward(EffectScope e) { }

    /// <summary>Effects executed when this focus is selected.</summary>
    protected virtual void SelectEffect(EffectScope e) { }

    /// <summary>Effects executed when this focus is cancelled.</summary>
    protected virtual void CancelEffect(EffectScope e) { }

    /// <summary>AI weight configuration.</summary>
    protected virtual void AiWillDo(AiScope ai) { }

    // ─── Internal: Compile methods (called by ModCompiler) ─────────

    internal Block? CompileAvailable()
    {
        var scope = new TriggerScope();
        Available(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    internal Block? CompileBypass()
    {
        var scope = new TriggerScope();
        Bypass(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    internal Block? CompileCompletionReward()
    {
        var scope = new EffectScope();
        CompletionReward(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    internal Block? CompileSelectEffect()
    {
        var scope = new EffectScope();
        SelectEffect(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    internal Block? CompileCancelEffect()
    {
        var scope = new EffectScope();
        CancelEffect(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    internal Block? CompileAiWillDo()
    {
        var scope = new AiScope();
        AiWillDo(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    public IReadOnlyList<(Action<TriggerScope>? Trigger, GfxSprite Icon)> CompileDynamicIcon()
    {
        var scope = new DynamicGfxScope();
        DynamicIcon(scope);
        return scope.Build();
    }

    public IReadOnlyList<(Action<TriggerScope>? Trigger, LocalizedText Text)> CompileDynamicTitle()
    {
        var scope = new DynamicLocScope();
        DynamicTitle(scope);
        return scope.Build();
    }

    public IReadOnlyList<(Action<TriggerScope>? Trigger, LocalizedText Text)> CompileDynamicDescription()
    {
        var scope = new DynamicLocScope();
        DynamicDescription(scope);
        return scope.Build();
    }
}
