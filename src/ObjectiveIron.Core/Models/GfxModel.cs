namespace ObjectiveIron.Core.Models;

/// <summary>
/// AST model for a spriteType entry in a .gfx file.
/// </summary>
public sealed class Sprite
{
    public required string Name { get; init; }
    public required string TextureFile { get; init; }
    public int? NoOfFrames { get; init; }
    public bool? LegacyLazyLoad { get; init; }
}

/// <summary>
/// AST model for a complete .gfx file containing multiple sprites.
/// </summary>
public sealed class GfxFile
{
    public required string Id { get; init; }
    public required List<Sprite> Sprites { get; init; }
}
