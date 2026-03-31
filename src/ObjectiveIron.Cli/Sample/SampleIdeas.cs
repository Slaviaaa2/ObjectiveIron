using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

public class EconomicRecoverySpirit : IdeaDefinition
{
    public override string Id => "EXA_economic_recovery";
    
    public override LocalizedText Name => new()
    {
        En = "Economic Recovery",
        Ja = "経済復興"
    };

    public override LocalizedText Description => new()
    {
        En = "Our nation is slowly recovering from the recent crisis.",
        Ja = "我が国は最近の危機から徐々に回復しています。"
    };

    public override string Picture => "GFX_idea_generic_production";

    protected override void Modifier(ModifierScope m)
    {
        m.StabilityFactor(0.05);
        m.IndustrialCapacityFactory(0.1);
        m.ConsumerGoodsFactor(-0.05);
    }
}

public class MilitarizedSocietySpirit : IdeaDefinition
{
    public override string Id => "EXA_militarized_society";
    
    public override LocalizedText Name => new()
    {
        En = "Militarized Society",
        Ja = "軍事化社会"
    };

    protected override void Modifier(ModifierScope m)
    {
        m.WarSupportFactor(0.1);
        m.PoliticalPowerGain(0.15);
        m.ResearchSpeedFactor(0.05);
        m.ExperienceGainArmy(0.2);
    }
}
