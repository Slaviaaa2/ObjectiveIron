using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ExampleMod;

public class NationalUnitySpirit : IdeaDefinition
{
    public override string Id => "EXA_national_unity";
    public override string Category => "country";
    public override LocalizedText Name => new()
    {
        En = "National Unity",
        Ja = "国民の団結"
    };
    public override LocalizedText Description => new()
    {
        En = "The people stand united behind their leaders.",
        Ja = "国民は指導者の下に一致団結している。"
    };

    protected override void Modifier(ModifierScope m)
    {
        m.StabilityFactor(0.1)
         .WarSupportFactor(0.05)
         .PoliticalPowerGain(0.5);
    }
}
