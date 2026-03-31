using System.Text;
using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits map-related files: definition.csv, adjacencies.csv, default.map
/// </summary>
public static class MapEmitter
{
    public static string EmitDefinitionCsv(
        IEnumerable<ProvinceDefinition> provinces,
        string outputBasePath)
    {
        var mapDir = Path.Combine(outputBasePath, "map");
        Directory.CreateDirectory(mapDir);

        var filePath = Path.Combine(mapDir, "definition.csv");
        using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

        // Header
        writer.WriteLine("province;red;green;blue;type;coastal;terrain;continent");

        foreach (var p in provinces)
            writer.WriteLine(p.ToCsvLine());

        return filePath;
    }

    public static string EmitAdjacenciesCsv(
        IEnumerable<AdjacencyDefinition> adjacencies,
        string outputBasePath)
    {
        var mapDir = Path.Combine(outputBasePath, "map");
        Directory.CreateDirectory(mapDir);

        var filePath = Path.Combine(mapDir, "adjacencies.csv");
        using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

        // Header
        writer.WriteLine("From;To;Type;Through;start_x;start_y;stop_x;stop_y;adjacency_rule_name;Comment");

        foreach (var a in adjacencies)
            writer.WriteLine(a.ToCsvLine());

        // HoI4 requires trailing line
        writer.WriteLine("-1;-1;;-1;-1;-1;-1;-1;-1;");

        return filePath;
    }

    public static string EmitDefaultMap(
        DefaultMapBuildResult map,
        string outputBasePath)
    {
        var mapDir = Path.Combine(outputBasePath, "map");
        Directory.CreateDirectory(mapDir);

        var filePath = Path.Combine(mapDir, "default.map");
        using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

        writer.WriteLine($"max_provinces = {map.MaxProvinces}");
        writer.WriteLine($"definitions = \"{map.Definitions}\"");
        writer.WriteLine($"provinces = \"{map.Provinces}\"");
        writer.WriteLine($"terrain = \"{map.Terrain}\"");
        writer.WriteLine($"rivers = \"{map.Rivers}\"");
        writer.WriteLine($"heightmap = \"{map.Heightmap}\"");
        writer.WriteLine($"tree_definition = \"{map.TreeDefinition}\"");
        writer.WriteLine($"adjacencies = \"{map.Adjacencies}\"");
        writer.WriteLine();

        if (map.SeaStarts.Length > 0)
        {
            writer.WriteLine("sea_starts = {");
            writer.Write("\t");
            writer.WriteLine(string.Join(" ", map.SeaStarts));
            writer.WriteLine("}");
            writer.WriteLine();
        }

        if (map.LakeStarts.Length > 0)
        {
            writer.WriteLine("only_water_provinces = {");
            writer.Write("\t");
            writer.WriteLine(string.Join(" ", map.LakeStarts));
            writer.WriteLine("}");
            writer.WriteLine();
        }

        if (map.Continents is { Count: > 0 })
        {
            foreach (var continent in map.Continents)
            {
                writer.WriteLine($"{continent.Name} = {{");
                writer.Write("\t");
                writer.WriteLine(string.Join(" ", continent.ProvinceIds));
                writer.WriteLine("}");
                writer.WriteLine();
            }
        }

        return filePath;
    }
}
