using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits ScriptedLocalisation ast items into the common/scripted_localisation folder.
/// 
/// Syntax:
/// <code>
/// defined_text = {
///     name = Get_Macro_Name
///     text = {
///         trigger = { has_war = yes }
///         localization_key = some_key
///     }
///     text = {
///         localization_key = some_key_default
///     }
/// }
/// </code>
/// </summary>
public static class ScriptedLocalisationEmitter
{
    public static void Emit(IEnumerable<ScriptedLocalisation> locs, ClausewitzWriter writer)
    {
        foreach (var loc in locs)
        {
            writer.BeginBlock("defined_text");
            writer.WriteProperty("name", loc.Name);

            foreach (var text in loc.Texts)
            {
                writer.BeginBlock("text");
                if (text.Trigger is not null)
                {
                    BlockEmitter.Emit("trigger", text.Trigger, writer);
                }
                writer.WriteProperty("localization_key", text.LocalizationKey);
                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}
