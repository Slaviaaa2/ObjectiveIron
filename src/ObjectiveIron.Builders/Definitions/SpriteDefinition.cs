namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Definition for a single interface sprite/icon.
///
/// Two modes of operation:
/// 1. Manual path: Override TextureFile with the mod-relative path (e.g., "gfx/interface/goals/icon.dds").
///    You must copy the file yourself via AddRawAsset().
/// 2. Auto-resolve: Override SourceFile with a local file path (e.g., "./assets/icon.dds")
///    and optionally TextureFolder (defaults to "gfx/interface/goals/").
///    TextureFile is auto-computed. The file is automatically copied on emit.
/// </summary>
public abstract class SpriteDefinition
{
    public abstract string Name { get; }

    /// <summary>
    /// Mod-relative texture path (e.g., "gfx/interface/goals/icon.dds").
    /// If SourceFile is set, this is auto-computed from TextureFolder + filename.
    /// </summary>
    public virtual string TextureFile =>
        SourceFile != null
            ? TextureFolder.TrimEnd('/') + "/" + System.IO.Path.GetFileName(SourceFile)
            : throw new System.InvalidOperationException(
                $"SpriteDefinition '{Name}': either TextureFile or SourceFile must be overridden.");

    /// <summary>
    /// Local filesystem path to the source image file (e.g., "./assets/my_icon.dds").
    /// When set, the file is automatically copied to the correct output location on emit.
    /// </summary>
    public virtual string? SourceFile => null;

    /// <summary>
    /// Target folder within the mod for the texture (e.g., "gfx/interface/goals/").
    /// Only used when SourceFile is set. Defaults to "gfx/interface/goals/".
    /// </summary>
    public virtual string TextureFolder => "gfx/interface/goals/";

    public virtual int? NoOfFrames => null;
    public virtual bool? LegacyLazyLoad => null;
}
