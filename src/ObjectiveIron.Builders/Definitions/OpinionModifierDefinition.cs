using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Base class for defining opinion modifiers.
/// </summary>
public abstract class OpinionModifierDefinition
{
    public abstract string Id { get; }

    protected abstract void Define(OpinionModifierScope o);

    public IReadOnlyList<OpinionModifier> Build()
    {
        var scope = new OpinionModifierScope();
        Define(scope);
        return scope.Build();
    }
}

/// <summary>
/// Scope for adding opinion modifier entries.
/// </summary>
public class OpinionModifierScope
{
    private readonly List<OpinionModifier> _modifiers = [];

    public OpinionModifierScope Add(
        string id,
        int value,
        double? decay = null,
        int? minTrust = null,
        int? maxTrust = null,
        int? days = null,
        int? months = null,
        int? years = null,
        bool? trade = null)
    {
        _modifiers.Add(new OpinionModifier(id, value, decay, minTrust, maxTrust, days, months, years, trade));
        return this;
    }

    internal IReadOnlyList<OpinionModifier> Build() => _modifiers.AsReadOnly();
}
