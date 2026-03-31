using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class OccupationLawEmitter
{
    public static void Emit(OccupationLawFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("occupation_laws");

        foreach (var law in file.Laws)
        {
            writer.BeginBlock(law.Id);

            if (law.Icon != null) writer.WriteProperty("icon", law.Icon);
            if (law.Sound != null) writer.WriteProperty("sound", law.Sound);
            if (law.StateFrame.HasValue) writer.WriteProperty("state_frame", law.StateFrame.Value);
            if (law.GuiOrder.HasValue) writer.WriteProperty("gui_order", law.GuiOrder.Value);

            if (law.Visible is { Entries.Count: > 0 })
            {
                writer.BeginBlock("visible");
                BlockEmitter.EmitContents(law.Visible, writer);
                writer.EndBlock();
            }

            if (law.Available is { Entries.Count: > 0 })
            {
                writer.BeginBlock("available");
                BlockEmitter.EmitContents(law.Available, writer);
                writer.EndBlock();
            }

            if (law.Modifiers is { Count: > 0 })
            {
                writer.BeginBlock("modifier");
                foreach (var (key, val) in law.Modifiers)
                    writer.WriteProperty(key, val);
                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }

        writer.EndBlock();
    }
}
