using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// A reusable scripted trigger that can be called from any TriggerScope.
/// Emission: /common/scripted_triggers/*.txt
/// </summary>
public abstract class ScriptedTriggerDefinition
{
    public abstract string Id { get; }
    
    protected abstract void Body(TriggerScope t);

    public Block Compile()
    {
        var scope = new TriggerScope();
        Body(scope);
        return scope.Build();
    }
}

/// <summary>
/// A reusable scripted effect that can be called from any EffectScope.
/// Emission: /common/scripted_effects/*.txt
/// </summary>
public abstract class ScriptedEffectDefinition
{
    public abstract string Id { get; }
    
    protected abstract void Body(EffectScope e);

    public Block Compile()
    {
        var scope = new EffectScope();
        Body(scope);
        return scope.Build();
    }
}
