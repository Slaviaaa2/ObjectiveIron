using ObjectiveIron.Builders.Types;

namespace ObjectiveIron.Builders;

/// <summary>
/// A builder for defining dynamic icon triggers.
/// </summary>
public sealed class DynamicGfxScope
{
    private readonly List<(Action<TriggerScope>? Trigger, GfxSprite Icon)> _entries = [];
    private bool _hasDefault;

    /// <summary>
    /// Display the given icon if the trigger condition is met.
    /// Triggers are evaluated from top to bottom.
    /// </summary>
    public DynamicGfxScope If(Action<TriggerScope> condition, GfxSprite icon)
    {
        if (_hasDefault)
            throw new InvalidOperationException("Cannot add 'If' after 'Default' has been called.");
            
        _entries.Add((condition, icon));
        return this;
    }

    /// <summary>
    /// Display this icon if no previous conditions were met.
    /// This should be the last method called.
    /// </summary>
    public void Default(GfxSprite icon)
    {
        if (_hasDefault)
            throw new InvalidOperationException("Default has already been called.");
            
        _hasDefault = true;
        _entries.Add((null, icon));
    }

    internal IReadOnlyList<(Action<TriggerScope>? Trigger, GfxSprite Icon)> Build()
    {
        return _entries.AsReadOnly();
    }
}
