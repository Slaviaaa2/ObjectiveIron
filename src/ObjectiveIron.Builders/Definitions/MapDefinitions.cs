namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// A raw/static asset to be copied into the mod output directory.
/// Use this for binary files (images, DDS textures, sounds, fonts, etc.)
/// that cannot be generated from C# definitions.
/// </summary>
public record RawAsset(string SourcePath, string TargetRelativePath);

// ─── Province Definition (definition.csv) ──────────────────────

/// <summary>
/// Defines a province entry for map/definition.csv.
/// Each province maps a color on provinces.bmp to game data.
/// </summary>
public record ProvinceDefinition(
    int Id,
    int R, int G, int B,
    ProvinceType Type,
    bool IsCoastal,
    string Terrain,
    string Continent
)
{
    public string ToCsvLine()
        => $"{Id};{R};{G};{B};{Type.ToClausewitz()};{(IsCoastal ? "true" : "false")};{Terrain};{Continent}";
}

public enum ProvinceType
{
    Land,
    Sea,
    Lake
}

public static class ProvinceTypeExtensions
{
    public static string ToClausewitz(this ProvinceType type) => type switch
    {
        ProvinceType.Land => "land",
        ProvinceType.Sea => "sea",
        ProvinceType.Lake => "lake",
        _ => "land"
    };
}

// ─── Adjacency Definition (adjacencies.csv) ────────────────────

/// <summary>
/// Defines an adjacency rule between provinces for map/adjacencies.csv.
/// </summary>
public record AdjacencyDefinition(
    int From,
    int To,
    AdjacencyType Type,
    int Through,
    int? StartX = null,
    int? StartY = null,
    int? StopX = null,
    int? StopY = null,
    string? AdjacencyRuleName = null,
    string? Comment = null
)
{
    public string ToCsvLine()
    {
        var sx = StartX?.ToString() ?? "-1";
        var sy = StartY?.ToString() ?? "-1";
        var ex = StopX?.ToString() ?? "-1";
        var ey = StopY?.ToString() ?? "-1";
        var rule = AdjacencyRuleName ?? "";
        var comment = Comment ?? "";
        return $"{From};{To};{Type.ToClausewitz()};{Through};{sx};{sy};{ex};{ey};{rule};{comment}";
    }
}

public enum AdjacencyType
{
    Sea,
    Land,
    Impassable,
    Canal
}

public static class AdjacencyTypeExtensions
{
    public static string ToClausewitz(this AdjacencyType type) => type switch
    {
        AdjacencyType.Sea => "sea",
        AdjacencyType.Land => "land",
        AdjacencyType.Impassable => "impassable",
        AdjacencyType.Canal => "canal",
        _ => "sea"
    };
}

// ─── default.map ────────────────────────────────────────────────

/// <summary>
/// Defines the contents of map/default.map.
/// </summary>
public abstract class DefaultMapDefinition
{
    public abstract int MaxProvinces { get; }
    public virtual string Definitions => "definition.csv";
    public virtual string Provinces => "provinces.bmp";
    public virtual string Terrain => "terrain.bmp";
    public virtual string Rivers => "rivers.bmp";
    public virtual string Heightmap => "heightmap.bmp";
    public virtual string TreeDefinition => "trees.bmp";
    public virtual string Adjacencies => "adjacencies.csv";

    /// <summary>Sea province IDs.</summary>
    public virtual int[] SeaStarts => [];

    /// <summary>Lake province IDs (only_water_provinces).</summary>
    public virtual int[] LakeStarts => [];

    /// <summary>Additional continent definitions: continent_name → province IDs.</summary>
    protected virtual void Continents(ContinentScope c) { }

    public DefaultMapBuildResult Build()
    {
        var scope = new ContinentScope();
        Continents(scope);
        return new DefaultMapBuildResult(
            MaxProvinces, Definitions, Provinces, Terrain, Rivers,
            Heightmap, TreeDefinition, Adjacencies,
            SeaStarts, LakeStarts, scope.Build()
        );
    }
}

public record DefaultMapBuildResult(
    int MaxProvinces,
    string Definitions,
    string Provinces,
    string Terrain,
    string Rivers,
    string Heightmap,
    string TreeDefinition,
    string Adjacencies,
    int[] SeaStarts,
    int[] LakeStarts,
    IReadOnlyList<ContinentEntry>? Continents
);

public class ContinentScope
{
    private readonly List<ContinentEntry> _entries = [];

    public ContinentScope Add(string name, params int[] provinceIds)
    {
        _entries.Add(new ContinentEntry(name, provinceIds));
        return this;
    }

    internal IReadOnlyList<ContinentEntry>? Build()
        => _entries.Count > 0 ? _entries.AsReadOnly() : null;
}

public record ContinentEntry(string Name, int[] ProvinceIds);
