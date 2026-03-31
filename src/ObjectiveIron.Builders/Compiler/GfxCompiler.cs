using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Builders.Compiler;

public static class GfxCompiler
{
    public static Sprite Compile(SpriteDefinition def)
    {
        return new Sprite
        {
            Name = def.Name,
            TextureFile = def.TextureFile,
            NoOfFrames = def.NoOfFrames,
            LegacyLazyLoad = def.LegacyLazyLoad
        };
    }
}
