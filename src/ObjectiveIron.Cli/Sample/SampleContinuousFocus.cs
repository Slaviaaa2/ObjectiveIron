using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class ExampleiaContinuousFocus : ContinuousFocusDefinition
{
    public override string Id => "exampleia_continuous";
    public override bool Default => false;

    protected override void Country(TriggerScope t)
    {
        t.Custom("tag", "EXA");
    }

    protected override void Define(ContinuousFocusScope s)
    {
        s.Add("exa_continuous_industrial", f => f
            .Icon("GFX_goal_generic_construct_civ_factory")
            .DailyCost(1)
            .Modifier(m =>
            {
                m.ProductionSpeedIndustrialComplexFactor(0.1);
            })
        );

        s.Add("exa_continuous_research", f => f
            .Icon("GFX_goal_generic_scientific_exchange")
            .DailyCost(1)
            .Modifier(m =>
            {
                m.ResearchSpeedFactor(0.05);
            })
            .AvailableIfCapitulated(true)
        );
    }
}
