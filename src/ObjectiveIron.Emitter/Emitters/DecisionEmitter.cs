using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits Decision Categories and Decisions to Clausewitz format.
/// </summary>
public static class DecisionEmitter
{
    public static void EmitCategories(IEnumerable<DecisionCategory> categories, ClausewitzWriter writer)
    {
        foreach (var cat in categories)
        {
            writer.BeginBlock(cat.Id);
            
            if (cat.NameIdentifier != null)
                writer.WriteProperty("name", cat.NameIdentifier);
            
            if (cat.Icon != null)
                writer.WriteProperty("icon", cat.Icon);

            if (cat.Visible != null)
                BlockEmitter.Emit("visible", cat.Visible, writer);

            if (cat.Available != null)
                BlockEmitter.Emit("available", cat.Available, writer);

            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }

    public static void EmitDecisions(string categoryId, IEnumerable<Decision> decisions, ClausewitzWriter writer)
    {
        writer.BeginBlock(categoryId);

        foreach (var dec in decisions)
        {
            writer.BeginBlock(dec.Id);

            if (!string.IsNullOrEmpty(dec.DynamicName))
                writer.WriteProperty("name", dec.DynamicName);
            else
                writer.WriteProperty("name", dec.NameIdentifier);

            if (!string.IsNullOrEmpty(dec.DynamicDescription))
                writer.WriteProperty("desc", dec.DynamicDescription);

            if (dec.Icon != null)
                writer.WriteProperty("icon", dec.Icon);
            
            if (dec.Cost.HasValue)
                writer.WriteProperty("cost", dec.Cost.Value);

            if (dec.DaysToRemove.HasValue)
                writer.WriteProperty("days_remove", dec.DaysToRemove.Value);

            if (dec.IsVisible.HasValue)
                writer.WriteProperty("visible_by_default", dec.IsVisible.Value);

            if (dec.Visible != null)
                BlockEmitter.Emit("visible", dec.Visible, writer);

            if (dec.Available != null)
                BlockEmitter.Emit("available", dec.Available, writer);

            if (dec.CompleteEffect != null)
                BlockEmitter.Emit("complete_effect", dec.CompleteEffect, writer);

            if (dec.AiWillDo != null)
                BlockEmitter.Emit("ai_will_do", dec.AiWillDo, writer);

            if (dec.TimeoutEffect != null)
                BlockEmitter.Emit("timeout_effect", dec.TimeoutEffect, writer);

            if (dec.RemoveEffect != null)
                BlockEmitter.Emit("remove_effect", dec.RemoveEffect, writer);

            writer.EndBlock();
            writer.WriteBlankLine();
        }

        writer.EndBlock();
    }
}
