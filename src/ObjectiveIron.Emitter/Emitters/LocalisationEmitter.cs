using System.Text;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Generates HoI4 localisation .yml files from FocusDefinition, EventDefinition, and DecisionDefinitions.
/// Produces one file per language in the correct folder structure.
/// </summary>
public static class LocalisationEmitter
{
    /// <summary>
    /// Collect all localisation entries and emit .yml files per language.
    /// </summary>
    public static List<string> Emit(
        string modId,
        IEnumerable<FocusDefinition> focuses,
        IEnumerable<EventDefinition> events,
        IEnumerable<DecisionCategoryDefinition> decisionCategories,
        IEnumerable<DecisionDefinition> decisions,
        IEnumerable<IdeaDefinition> ideas,
        IEnumerable<CharacterDefinition> characters,
        IEnumerable<TechnologyDefinition> technologies,
        IEnumerable<CountryDefinition> countries,
        IEnumerable<StateDefinition> states,
        string outputBasePath)
    {
        // Collect entries per language: language suffix → list of (key, value)
        var entriesByLanguage = new Dictionary<string, List<(string Key, string Value)>>();

        // Helper to add entry to dictionary
        void AddEntry(string suffix, string key, string value)
        {
            if (!entriesByLanguage.ContainsKey(suffix)) 
                entriesByLanguage[suffix] = [];
            entriesByLanguage[suffix].Add((key, value));
        }

        // 1. Focus Localisation
        foreach (var focus in focuses)
        {
            var dynamicTitle = focus.CompileDynamicTitle();
            if (dynamicTitle.Count > 0)
            {
                var suffixes = new HashSet<string>();
                int index = 0;
                foreach (var (trigger, text) in dynamicTitle)
                {
                    string locKey = trigger == null ? $"{focus.Id}_title_default" : $"{focus.Id}_title_{index++}";
                    foreach (var entry in text.GetEntries())
                    {
                        AddEntry(entry.Suffix, locKey, entry.Text);
                        suffixes.Add(entry.Suffix);
                    }
                }
                foreach (var suffix in suffixes)
                {
                    AddEntry(suffix, focus.Id, $"[Get_{focus.Id}_Title]");
                }
            }
            else if (focus.Title is { HasAny: true } title)
            {
                foreach (var entry in title.GetEntries())
                {
                    AddEntry(entry.Suffix, focus.Id, entry.Text);
                }
            }

            var dynamicDesc = focus.CompileDynamicDescription();
            if (dynamicDesc.Count > 0)
            {
                var suffixes = new HashSet<string>();
                int index = 0;
                foreach (var (trigger, text) in dynamicDesc)
                {
                    string locKey = trigger == null ? $"{focus.Id}_desc_default" : $"{focus.Id}_desc_{index++}";
                    foreach (var entry in text.GetEntries())
                    {
                        AddEntry(entry.Suffix, locKey, entry.Text);
                        suffixes.Add(entry.Suffix);
                    }
                }
                foreach (var suffix in suffixes)
                {
                    AddEntry(suffix, $"{focus.Id}_desc", $"[Get_{focus.Id}_Desc]");
                }
            }
            else if (focus.Description is { HasAny: true } desc)
            {
                foreach (var entry in desc.GetEntries())
                {
                    AddEntry(entry.Suffix, $"{focus.Id}_desc", entry.Text);
                }
            }
        }

        // 2. Event Localisation
        foreach (var ev in events)
        {
            if (ev.Title is { HasAny: true } title)
            {
                foreach (var entry in title.GetEntries())
                {
                    AddEntry(entry.Suffix, $"{ev.Id}.t", entry.Text);
                }
            }
            if (ev.Description is { HasAny: true } desc)
            {
                foreach (var entry in desc.GetEntries())
                {
                    AddEntry(entry.Suffix, $"{ev.Id}.d", entry.Text);
                }
            }
            foreach (var optLoc in ev.CompileOptionLocalizations())
            {
                foreach (var entry in optLoc.Name.GetEntries())
                {
                    AddEntry(entry.Suffix, optLoc.Identifier, entry.Text);
                }
            }
        }

        // 3. Decision Category Localisation
        foreach (var cat in decisionCategories)
        {
            if (cat.Name?.HasAny == true)
            {
                foreach (var entry in cat.Name.GetEntries())
                {
                    AddEntry(entry.Suffix, cat.Id, entry.Text);
                }
            }
        }

        // 4. Decision Localisation
        foreach (var dec in decisions)
        {
            if (dec.Name?.HasAny == true)
            {
                foreach (var entry in dec.Name.GetEntries())
                {
                    AddEntry(entry.Suffix, dec.Id, entry.Text);
                }
            }
            if (dec.Description?.HasAny == true)
            {
                foreach (var entry in dec.Description.GetEntries())
                {
                    AddEntry(entry.Suffix, $"{dec.Id}_desc", entry.Text);
                }
            }
        }

        // 5. Idea Localisation
        foreach (var idea in ideas)
        {
            if (idea.Name?.HasAny == true)
            {
                foreach (var entry in idea.Name.GetEntries())
                {
                    AddEntry(entry.Suffix, idea.Id, entry.Text);
                }
            }
            if (idea.Description?.HasAny == true)
            {
                foreach (var entry in idea.Description.GetEntries())
                {
                    AddEntry(entry.Suffix, $"{idea.Id}_desc", entry.Text);
                }
            }
        }

        // 7. Technology Localisation
        foreach (var tech in technologies)
        {
            if (tech.Name?.HasAny == true)
            {
                foreach (var entry in tech.Name.GetEntries())
                {
                    AddEntry(entry.Suffix, tech.Id, entry.Text);
                }
            }
            if (tech.Description?.HasAny == true)
            {
                foreach (var entry in tech.Description.GetEntries())
                {
                    AddEntry(entry.Suffix, $"{tech.Id}_desc", entry.Text);
                }
            }
        }

        // 8. State Localisation
        foreach (var state in states)
        {
            if (state.DisplayName?.HasAny == true)
            {
                foreach (var entry in state.DisplayName.GetEntries())
                {
                    AddEntry(entry.Suffix, $"STATE_{state.Id}", entry.Text);
                }
            }
        }

        // 9. Country Localisation
        foreach (var country in countries)
        {
            if (country.Name?.HasAny == true)
            {
                foreach (var entry in country.Name.GetEntries())
                {
                    AddEntry(entry.Suffix, country.Tag, entry.Text);
                }
            }
        }

        // Emit files
        var emittedFiles = new List<string>();
        foreach (var (suffix, entries) in entriesByLanguage)
        {
            if (entries.Count == 0) continue;

            var folderName = suffix.Replace("l_", "");
            var dirPath = Path.Combine(outputBasePath, "localisation", folderName);
            Directory.CreateDirectory(dirPath);

            var fileName = $"{SanitizeModId(modId)}_{suffix}.yml";
            var filePath = Path.Combine(dirPath, fileName);

            WriteYml(filePath, suffix, entries);
            emittedFiles.Add(filePath);
        }

        return emittedFiles;
    }

