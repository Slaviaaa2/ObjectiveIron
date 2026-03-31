using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits a generic Block (trigger/effect/modifier) to Clausewitz format.
/// Recursively handles nested blocks and properties.
/// </summary>
public static class BlockEmitter
{
    /// <summary>
    /// Emit a block's contents (without the outer key = { } wrapper).
    /// </summary>
    public static void EmitContents(Block block, ClausewitzWriter writer)
    {
        foreach (var entry in block.Entries)
        {
            switch (entry)
            {
                case BlockEntry.PropertyEntry prop:
                    writer.WriteProperty(prop.Property);
                    break;

                case BlockEntry.NestedBlock nested:
                    writer.BeginBlock(nested.Name);
                    EmitContents(nested.Block, writer);
                    writer.EndBlock();
                    break;

                case BlockEntry.Comment comment:
                    writer.WriteComment(comment.Text);
                    break;
            }
        }
    }

    /// <summary>
    /// Emit a named block: key = { contents }.
    /// </summary>
    public static void Emit(string name, Block block, ClausewitzWriter writer)
    {
        writer.BeginBlock(name);
        EmitContents(block, writer);
        writer.EndBlock();
    }
}
