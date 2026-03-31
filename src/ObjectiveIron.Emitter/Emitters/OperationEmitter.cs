using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class OperationEmitter
{
    public static void Emit(OperationFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("operations");

        foreach (var op in file.Operations)
        {
            writer.BeginBlock(op.Id);

            if (op.Icon != null) writer.WriteProperty("icon", op.Icon);
            if (op.MapIcon != null) writer.WriteProperty("map_icon", op.MapIcon);
            if (op.Priority.HasValue) writer.WriteProperty("priority", op.Priority.Value);
            if (op.Days.HasValue) writer.WriteProperty("days", op.Days.Value);
            if (op.NetworkStrength.HasValue) writer.WriteProperty("network_strength", op.NetworkStrength.Value);
            if (op.OperativesNeeded.HasValue) writer.WriteProperty("operatives", op.OperativesNeeded.Value);
            if (op.RiskChance.HasValue) writer.WriteProperty("risk_chance", op.RiskChance.Value);
            if (op.ExperienceGain.HasValue) writer.WriteProperty("experience", op.ExperienceGain.Value);
            if (op.Cost.HasValue) writer.WriteProperty("cost", op.Cost.Value);

            if (op.Visible is { Entries.Count: > 0 })
            {
                writer.BeginBlock("visible");
                BlockEmitter.EmitContents(op.Visible, writer);
                writer.EndBlock();
            }

            if (op.Available is { Entries.Count: > 0 })
            {
                writer.BeginBlock("available");
                BlockEmitter.EmitContents(op.Available, writer);
                writer.EndBlock();
            }

            if (op.OnStart is { Entries.Count: > 0 })
            {
                writer.BeginBlock("on_start");
                BlockEmitter.EmitContents(op.OnStart, writer);
                writer.EndBlock();
            }

            if (op.Outcome is { Entries.Count: > 0 })
            {
                writer.BeginBlock("outcome");
                BlockEmitter.EmitContents(op.Outcome, writer);
                writer.EndBlock();
            }

            if (op.RiskOutcome is { Entries.Count: > 0 })
            {
                writer.BeginBlock("risk_outcome");
                BlockEmitter.EmitContents(op.RiskOutcome, writer);
                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }

        writer.EndBlock();
    }
}
