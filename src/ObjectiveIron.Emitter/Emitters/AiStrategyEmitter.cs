using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class AiStrategyEmitter
{
    public static void Emit(AiStrategyFileBuild file, ClausewitzWriter writer)
    {
        foreach (var entry in file.Entries)
        {
            writer.BeginBlock(entry.Id);

            if (entry.Allowed is { Entries.Count: > 0 })
            {
                writer.BeginBlock("allowed");
                BlockEmitter.EmitContents(entry.Allowed, writer);
                writer.EndBlock();
            }

            if (entry.Enable is { Entries.Count: > 0 })
            {
                writer.BeginBlock("enable");
                BlockEmitter.EmitContents(entry.Enable, writer);
                writer.EndBlock();
            }

            if (entry.Abort is { Entries.Count: > 0 })
            {
                writer.BeginBlock("abort");
                BlockEmitter.EmitContents(entry.Abort, writer);
                writer.EndBlock();
            }

            foreach (var strategy in entry.Strategies)
            {
                writer.WriteBlankLine();
                writer.BeginBlock("ai_strategy");
                writer.WriteProperty("type", strategy.Type);
                writer.WriteProperty("id", $"\"{strategy.TargetId}\"");
                writer.WriteProperty("value", strategy.Value);
                writer.EndBlock();
            }

            writer.EndBlock(); // entry id
            writer.WriteBlankLine();
        }
    }
}
