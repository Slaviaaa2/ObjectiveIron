using System.Collections.Generic;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

/// <summary>
/// Represents a Scripted Localisation definition (defined_text) in HoI4.
/// It dynamically returns a localisation key based on runtime triggers.
/// 
/// output: common/scripted_localisation/00_my_dynamic_texts.txt
/// <code>
/// defined_text = {
///     name = Get_Dynamic_Focus_Title
///     text = {
///         trigger = { has_war = yes }
///         localization_key = my_focus_title_1
///     }
///     text = {
///         localization_key = my_focus_title_default
///     }
/// }
/// </code>
/// </summary>
public sealed class ScriptedLocalisation
{
    /// <summary>The name of the defined_text macro (e.g., Get_Dynamic_Title)</summary>
    public required string Name { get; init; }
    
    /// <summary>The text definitions evaluating from top to bottom.</summary>
    public IReadOnlyList<ScriptedLocalisationText> Texts { get; init; } = [];
}

/// <summary>
/// Represents a single text evaluation block inside a defined_text.
/// </summary>
public sealed class ScriptedLocalisationText
{
    /// <summary>Trigger block to evaluate. Null means it's the fallback/default.</summary>
    public Block? Trigger { get; init; }
    
    /// <summary>The localisation key to resolve if the trigger evaluates to true.</summary>
    public required string LocalizationKey { get; init; }
}
