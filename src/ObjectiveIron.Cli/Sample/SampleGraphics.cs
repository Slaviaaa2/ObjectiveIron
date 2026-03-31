using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

// Mode 1: Manual path (existing pattern — you copy the file yourself)
public class CustomFocusIcon : SpriteDefinition
{
    public override string Name => "GFX_custom_focus_icon";
    public override string TextureFile => "gfx/interface/goals/custom_focus_icon.dds";
}

// Mode 1: Manual path with event picture folder
public class CustomEventPicture : SpriteDefinition
{
    public override string Name => "GFX_custom_event_picture";
    public override string TextureFile => "gfx/interface/event_pictures/custom_event_picture.dds";
    public override bool? LegacyLazyLoad => false;
}

// Mode 1: Manual path for leader portrait
public class MikhailGromanPortrait : SpriteDefinition
{
    public override string Name => "GFX_portrait_soviet_mikhail_groman";
    public override string TextureFile => "gfx/leaders/SOV/portrait_soviet_mikhail_groman.dds";
}

// Mode 2: Auto-resolve — specify SourceFile and the texture is automatically
// copied to the correct output path. TextureFile is computed from TextureFolder + filename.
//
// Usage:
//   public class MyIcon : SpriteDefinition
//   {
//       public override string Name => "GFX_my_focus_icon";
//       public override string? SourceFile => "./assets/my_icon.dds";             // local file
//       public override string TextureFolder => "gfx/interface/goals/";           // (default)
//       // TextureFile automatically becomes "gfx/interface/goals/my_icon.dds"
//       // The file is auto-copied to {output}/gfx/interface/goals/my_icon.dds
//   }
//
//   public class MyLeaderPortrait : SpriteDefinition
//   {
//       public override string Name => "GFX_portrait_my_leader";
//       public override string? SourceFile => "./assets/portraits/my_leader.dds";
//       public override string TextureFolder => "gfx/leaders/EXA/";
//       // TextureFile = "gfx/leaders/EXA/my_leader.dds" (auto)
//   }
