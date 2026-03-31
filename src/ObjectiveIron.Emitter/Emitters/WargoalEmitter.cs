using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class WargoalEmitter
{
    public static void Emit(WargoalFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("wargoals");

        foreach (var wg in file.Wargoals)
        {
            writer.BeginBlock(wg.Id);

            if (wg.WarDesirability.HasValue) writer.WriteProperty("war_desirability", wg.WarDesirability.Value);
            if (wg.WorldTension.HasValue) writer.WriteProperty("world_tension", wg.WorldTension.Value);
            if (wg.JustifyWarGoalCost.HasValue) writer.WriteProperty("justify_war_goal_cost", wg.JustifyWarGoalCost.Value);
            if (wg.GenerateCb.HasValue) writer.WriteProperty("generate_cb", wg.GenerateCb.Value);

            if (wg.Available is { Entries.Count: > 0 })
            {
                writer.BeginBlock("available");
                BlockEmitter.EmitContents(wg.Available, writer);
                writer.EndBlock();
            }

            if (wg.Allowed is { Entries.Count: > 0 })
            {
                writer.BeginBlock("allowed");
                BlockEmitter.EmitContents(wg.Allowed, writer);
                writer.EndBlock();
            }

            if (wg.OnAdd is { Entries.Count: > 0 })
            {
                writer.BeginBlock("on_add");
                BlockEmitter.EmitContents(wg.OnAdd, writer);
                writer.EndBlock();
            }

            if (wg.OnSuccess is { Entries.Count: > 0 })
            {
                writer.BeginBlock("on_success");
                BlockEmitter.EmitContents(wg.OnSuccess, writer);
                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }

        writer.EndBlock();
    }
}