    private static void WriteYml(string filePath, string suffix, List<(string Key, string Value)> entries)
    {
        using var writer = new StreamWriter(filePath, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        writer.WriteLine($"{suffix}:");
        foreach (var (key, value) in entries)
        {
            writer.WriteLine($" {key}:0 \"{EscapeLocValue(value)}\"");
        }
    }

    private static string EscapeLocValue(string value)
    {
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    /// <summary>
    /// Emit localisation replace files to localisation/replace/{language}/ for overriding vanilla strings.
    /// </summary>
    public static List<string> EmitReplace(
        IEnumerable<LocalisationReplaceDefinition> definitions,
        string outputBasePath)
    {
        var emittedFiles = new List<string>();

        foreach (var def in definitions)
        {
            var build = def.Build();
            if (build.Entries.Count == 0) continue;

            var entriesByLanguage = new Dictionary<string, List<(string Key, string Value)>>();

            foreach (var entry in build.Entries)
            {
                foreach (var loc in entry.Text.GetEntries())
                {
                    if (!entriesByLanguage.ContainsKey(loc.Suffix))
                        entriesByLanguage[loc.Suffix] = [];
                    entriesByLanguage[loc.Suffix].Add((entry.Key, loc.Text));
                }
            }

            foreach (var (suffix, entries) in entriesByLanguage)
            {
                if (entries.Count == 0) continue;

                var folderName = suffix.Replace("l_", "");
                var dirPath = Path.Combine(outputBasePath, "localisation", "replace", folderName);
                Directory.CreateDirectory(dirPath);

                var fileName = $"{build.Id}_replace_{suffix}.yml";
                var filePath = Path.Combine(dirPath, fileName);

                WriteYml(filePath, suffix, entries);
                emittedFiles.Add(filePath);
            }
        }

        return emittedFiles;
    }

    private static string SanitizeModId(string modId)
    {
        return modId.ToLowerInvariant().Replace(' ', '_').Replace('-', '_');
    }
}
