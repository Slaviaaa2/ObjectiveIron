using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class LeaderTraitEmitter
{
    public static void Emit(LeaderTraitFileBuild file, ClausewitzWriter writer, string rootKey = "leader_traits")
    {
        writer.BeginBlock(rootKey);

        foreach (var trait in file.Traits)
        {
            writer.BeginBlock(trait.Id);

            if (trait.Type != null) writer.WriteProperty("type", trait.Type);
            if (trait.Sprite != null) writer.WriteProperty("sprite", trait.Sprite);
            if (trait.RandomAssign.HasValue) writer.WriteProperty("random", trait.RandomAssign.Value);

            if (trait.Allowed is { Entries.Count: > 0 })
            {
                writer.BeginBlock("allowed");
                BlockEmitter.EmitContents(trait.Allowed, writer);
                writer.EndBlock();
            }

            if (trait.Visible is { Entries.Count: > 0 })
            {
                writer.BeginBlock("visible");
                BlockEmitter.EmitContents(trait.Visible, writer);
                writer.EndBlock();
            }

            if (trait.Modifiers is { Count: > 0 })
            {
                writer.BeginBlock("modifier");
                foreach (var (key, val) in trait.Modifiers)
                    writer.WriteProperty(key, val);
                writer.EndBlock();
            }

            if (trait.NonStackableModifiers is { Count: > 0 })
            {
                writer.BeginBlock("non_stackable_modifier");
                foreach (var (key, val) in trait.NonStackableModifiers)
                    writer.WriteProperty(key, val);
                writer.EndBlock();
            }

            if (trait.TraitXp is { Count: > 0 })
            {
                writer.BeginBlock("trait_xp");
                foreach (var xp in trait.TraitXp)
                    writer.WriteUnquoted(xp);
                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }

        writer.EndBlock();
    }
}
