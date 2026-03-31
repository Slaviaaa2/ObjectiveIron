using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits state history files: history/states/{Id}-{Name}.txt
/// </summary>
public static class StateEmitter
{
    public static void Emit(StateBuildResult state, ClausewitzWriter writer)
    {
        writer.BeginBlock("state");

        writer.WriteProperty("id", state.Id);
        writer.WriteProperty("name", $"\"STATE_{state.Id}\"");
        writer.WriteProperty("manpower", state.Manpower);

        if (state.BuildingsMaxLevelFactor.HasValue)
            writer.WriteProperty("buildings_max_level_factor", state.BuildingsMaxLevelFactor.Value);

        if (state.Impassable)
            writer.WriteProperty("impassable", "yes");

        writer.WriteBlankLine();
        writer.WriteProperty("state_category", state.StateCategory);
        writer.WriteBlankLine();

        // Resources
        if (state.Resources is { Count: > 0 })
        {
            writer.BeginBlock("resources");
            foreach (var res in state.Resources)
            {
                writer.WriteProperty(res.Resource, res.Amount);
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }

        // History block
        EmitHistory(state.History, writer);

        // Provinces
        writer.WriteBlankLine();
        writer.BeginBlock("provinces");
        writer.WriteUnquoted(string.Join(" ", state.Provinces));
        writer.EndBlock();

        if (state.LocalSupplies.HasValue)
        {
            writer.WriteBlankLine();
            writer.WriteProperty("local_supplies", state.LocalSupplies.Value);
        }

        writer.EndBlock(); // state
    }

    private static void EmitHistory(StateHistoryBuildResult history, ClausewitzWriter writer)
    {
        writer.BeginBlock("history");

        if (history.Owner != null)
            writer.WriteProperty("owner", history.Owner);

        if (history.Controller != null)
            writer.WriteProperty("controller", history.Controller);

        // Cores
        if (history.Cores is { Count: > 0 })
        {
            foreach (var core in history.Cores)
                writer.WriteProperty("add_core_of", core);
        }

        // Claims
        if (history.Claims is { Count: > 0 })
        {
            foreach (var claim in history.Claims)
                writer.WriteProperty("add_claim_by", claim);
        }

        // Victory points
        if (history.VictoryPoints is { Count: > 0 })
        {
            foreach (var vp in history.VictoryPoints)
            {
                writer.BeginBlock("victory_points");
                writer.WriteUnquoted($"{vp.ProvinceId} {vp.Value}");
                writer.EndBlock();
            }
        }

        // Buildings
        if (history.Buildings is { Count: > 0 } || history.ProvinceBuildings is { Count: > 0 })
        {
            writer.BeginBlock("buildings");

            if (history.Buildings is { Count: > 0 })
            {
                foreach (var b in history.Buildings)
                    writer.WriteProperty(b.Type, b.Level);
            }

            if (history.ProvinceBuildings is { Count: > 0 })
            {
                // Group by province
                var byProvince = history.ProvinceBuildings
                    .GroupBy(pb => pb.ProvinceId);
                foreach (var group in byProvince)
                {
                    writer.BeginBlock(group.Key.ToString());
                    foreach (var pb in group)
                        writer.WriteProperty(pb.Type, pb.Level);
                    writer.EndBlock();
                }
            }

            writer.EndBlock(); // buildings
        }

        // Effects
        if (history.Effects is { Count: > 0 })
        {
            foreach (var effect in history.Effects)
                BlockEmitter.EmitContents(effect.EffectBlock, writer);
        }

        // Date entries
        if (history.DateEntries is { Count: > 0 })
        {
            foreach (var entry in history.DateEntries)
            {
                writer.WriteBlankLine();
                writer.BeginBlock(entry.Date);
                BlockEmitter.EmitContents(entry.Effects, writer);
                writer.EndBlock();
            }
        }

        writer.EndBlock(); // history
    }
}
