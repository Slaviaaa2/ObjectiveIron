using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders;

/// <summary>
/// A builder for defining dynamic localisation text triggers.
/// </summary>
public sealed class DynamicLocScope
{
    private readonly List<(Action<TriggerScope>? Trigger, LocalizedText Text)> _entries = [];
    private bool _hasDefault;

    /// <summary>
    /// Use the provided localised text if the trigger condition is met.
    /// Triggers are evaluated from top to bottom.
    /// </summary>
    public DynamicLocScope If(Action<TriggerScope> condition, LocalizedText text)
    {
        if (_hasDefault)
            throw new InvalidOperationException("Cannot add 'If' after 'Default' has been called.");
            
        _entries.Add((condition, text));
        return this;
    }

    /// <summary>
    /// Use this text if no previous conditions were met.
    /// This should be the last method called.
    /// </summary>
    public void Default(LocalizedText text)
    {
        if (_hasDefault)
            throw new InvalidOperationException("Default has already been called.");
            
        _hasDefault = true;
        _entries.Add((null, text));
    }

    internal IReadOnlyList<(Action<TriggerScope>? Trigger, LocalizedText Text)> Build()
    {
        return _entries.AsReadOnly();
    }
}
