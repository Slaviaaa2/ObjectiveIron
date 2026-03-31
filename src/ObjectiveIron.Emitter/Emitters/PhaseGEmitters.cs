using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class MioEmitter
{
    public static void Emit(MioFileBuild file, ClausewitzWriter writer)
    {
        foreach (var mio in file.Organizations)
        {
            writer.BeginBlock(mio.Id);
            if (mio.Name != null) writer.WriteProperty("name", mio.Name);
            if (mio.Icon != null) writer.WriteProperty("icon", mio.Icon);
            if (mio.OwnerTag != null) writer.WriteProperty("allowed", $"{{ original_tag = {mio.OwnerTag} }}");

            if (mio.EquipmentTypes is { Count: > 0 })
            {
                writer.BeginBlock("equipment_type");
                foreach (var t in mio.EquipmentTypes) writer.WriteUnquoted(t);
                writer.EndBlock();
            }

            if (mio.ResearchCategories is { Count: > 0 })
            {
                writer.BeginBlock("research_categories");
                foreach (var c in mio.ResearchCategories) writer.WriteUnquoted(c);
                writer.EndBlock();
            }

            if (mio.InitialModifiers is { Count: > 0 })
            {
                writer.BeginBlock("initial_trait");
                writer.BeginBlock("equipment_bonus");
                foreach (var (k, v) in mio.InitialModifiers) writer.WriteProperty(k, v);
                writer.EndBlock();
                writer.EndBlock();
            }

            if (mio.Traits is { Count: > 0 })
            {
                writer.BeginBlock("add_trait");
                foreach (var trait in mio.Traits)
                {
                    writer.BeginBlock(trait.Id);
                    if (trait.Icon != null) writer.WriteProperty("icon", trait.Icon);
                    if (trait.Parent != null) writer.WriteProperty("parent", trait.Parent);
                    if (trait.Position.HasValue) writer.WriteProperty("position", trait.Position.Value);
                    if (trait.Modifiers is { Count: > 0 })
                    {
                        writer.BeginBlock("equipment_bonus");
                        foreach (var (k, v) in trait.Modifiers) writer.WriteProperty(k, v);
                        writer.EndBlock();
                    }
                    writer.EndBlock();
                }
                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}

public static class IntelAgencyEmitter
{
    public static void Emit(IntelAgencyFileBuild file, ClausewitzWriter writer)
    {
        foreach (var agency in file.Agencies)
        {
            writer.BeginBlock(agency.Id);
            if (agency.Picture != null) writer.WriteProperty("picture", agency.Picture);
            if (agency.Names != null) writer.WriteProperty("names", $"\"{agency.Names}\"");
            if (agency.Upgrades is { Count: > 0 })
            {
                writer.BeginBlock("default_upgrades");
                foreach (var u in agency.Upgrades)
                    writer.WriteProperty(u.UpgradeId, true);
                writer.EndBlock();
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}

public static class PeaceConferenceEmitter
{
    public static void Emit(PeaceConferenceFileBuild file, ClausewitzWriter writer)
    {
        foreach (var action in file.Actions)
        {
            writer.BeginBlock(action.Id);
            if (action.Cost.HasValue) writer.WriteProperty("cost", action.Cost.Value);
            if (action.Available is { Entries.Count: > 0 })
            {
                writer.BeginBlock("available");
                BlockEmitter.EmitContents(action.Available, writer);
                writer.EndBlock();
            }
            if (action.Effect is { Entries.Count: > 0 })
            {
                writer.BeginBlock("effect");
                BlockEmitter.EmitContents(action.Effect, writer);
                writer.EndBlock();
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}

public static class BopEmitter
{
    public static void Emit(BopFileBuild file, ClausewitzWriter writer)
    {
        foreach (var bop in file.Entries)
        {
            writer.BeginBlock(bop.Id);
            if (bop.InitialValue.HasValue) writer.WriteProperty("initial_value", bop.InitialValue.Value);
            if (bop.LeftSide != null) writer.WriteProperty("left_side", bop.LeftSide);
            if (bop.RightSide != null) writer.WriteProperty("right_side", bop.RightSide);
            if (bop.Ranges is { Count: > 0 })
            {
                foreach (var range in bop.Ranges)
                {
                    writer.BeginBlock("range");
                    writer.WriteProperty("min", range.Min);
                    writer.WriteProperty("max", range.Max);
                    if (range.Modifiers is { Count: > 0 })
                    {
                        writer.BeginBlock("modifier");
                        foreach (var (k, v) in range.Modifiers) writer.WriteProperty(k, v);
                        writer.EndBlock();
                    }
                    writer.EndBlock();
                }
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}

public static class ScriptedDiploEmitter
{
    public static void Emit(ScriptedDiploFileBuild file, ClausewitzWriter writer)
    {
        foreach (var action in file.Actions)
        {
            writer.BeginBlock(action.Id);
            if (action.Cost.HasValue) writer.WriteProperty("cost", action.Cost.Value);
            if (action.Visible is { Entries.Count: > 0 })
            {
                writer.BeginBlock("visible");
                BlockEmitter.EmitContents(action.Visible, writer);
                writer.EndBlock();
            }
            if (action.Available is { Entries.Count: > 0 })
            {
                writer.BeginBlock("available");
                BlockEmitter.EmitContents(action.Available, writer);
                writer.EndBlock();
            }
            if (action.CompleteEffect is { Entries.Count: > 0 })
            {
                writer.BeginBlock("complete_effect");
                BlockEmitter.EmitContents(action.CompleteEffect, writer);
                writer.EndBlock();
            }
            if (action.RemoveEffect is { Entries.Count: > 0 })
            {
                writer.BeginBlock("remove_effect");
                BlockEmitter.EmitContents(action.RemoveEffect, writer);
                writer.EndBlock();
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}

public static class ScriptedGuiEmitter
{
    public static void Emit(ScriptedGuiFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("scripted_guis");
        foreach (var gui in file.Guis)
        {
            writer.BeginBlock(gui.Id);
            if (gui.Context != null) writer.WriteProperty("context_type", gui.Context);
            if (gui.Window != null) writer.WriteProperty("window_name", $"\"{gui.Window}\"");

            if (gui.Visible is { Entries.Count: > 0 })
            {
                writer.BeginBlock("visible");
                BlockEmitter.EmitContents(gui.Visible, writer);
                writer.EndBlock();
            }

            if (gui.Effects is { Count: > 0 })
            {
                writer.BeginBlock("effects");
                foreach (var eff in gui.Effects)
                {
                    writer.BeginBlock(eff.Name);
                    BlockEmitter.EmitContents(eff.Block, writer);
                    writer.EndBlock();
                }
                writer.EndBlock();
            }

            if (gui.Triggers is { Count: > 0 })
            {
                writer.BeginBlock("triggers");
                foreach (var trig in gui.Triggers)
                {
                    writer.BeginBlock(trig.Name);
                    BlockEmitter.EmitContents(trig.Block, writer);
                    writer.EndBlock();
                }
                writer.EndBlock();
            }

            if (gui.Properties is { Count: > 0 })
            {
                writer.BeginBlock("properties");
                foreach (var prop in gui.Properties)
                    writer.WriteProperty(prop.Name, prop.Value);
                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }
        writer.EndBlock();
    }
}
