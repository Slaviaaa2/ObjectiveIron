namespace ObjectiveIron.Builders.Types;

/// <summary>
/// Position on the focus tree grid.
/// </summary>
public sealed record FocusPosition(int X, int Y)
{
    public static readonly FocusPosition Origin = new(0, 0);
}
