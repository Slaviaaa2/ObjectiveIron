using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits country tags and country definition files.
/// </summary>
public static class CountryEmitter
{
    /// <summary>
    /// Emit the country tag file: TAG = "countries/filename.txt"
    /// </summary>
    public static void EmitTags(IEnumerable<CountryDefinition> countries, ClausewitzWriter writer)
    {
        foreach (var country in countries)
        {
            var filename = $"countries/{country.Tag} - {country.Tag}.txt";
            writer.WriteProperty(country.Tag, $"\"{filename}\"");
        }
    }

    /// <summary>
    /// Emit a single country definition file (color, graphical_culture, etc.)
    /// </summary>
    public static void EmitCountryFile(CountryDefinition country, ClausewitzWriter writer)
    {
        writer.WriteProperty("graphical_culture", country.GraphicalCulture);

        if (country.GraphicalCulture2d != null)
            writer.WriteProperty("graphical_culture_2d", country.GraphicalCulture2d);

        var (r, g, b) = country.Color;
        writer.BeginBlock("color");
        writer.WriteUnquoted($"{r}  {g}  {b}");
        writer.EndBlock();

        if (country.ColorUi.HasValue)
        {
            var (ur, ug, ub) = country.ColorUi.Value;
            writer.BeginBlock("color_ui");
            writer.WriteUnquoted($"{ur}  {ug}  {ub}");
            writer.EndBlock();
        }

        if (country.FlagColors is { Length: > 0 })
        {
            writer.BeginBlock("flag_colors");
            writer.WriteUnquoted(string.Join("  ", country.FlagColors));
            writer.EndBlock();
        }
    }
}
