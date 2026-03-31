using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits opinion_modifiers to Clausewitz format.
/// </summary>
public static class OpinionModifierEmitter
{
    public static void Emit(IEnumerable<OpinionModifier> modifiers, ClausewitzWriter writer)
    {
        foreach (var mod in modifiers)
        {
            writer.BeginBlock(mod.Id);
            writer.WriteProperty("value", mod.Value);

            if (mod.Decay.HasValue)
                writer.WriteProperty("decay", mod.Decay.Value);
            if (mod.MinTrust.HasValue)
                writer.WriteProperty("min_trust", mod.MinTrust.Value);
            if (mod.MaxTrust.HasValue)
                writer.WriteProperty("max_trust", mod.MaxTrust.Value);
            if (mod.Days.HasValue)
                writer.WriteProperty("days", mod.Days.Value);
            if (mod.Months.HasValue)
                writer.WriteProperty("months", mod.Months.Value);
            if (mod.Years.HasValue)
                writer.WriteProperty("years", mod.Years.Value);
            if (mod.Trade.HasValue)
                writer.WriteProperty("trade", mod.Trade.Value);

            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}
