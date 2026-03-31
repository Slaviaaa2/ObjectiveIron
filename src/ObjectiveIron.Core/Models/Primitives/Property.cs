namespace ObjectiveIron.Core.Models.Primitives;

/// <summary>
/// Represents a single key = value pair in Clausewitz script.
/// Value can be a string, number, boolean (yes/no), or another block reference.
/// </summary>
public sealed record Property(string Key, PropertyValue Value, Operator Operator = Operator.Equals);

/// <summary>
/// A polymorphic value that can represent different Clausewitz value types.
/// </summary>
public abstract record PropertyValue
{
    public sealed record StringValue(string Value) : PropertyValue;
    public sealed record QuotedStringValue(string Value) : PropertyValue;
    public sealed record IntValue(int Value) : PropertyValue;
    public sealed record FloatValue(double Value) : PropertyValue;
    public sealed record BoolValue(bool Value) : PropertyValue;
    public sealed record BlockRef(Block Block) : PropertyValue;

    public string ToClausewitz() => this switch
    {
        StringValue s => s.Value,
        QuotedStringValue q => $"\"{q.Value}\"",
        IntValue i => i.Value.ToString(),
        FloatValue f => f.Value.ToString("0.####"),
        BoolValue b => b.Value ? "yes" : "no",
        BlockRef => throw new InvalidOperationException("Block references cannot be inlined as values."),
        _ => throw new InvalidOperationException($"Unknown PropertyValue type: {GetType().Name}")
    };
}
