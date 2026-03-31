namespace ObjectiveIron.Builders.Definitions;

// ─── GUI Definition ──────────────────────────────────────────
// interface/{Id}.gui

public abstract class GuiDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(GuiFileScope s);

    public GuiFileBuild Build()
    {
        var scope = new GuiFileScope();
        Define(scope);
        return new GuiFileBuild(Id, scope.Build());
    }
}

public record GuiFileBuild(string Id, IReadOnlyList<GuiWindowEntry> Windows);

public class GuiFileScope
{
    private readonly List<GuiWindowEntry> _windows = [];

    public GuiFileScope Window(string name, Action<GuiWindowBuilder> configure)
    {
        var builder = new GuiWindowBuilder(name);
        configure(builder);
        _windows.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<GuiWindowEntry> Build() => _windows.AsReadOnly();
}

public class GuiWindowBuilder
{
    private readonly string _name;
    private (int X, int Y)? _position;
    private (int W, int H)? _size;
    private bool? _moveable;
    private string? _orientation;
    private readonly List<GuiElement> _elements = [];

    public GuiWindowBuilder(string name) => _name = name;

    public GuiWindowBuilder Position(int x, int y) { _position = (x, y); return this; }
    public GuiWindowBuilder Size(int w, int h) { _size = (w, h); return this; }
    public GuiWindowBuilder Moveable(bool v) { _moveable = v; return this; }
    public GuiWindowBuilder Orientation(string v) { _orientation = v; return this; }

    public GuiWindowBuilder Button(string name, Action<GuiButtonBuilder> configure)
    {
        var builder = new GuiButtonBuilder(name);
        configure(builder);
        _elements.Add(builder.Build());
        return this;
    }

    public GuiWindowBuilder Icon(string name, Action<GuiIconBuilder> configure)
    {
        var builder = new GuiIconBuilder(name);
        configure(builder);
        _elements.Add(builder.Build());
        return this;
    }

    public GuiWindowBuilder Text(string name, Action<GuiTextBuilder> configure)
    {
        var builder = new GuiTextBuilder(name);
        configure(builder);
        _elements.Add(builder.Build());
        return this;
    }

    public GuiWindowBuilder Background(string name, string sprite)
    {
        _elements.Add(new GuiElement("background", name, sprite, null, null, null, null, null, null, null));
        return this;
    }

    internal GuiWindowEntry Build() => new(_name, _position, _size, _moveable, _orientation, _elements.AsReadOnly());
}

public class GuiButtonBuilder
{
    private readonly string _name;
    private string? _sprite;
    private (int X, int Y)? _position;
    private (int W, int H)? _size;
    private string? _onClick;
    private string? _tooltip;
    private string? _font;

    public GuiButtonBuilder(string name) => _name = name;

    public GuiButtonBuilder Sprite(string v) { _sprite = v; return this; }
    public GuiButtonBuilder Position(int x, int y) { _position = (x, y); return this; }
    public GuiButtonBuilder Size(int w, int h) { _size = (w, h); return this; }
    public GuiButtonBuilder OnClick(string v) { _onClick = v; return this; }
    public GuiButtonBuilder Tooltip(string v) { _tooltip = v; return this; }
    public GuiButtonBuilder Font(string v) { _font = v; return this; }

    internal GuiElement Build() => new("buttonType", _name, _sprite, _position, _size, _onClick, _tooltip, _font, null, null);
}

public class GuiIconBuilder
{
    private readonly string _name;
    private string? _sprite;
    private (int X, int Y)? _position;
    private int? _frame;

    public GuiIconBuilder(string name) => _name = name;

    public GuiIconBuilder Sprite(string v) { _sprite = v; return this; }
    public GuiIconBuilder Position(int x, int y) { _position = (x, y); return this; }
    public GuiIconBuilder Frame(int v) { _frame = v; return this; }

