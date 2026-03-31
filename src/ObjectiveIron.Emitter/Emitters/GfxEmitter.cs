using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Emitter.Emitters;

public static class GfxEmitter
{
    public static void Emit(GfxFile gfxFile, ClausewitzWriter writer)
    {
        writer.BeginBlock("spriteTypes");
        
        foreach (var sprite in gfxFile.Sprites)
        {
            writer.BeginBlock("spriteType");
            
            writer.WriteProperty(new Property("name", new PropertyValue.QuotedStringValue(sprite.Name)));
            writer.WriteProperty(new Property("texturefile", new PropertyValue.QuotedStringValue(sprite.TextureFile)));
            
            if (sprite.NoOfFrames.HasValue)
                writer.WriteProperty(new Property("noOfFrames", new PropertyValue.IntValue(sprite.NoOfFrames.Value)));
                
            if (sprite.LegacyLazyLoad.HasValue)
                writer.WriteProperty(new Property("legacy_lazy_load", new PropertyValue.BoolValue(sprite.LegacyLazyLoad.Value)));
            
            writer.EndBlock();
        }
        
        writer.EndBlock();
    }
}
