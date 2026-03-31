using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

public class HighStabilityTrigger : ScriptedTriggerDefinition
{
    public override string Id => "EXA_is_high_stability";

    protected override void Body(TriggerScope t)
    {
        t.HasStability(Operator.GreaterThanOrEqual, 0.7);
    }
}

public class StandardIndustrialBoost : ScriptedEffectDefinition
{
    public override string Id => "EXA_standard_industrial_boost";

    protected override void Body(EffectScope e)
    {
        e.AddBuildingConstruction(BuildingType.IndustrialComplex, 1);
        e.AddPoliticalPower(10);
    }
}
