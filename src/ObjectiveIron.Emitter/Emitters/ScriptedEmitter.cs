using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits scripted triggers and effects to Clausewitz format.
/// </summary>
public static class ScriptedEmitter
{
    public static void EmitTriggers(IEnumerable<ScriptedTrigger> triggers, ClausewitzWriter writer)
    {
        foreach (var t in triggers)
        {
            writer.BeginBlock(t.Id);
            BlockEmitter.EmitContents(t.Body, writer);
            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }

    public static void EmitEffects(IEnumerable<ScriptedEffect> effects, ClausewitzWriter writer)
    {
        foreach (var e in effects)
        {
            writer.BeginBlock(e.Id);
            BlockEmitter.EmitContents(e.Body, writer);
            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}
