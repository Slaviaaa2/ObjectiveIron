namespace ObjectiveIron.Builders.Types;

/// <summary>
/// Typed country tag. Prevents raw string typos and enables IDE autocompletion.
/// </summary>
public sealed record CountryTag(string Value)
{
    public static implicit operator CountryTag(string s) => new(s);

    public override string ToString() => Value;
}
