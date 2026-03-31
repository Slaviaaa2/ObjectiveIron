using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

/// <summary>
/// Test sprite using SourceFile auto-resolve mode.
/// The file at SourceFile is automatically copied to gfx/interface/goals/test_icon.dds
/// </summary>
public class AutoResolvedSprite : SpriteDefinition
{
    public override string Name => "GFX_auto_resolved_test";
    public override string? SourceFile => "test_assets/test_icon.dds";
    // TextureFolder defaults to "gfx/interface/goals/"
    // TextureFile auto-computed: "gfx/interface/goals/test_icon.dds"
}

public class AutoResolvedLeaderPortrait : SpriteDefinition
{
    public override string Name => "GFX_portrait_auto_test";
    public override string? SourceFile => "test_assets/test_icon.dds";
    public override string TextureFolder => "gfx/leaders/EXA/";
    // TextureFile auto-computed: "gfx/leaders/EXA/test_icon.dds"
}
