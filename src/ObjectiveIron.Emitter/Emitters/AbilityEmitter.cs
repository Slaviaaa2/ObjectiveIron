using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class AbilityEmitter
{
    public static void Emit(AbilityFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("abilities");

        foreach (var ab in file.Abilities)
        {
            writer.BeginBlock(ab.Id);

            if (ab.Name != null) writer.WriteProperty("name", ab.Name);
            if (ab.Desc != null) writer.WriteProperty("desc", ab.Desc);
            if (ab.Icon != null) writer.WriteProperty("icon", ab.Icon);
            if (ab.Sound != null) writer.WriteProperty("sound", ab.Sound);
            if (ab.Type != null) writer.WriteProperty("type", ab.Type);
            if (ab.Cost.HasValue) writer.WriteProperty("cost", ab.Cost.Value);
            if (ab.Duration.HasValue) writer.WriteProperty("duration", ab.Duration.Value);
            if (ab.Cooldown.HasValue) writer.WriteProperty("cooldown", ab.Cooldown.Value);

            if (ab.Allowed is { Entries.Count: > 0 })
            {
                writer.BeginBlock("allowed");
                BlockEmitter.EmitContents(ab.Allowed, writer);
                writer.EndBlock();
            }

            if (ab.Available is { Entries.Count: > 0 })
            {
                writer.BeginBlock("available");
                BlockEmitter.EmitContents(ab.Available, writer);
                writer.EndBlock();
            }

            if (ab.UnitModifiers is { Count: > 0 })
            {
                writer.BeginBlock("unit_modifier");
                foreach (var (key, val) in ab.UnitModifiers)
                    writer.WriteProperty(key, val);
                writer.EndBlock();
            }

            if (ab.OnActivate is { Entries.Count: > 0 })
            {
                writer.BeginBlock("ai_will_do");
                BlockEmitter.EmitContents(ab.OnActivate, writer);
                writer.EndBlock();
            }

            if (ab.CancelTrigger is { Entries.Count: > 0 })
            {
                writer.BeginBlock("cancel_trigger");
                BlockEmitter.EmitContents(ab.CancelTrigger, writer);
                writer.EndBlock();
            }

            if (ab.CancelEffect is { Entries.Count: > 0 })
            {
                writer.BeginBlock("cancel_effect");
                BlockEmitter.EmitContents(ab.CancelEffect, writer);
                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }

        writer.EndBlock();
    }
}
