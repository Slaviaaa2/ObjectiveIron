using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class ContinuousFocusEmitter
{
    public static void Emit(ContinuousFocusPaletteBuild palette, ClausewitzWriter writer)
    {
        writer.BeginBlock("continuous_focus_palette");
        writer.WriteProperty("id", palette.Id);

        if (palette.Country != null)
        {
            writer.BeginBlock("country");
            BlockEmitter.EmitContents(palette.Country, writer);
            writer.EndBlock();
        }

        writer.WriteProperty("default", palette.Default);
        writer.WriteProperty("reset_on_civilwar", palette.ResetOnCivilWar);
        writer.WriteBlankLine();

        foreach (var focus in palette.Focuses)
        {
            writer.BeginBlock("focus");
            writer.WriteProperty("id", focus.Id);
            if (focus.Icon != null) writer.WriteProperty("icon", focus.Icon);
            if (focus.DailyCost.HasValue) writer.WriteProperty("daily_cost", focus.DailyCost.Value);
            if (focus.AvailableIfCapitulated) writer.WriteProperty("available_if_capitulated", true);
            if (focus.SupportsAiStrategy != null) writer.WriteProperty("supports_ai_strategy", focus.SupportsAiStrategy);

            if (focus.Available is { Entries.Count: > 0 })
            {
                writer.BeginBlock("available");
                BlockEmitter.EmitContents(focus.Available, writer);
                writer.EndBlock();
            }

            if (focus.Enable is { Entries.Count: > 0 })
            {
                writer.BeginBlock("enable");
                BlockEmitter.EmitContents(focus.Enable, writer);
                writer.EndBlock();
            }

            if (focus.Modifier is { Entries.Count: > 0 })
            {
                writer.BeginBlock("modifier");
                BlockEmitter.EmitContents(focus.Modifier, writer);
                writer.EndBlock();
            }

            if (focus.SelectEffect is { Entries.Count: > 0 })
            {
                writer.BeginBlock("select_effect");
                BlockEmitter.EmitContents(focus.SelectEffect, writer);
                writer.EndBlock();
            }

            if (focus.CancelEffect is { Entries.Count: > 0 })
            {
                writer.BeginBlock("cancel_effect");
                BlockEmitter.EmitContents(focus.CancelEffect, writer);
                writer.EndBlock();
            }

            writer.EndBlock(); // focus
            writer.WriteBlankLine();
        }

        writer.EndBlock(); // continuous_focus_palette
    }
}
