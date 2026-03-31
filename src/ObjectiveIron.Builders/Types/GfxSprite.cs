namespace ObjectiveIron.Builders.Types;

/// <summary>
/// Typed GFX sprite reference. Can be pre-defined constants or user-created.
/// </summary>
public sealed record GfxSprite(string Value)
{
    public static implicit operator GfxSprite(string s) => new(s);

    public override string ToString() => Value;
}
