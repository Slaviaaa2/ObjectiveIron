using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Builders.Compiler;

public static class ScriptedCompiler
{
    public static ScriptedTrigger CompileTrigger(ScriptedTriggerDefinition def)
    {
        return new ScriptedTrigger
        {
            Id = def.Id,
            Body = def.Compile()
        };
    }

    public static ScriptedEffect CompileEffect(ScriptedEffectDefinition def)
    {
        return new ScriptedEffect
        {
            Id = def.Id,
            Body = def.Compile()
        };
    }
}
