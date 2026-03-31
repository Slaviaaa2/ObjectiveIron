using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders;

/// <summary>
/// Scope for defining AI weight/factor blocks.
/// Used in AiWillDo override on FocusDefinition.
/// </summary>
public sealed class AiScope
{
    private readonly List<BlockEntry> _entries = [];

    public AiScope Base(int value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property("base", new PropertyValue.IntValue(value))));
        return this;
    }

    public AiScope Base(double value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property("base", new PropertyValue.FloatValue(value))));
        return this;
    }

    public AiScope Factor(int value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property("factor", new PropertyValue.IntValue(value))));
        return this;
    }

    public AiScope Factor(double value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property("factor", new PropertyValue.FloatValue(value))));
        return this;
    }

    public AiScope Modifier(int factor, Action<TriggerScope> trigger)
    {
        var triggerScope = new TriggerScope();
        trigger(triggerScope);
        var block = triggerScope.Build();

        var entries = new List<BlockEntry>
        {
            new BlockEntry.PropertyEntry(new Property("factor", new PropertyValue.IntValue(factor)))
        };
        entries.AddRange(block.Entries);

        _entries.Add(new BlockEntry.NestedBlock("modifier", new Block(entries)));
        return this;
    }

    public AiScope Modifier(double factor, Action<TriggerScope> trigger)
    {
        var triggerScope = new TriggerScope();
        trigger(triggerScope);
        var block = triggerScope.Build();

        var entries = new List<BlockEntry>
        {
            new BlockEntry.PropertyEntry(new Property("factor", new PropertyValue.FloatValue(factor)))
        };
        entries.AddRange(block.Entries);

        _entries.Add(new BlockEntry.NestedBlock("modifier", new Block(entries)));
        return this;
    }

    internal Block Build() => new(_entries);
}
