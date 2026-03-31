using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class GuiEmitter
{
    public static void Emit(GuiFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("guiTypes");
        foreach (var window in file.Windows)
        {
            writer.BeginBlock("containerWindowType");
            writer.WriteProperty("name", $"\"{window.Name}\"");
            if (window.Position.HasValue)
            {
                writer.BeginBlock("position");
                writer.WriteProperty("x", window.Position.Value.X);
                writer.WriteProperty("y", window.Position.Value.Y);
                writer.EndBlock();
            }
            if (window.Size.HasValue)
            {
                writer.BeginBlock("size");
                writer.WriteProperty("width", window.Size.Value.W);
                writer.WriteProperty("height", window.Size.Value.H);
                writer.EndBlock();
            }
            if (window.Moveable.HasValue) writer.WriteProperty("moveable", window.Moveable.Value);
            if (window.Orientation != null) writer.WriteProperty("orientation", window.Orientation);

            foreach (var elem in window.Elements)
            {
                writer.BeginBlock(elem.ElementType);
                writer.WriteProperty("name", $"\"{elem.Name}\"");

                if (elem.Sprite != null)
                {
                    var spriteKey = elem.ElementType == "background" ? "quadTextureSprite" : "spriteType";
                    writer.WriteProperty(spriteKey, $"\"{elem.Sprite}\"");
                }

                if (elem.Position.HasValue)
                {
                    writer.BeginBlock("position");
                    writer.WriteProperty("x", elem.Position.Value.X);
                    writer.WriteProperty("y", elem.Position.Value.Y);
                    writer.EndBlock();
                }

                if (elem.Size.HasValue)
                {
                    writer.WriteProperty("maxWidth", elem.Size.Value.W);
                    writer.WriteProperty("maxHeight", elem.Size.Value.H);
                }

                if (elem.Font != null) writer.WriteProperty("font", $"\"{elem.Font}\"");
                if (elem.OnClick != null) writer.WriteProperty("pdx_tooltip", $"\"{elem.OnClick}\"");
                if (elem.Tooltip != null) writer.WriteProperty("pdx_tooltip", $"\"{elem.Tooltip}\"");
                if (elem.Frame.HasValue) writer.WriteProperty("frame", elem.Frame.Value);
                if (elem.Text != null) writer.WriteProperty("text", $"\"{elem.Text}\"");

                writer.EndBlock();
            }

            writer.EndBlock(); // containerWindowType
            writer.WriteBlankLine();
        }
        writer.EndBlock(); // guiTypes
    }
}

public static class MusicEmitter
{
    public static void Emit(MusicFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("music_station");
        writer.WriteProperty("name", $"\"{file.Id}_station\"");

        foreach (var song in file.Songs)
        {
            writer.BeginBlock("music");
            writer.WriteProperty("song", $"\"{song.File}\"");
            if (song.Chance.HasValue) writer.WriteProperty("chance", song.Chance.Value);
            if (song.AlwaysMusic.HasValue) writer.WriteProperty("always", song.AlwaysMusic.Value);
            writer.EndBlock();
        }

        writer.EndBlock();
    }
}

public static class SoundEmitter
{
    public static void Emit(SoundFileBuild file, StreamWriter writer)
    {
        // .asset format is different from Clausewitz
        foreach (var sound in file.Sounds)
        {
            writer.WriteLine("soundeffect = {");
            writer.WriteLine($"\tname = \"{sound.Name}\"");
            writer.WriteLine($"\tsounds = {{");
            writer.WriteLine($"\t\tsound = {{ file = \"{sound.File}\" }}");
            writer.WriteLine($"\t}}");
            if (sound.Volume.HasValue)
                writer.WriteLine($"\tvolume = {sound.Volume.Value}");
            writer.WriteLine("}");
            writer.WriteLine();
        }
    }
}

public static class StrategicRegionEmitter
{
    public static void Emit(StrategicRegionBuild region, ClausewitzWriter writer)
    {
        writer.BeginBlock("strategic_region");
        writer.WriteProperty("id", region.RegionId);
        writer.WriteProperty("name", $"\"{region.RegionName}\"");
        writer.BeginBlock("provinces");
        writer.WriteUnquoted(string.Join(" ", region.Provinces));
        writer.EndBlock();
        if (region.Weather != null)
        {
            writer.BeginBlock("weather");
            writer.WriteUnquoted(region.Weather);
            writer.EndBlock();
        }
        writer.EndBlock();
    }
}

public static class SupplyAreaEmitter
{
    public static void Emit(SupplyAreaBuild area, ClausewitzWriter writer)
    {
        writer.BeginBlock("supply_area");
        writer.WriteProperty("id", area.AreaId);
        writer.WriteProperty("name", $"\"{area.AreaName}\"");
        writer.WriteProperty("value", area.Value);
        writer.BeginBlock("states");
        writer.WriteUnquoted(string.Join(" ", area.States));
        writer.EndBlock();
        writer.EndBlock();
    }
}
