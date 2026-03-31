using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Builders.Compiler;

/// <summary>
/// Compiles EventDefinition definitions into AST Event nodes.
/// </summary>
public static class EventCompiler
{
    public static Event Compile(EventDefinition def)
    {
        return new Event
        {
            Id = def.Id,
            Type = def.Type,
            Picture = def.Picture?.Value,
            IsTriggeredOnly = def.IsTriggeredOnly,
            FireOnlyOnce = def.FireOnlyOnce,
            Hidden = def.Hidden,
            Major = def.Major,
            Trigger = def.CompileTrigger(),
            Immediate = def.CompileImmediate(),
            Options = def.CompileOptions()
        };
    }
}
