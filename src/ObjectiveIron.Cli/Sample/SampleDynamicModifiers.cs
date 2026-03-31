using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class WarExhaustionModifier : DynamicModifierDefinition
{
    public override string Id => "war_exhaustion_modifier";
    public override string? Icon => "GFX_idea_generic_war_exhaustion";

    protected override void Enable(TriggerScope t) => t.HasWar(true);
    protected override void RemoveTrigger(TriggerScope t) => t.HasWar(false);

    protected override void Modifiers(ModifierScope m)
    {
        m.StabilityFactor(-0.1);
        m.WarSupportFactor(-0.05);
        m.ConsumerGoodsFactor(0.05);
    }
}

public class IndustrialBoomModifier : DynamicModifierDefinition
{
    public override string Id => "industrial_boom";

    protected override void Modifiers(ModifierScope m)
    {
        m.IndustrialCapacityFactory(0.15);
        m.ConstructionSpeedFactor(0.1);
    }
}