    internal GuiElement Build() => new("iconType", _name, _sprite, _position, null, null, null, null, _frame, null);
}

public class GuiTextBuilder
{
    private readonly string _name;
    private string? _text;
    private (int X, int Y)? _position;
    private (int W, int H)? _size;
    private string? _font;
    private string? _format;

    public GuiTextBuilder(string name) => _name = name;

    public GuiTextBuilder Text(string v) { _text = v; return this; }
    public GuiTextBuilder Position(int x, int y) { _position = (x, y); return this; }
    public GuiTextBuilder Size(int w, int h) { _size = (w, h); return this; }
    public GuiTextBuilder Font(string v) { _font = v; return this; }
    public GuiTextBuilder Format(string v) { _format = v; return this; }

    internal GuiElement Build() => new("instantTextBoxType", _name, null, _position, _size, null, null, _font, null, _text);
}

public record GuiWindowEntry(
    string Name,
    (int X, int Y)? Position,
    (int W, int H)? Size,
    bool? Moveable,
    string? Orientation,
    IReadOnlyList<GuiElement> Elements
);

public record GuiElement(
    string ElementType, string Name, string? Sprite,
    (int X, int Y)? Position, (int W, int H)? Size,
    string? OnClick, string? Tooltip, string? Font,
    int? Frame, string? Text
);

// ─── Music Definition ────────────────────────────────────────
// music/{Id}_songs.txt

public abstract class MusicDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(MusicFileScope s);

    public MusicFileBuild Build()
    {
        var scope = new MusicFileScope();
        Define(scope);
        return new MusicFileBuild(Id, scope.Build());
    }
}

public record MusicFileBuild(string Id, IReadOnlyList<MusicEntry> Songs);

public class MusicFileScope
{
    private readonly List<MusicEntry> _songs = [];

    public MusicFileScope Song(string name, string file, double? chance = null, bool? alwaysMusic = null)
    {
        _songs.Add(new MusicEntry(name, file, chance, alwaysMusic));
        return this;
    }

    internal IReadOnlyList<MusicEntry> Build() => _songs.AsReadOnly();
}

public record MusicEntry(string Name, string File, double? Chance, bool? AlwaysMusic);

// ─── Sound Definition ────────────────────────────────────────
// sound/{Id}.asset

public abstract class SoundDefinition
{
    public abstract string Id { get; }
    protected abstract void Define(SoundFileScope s);

    public SoundFileBuild Build()
    {
        var scope = new SoundFileScope();
        Define(scope);
        return new SoundFileBuild(Id, scope.Build());
    }
}

public record SoundFileBuild(string Id, IReadOnlyList<SoundEntry> Sounds);

public class SoundFileScope
{
    private readonly List<SoundEntry> _sounds = [];

    public SoundFileScope Effect(string name, string file, double? volume = null)
    {
        _sounds.Add(new SoundEntry(name, file, volume));
        return this;
    }

    internal IReadOnlyList<SoundEntry> Build() => _sounds.AsReadOnly();
}

public record SoundEntry(string Name, string File, double? Volume);

// ─── Map Definition ──────────────────────────────────────────
// map/{type}.txt - strategic regions, supply areas, etc.

public abstract class StrategicRegionDefinition
{
    public abstract string Id { get; }
    public abstract int RegionId { get; }
    public abstract string RegionName { get; }
    public abstract int[] Provinces { get; }
    public virtual string? Weather => null;

    public StrategicRegionBuild Build() => new(Id, RegionId, RegionName, Provinces, Weather);
}

public record StrategicRegionBuild(string Id, int RegionId, string RegionName, int[] Provinces, string? Weather);

public abstract class SupplyAreaDefinition
{
    public abstract string Id { get; }
    public abstract int AreaId { get; }
    public abstract string AreaName { get; }
    public abstract int Value { get; }
    public abstract int[] States { get; }

    public SupplyAreaBuild Build() => new(Id, AreaId, AreaName, Value, States);
}

public record SupplyAreaBuild(string Id, int AreaId, string AreaName, int Value, int[] States);
