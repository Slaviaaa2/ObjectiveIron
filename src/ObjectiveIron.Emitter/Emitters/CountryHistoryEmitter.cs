using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits country history files: history/countries/{TAG} - {TAG}.txt
/// </summary>
public static class CountryHistoryEmitter
{
    public static void Emit(CountryHistoryBuildResult history, ClausewitzWriter writer)
    {
        // Capital
        if (history.Capital.HasValue)
            writer.WriteProperty("capital", history.Capital.Value);

        // OOB references
        if (history.Oob != null)
            writer.WriteProperty("set_oob", $"\"{history.Oob}\"");
        if (history.NavalOob != null)
            writer.WriteProperty("set_naval_oob", $"\"{history.NavalOob}\"");
        if (history.AirOob != null)
            writer.WriteProperty("set_air_oob", $"\"{history.AirOob}\"");

        // Fuel ratio
        if (history.FuelRatio.HasValue)
            writer.WriteProperty("set_fuel_ratio", history.FuelRatio.Value);

        // Convoys
        if (history.Convoys.HasValue)
            writer.WriteProperty("set_convoys", history.Convoys.Value);

        // Research slots
        if (history.ResearchSlots.HasValue)
            writer.WriteProperty("set_research_slots", history.ResearchSlots.Value);

        // Stability
        if (history.Stability.HasValue)
            writer.WriteProperty("set_stability", history.Stability.Value);

        // War support
        if (history.WarSupport.HasValue)
            writer.WriteProperty("set_war_support", history.WarSupport.Value);

        writer.WriteBlankLine();

        // Conditionals (DLC checks etc.) — placed early like vanilla
        if (history.Conditionals is { Count: > 0 })
        {
            foreach (var cond in history.Conditionals)
            {
                writer.BeginBlock("if");
                writer.BeginBlock("limit");
                BlockEmitter.EmitContents(cond.Limit, writer);
                writer.EndBlock();
                BlockEmitter.EmitContents(cond.Effects, writer);
                writer.EndBlock();
                writer.WriteBlankLine();
            }
        }

        // Politics
        if (history.Politics != null)
        {
            writer.BeginBlock("set_politics");
            if (history.Politics.RulingParty != null)
                writer.WriteProperty("ruling_party", history.Politics.RulingParty);
            if (history.Politics.LastElection != null)
                writer.WriteProperty("last_election", $"\"{history.Politics.LastElection}\"");
            if (history.Politics.ElectionFrequency.HasValue)
                writer.WriteProperty("election_frequency", history.Politics.ElectionFrequency.Value);
            if (history.Politics.ElectionsAllowed.HasValue)
                writer.WriteProperty("elections_allowed", history.Politics.ElectionsAllowed.Value);
            writer.EndBlock();
            writer.WriteBlankLine();
        }

        // Popularities
        if (history.Popularities is { Count: > 0 })
        {
            writer.BeginBlock("set_popularities");
            foreach (var pop in history.Popularities)
            {
                writer.WriteProperty(pop.Ideology, pop.Percentage);
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }

        // Technologies
        if (history.Technologies is { Count: > 0 })
        {
            writer.BeginBlock("set_technology");
            foreach (var tech in history.Technologies)
            {
                writer.WriteProperty(tech, 1);
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }

        // Ideas
        if (history.Ideas is { Count: > 0 })
        {
            foreach (var idea in history.Ideas)
            {
                writer.WriteProperty("add_ideas", idea);
            }
            writer.WriteBlankLine();
        }

        // Effects (recruit_character, add_equipment, etc.)
        if (history.Effects is { Entries.Count: > 0 })
        {
            BlockEmitter.EmitContents(history.Effects, writer);
            writer.WriteBlankLine();
        }

        // Date-specific entries
        if (history.DateEntries is { Count: > 0 })
        {
            foreach (var entry in history.DateEntries)
            {
                writer.BeginBlock(entry.Date);
                BlockEmitter.EmitContents(entry.Effects, writer);
                writer.EndBlock();
                writer.WriteBlankLine();
            }
        }
    }
}
