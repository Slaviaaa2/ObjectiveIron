using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits dynamic_modifiers to Clausewitz format.
/// </summary>
public static class DynamicModifierEmitter
{
    public static void Emit(IEnumerable<DynamicModifierBuildResult> modifiers, ClausewitzWriter writer)
    {
        foreach (var mod in modifiers)
        {
            writer.BeginBlock(mod.Id);

            if (mod.Icon != null)
                writer.WriteProperty("icon", mod.Icon);

            if (mod.AttackerModifier)
                writer.WriteProperty("attacker_modifier", true);

            if (mod.Enable != null)
                BlockEmitter.Emit("enable", mod.Enable, writer);

            if (mod.RemoveTrigger != null)
                BlockEmitter.Emit("remove_trigger", mod.RemoveTrigger, writer);

            // Emit modifier properties directly (not inside a "modifier" block)
            BlockEmitter.EmitContents(mod.Modifiers, writer);

            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}
