using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits a FocusTree and its Focus nodes to Clausewitz format.
/// 
/// Output structure:
/// <code>
/// focus_tree = {
///     id = tree_id
///     country = { ... }
///     default = no
///     
///     focus = { ... }
///     focus = { ... }
/// }
/// </code>
/// </summary>
public static class FocusTreeEmitter
{
    public static void Emit(FocusTree tree, ClausewitzWriter writer)
    {
        writer.BeginBlock("focus_tree");

        writer.WriteProperty("id", tree.Id);
        writer.WriteBlankLine();

        // Country filter
        EmitCountryFilter(tree.Country, writer);
        writer.WriteBlankLine();

        // Default
        if (!tree.IsDefault)
            writer.WriteProperty("default", false);

        // Continuous focus override
        if (tree.ContinuousFocusHasCountryTreeOverride)
            writer.WriteProperty("continuous_focus_has_country_tree_override", true);

        // Initial show position
        if (tree.InitialShowPosition.HasValue)
        {
            writer.BeginBlock("initial_show_position");
            writer.WriteProperty("x", tree.InitialShowPosition.Value.X);
            writer.WriteProperty("y", tree.InitialShowPosition.Value.Y);
            writer.EndBlock();
        }

        writer.WriteBlankLine();

        // Shared focuses
        foreach (var sharedId in tree.SharedFocuses)
        {
            writer.WriteProperty("shared_focus", sharedId);
        }

        // Focus nodes
        for (var i = 0; i < tree.Focuses.Count; i++)
        {
            EmitFocus(tree.Focuses[i], writer);
            if (i < tree.Focuses.Count - 1)
                writer.WriteBlankLine();
        }

        writer.EndBlock();
    }

    private static void EmitCountryFilter(CountryFilter filter, ClausewitzWriter writer)
    {
        writer.BeginBlock("country");
        writer.WriteProperty("factor", filter.BaseFactor);

        foreach (var mod in filter.Modifiers)
        {
            writer.BeginBlock("modifier");
            writer.WriteProperty("add", mod.Add);
            writer.WriteProperty("tag", mod.Tag);
            writer.EndBlock();
        }

        writer.EndBlock();
    }

    private static void EmitFocus(Focus focus, ClausewitzWriter writer)
    {
        writer.BeginBlock("focus");

        writer.WriteProperty("id", focus.Id);

        if (focus.Dynamic)
            writer.WriteProperty("dynamic", true);

        if (focus.Icon is not null)
            writer.WriteProperty("icon", focus.Icon);

        foreach (var dynamicIcon in focus.DynamicIcons)
        {
            writer.BeginBlock("icon");
            writer.WriteProperty("value", dynamicIcon.Value);
            if (dynamicIcon.Trigger is not null)
            {
                BlockEmitter.Emit("trigger", dynamicIcon.Trigger, writer);
            }
            writer.EndBlock();
        }

        writer.WriteProperty("x", focus.X);
        writer.WriteProperty("y", focus.Y);
        writer.WriteProperty("cost", focus.Cost);

        if (focus.RelativePositionId is not null)
            writer.WriteProperty("relative_position_id", focus.RelativePositionId);

        // Prerequisites (each group = one prerequisite block with OR semantics)
        foreach (var group in focus.Prerequisites)
        {
            writer.BeginBlock("prerequisite");
            foreach (var prereqId in group)
                writer.WriteProperty("focus", prereqId);
            writer.EndBlock();
        }

        // Mutually exclusive
        if (focus.MutuallyExclusive.Count > 0)
        {
            writer.BeginBlock("mutually_exclusive");
            foreach (var excId in focus.MutuallyExclusive)
                writer.WriteProperty("focus", excId);
            writer.EndBlock();
        }

        // Cancel if invalid
        if (focus.CancelIfInvalid)
            writer.WriteProperty("cancel_if_invalid", true);

        // Available if capitulated
        if (focus.AvailableIfCapitulated)
            writer.WriteProperty("available_if_capitulated", true);

        // Search filters
        foreach (var filter in focus.SearchFilters)
            writer.WriteProperty("search_filters", filter);

        // Trigger blocks
        if (focus.Available is not null)
            BlockEmitter.Emit("available", focus.Available, writer);

        if (focus.Bypass is not null)
            BlockEmitter.Emit("bypass", focus.Bypass, writer);

        // Effect blocks
        if (focus.SelectEffect is not null)
            BlockEmitter.Emit("select_effect", focus.SelectEffect, writer);

        if (focus.CompletionReward is not null)
            BlockEmitter.Emit("completion_reward", focus.CompletionReward, writer);

        if (focus.CancelEffect is not null)
            BlockEmitter.Emit("cancel_effect", focus.CancelEffect, writer);

        // AI
        if (focus.AiWillDo is not null)
            BlockEmitter.Emit("ai_will_do", focus.AiWillDo, writer);

        writer.EndBlock();
    }
}
