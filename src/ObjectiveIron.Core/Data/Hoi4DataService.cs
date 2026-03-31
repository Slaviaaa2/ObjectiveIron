using System.Collections.Concurrent;

namespace ObjectiveIron.Core.Data;

/// <summary>
/// Service to extract and cache game data from HOI4 install directory.
/// Used for validation and code completion suggestions.
/// </summary>
public class Hoi4DataService
{
    private readonly string _gamePath;
    private readonly ConcurrentDictionary<string, HashSet<string>> _cache = new();

    public Hoi4DataService(string gamePath)
    {
        _gamePath = gamePath;
    }

    public HashSet<string> GetBuildings() => GetOrLoad("buildings", "common/buildings");
    public HashSet<string> GetIdeologies() => GetOrLoad("ideologies", "common/ideologies");
    public HashSet<string> GetUnitTraits() => GetOrLoad("unit_traits", "common/unit_leader");
    public HashSet<string> GetCountryLeaderTraits() => GetOrLoad("country_leader_traits", "common/country_leader");
    
    private HashSet<string> GetOrLoad(string key, string subPath)
    {
        if (_cache.TryGetValue(key, out var cached)) return cached;

        var fullPath = Path.Combine(_gamePath, subPath);
        if (!Directory.Exists(fullPath)) return new HashSet<string>();

        var ids = new HashSet<string>();
        var files = Directory.GetFiles(fullPath, "*.txt", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            ExtractTopLevelKeys(content, ids);
        }

        _cache[key] = ids;
        return ids;
    }

    private static readonly HashSet<string> WrapperBlocks = new() 
    { 
        "buildings", "leader_traits", "ideologies", "ideas", "scripted_triggers", "scripted_effects" 
    };

    private void ExtractTopLevelKeys(string content, HashSet<string> ids)
    {
        var tokens = ClausewitzLexer.Tokenize(content).GetEnumerator();
        int braceLevel = 0;
        string? currentKey = null;
        string? parentKey = null;

        while (tokens.MoveNext())
        {
            var token = tokens.Current!;

            if (token == "{")
            {
                if (braceLevel == 0) parentKey = currentKey;
                braceLevel++;
                currentKey = null;
                continue;
            }
            if (token == "}")
            {
                braceLevel--;
                if (braceLevel == 0) parentKey = null;
                continue;
            }

            if (token == "=") continue;

            if (braceLevel == 0)
            {
                currentKey = token;
                ids.Add(token); 
            }
            else if (braceLevel == 1 && parentKey != null && WrapperBlocks.Contains(parentKey))
            {
                // If we are inside a wrapper block, the items inside are the real IDs
                ids.Add(token);
            }
        }
    }
}
