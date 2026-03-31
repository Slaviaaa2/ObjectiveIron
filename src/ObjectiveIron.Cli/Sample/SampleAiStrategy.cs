using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class ExampleiaAiStrategy : AiStrategyDefinition
{
    public override string Id => "EXA";

    protected override void Define(AiStrategyFileScope s)
    {
        s.Add("EXA_build_industry", e => e
            .Allowed(t => t.Custom("original_tag", "EXA"))
            .Enable(t => t.Custom("date", "< 1938.1.1"))
            .Strategy("building_target", "industrial_complex", 100)
        );

        s.Add("EXA_avoid_war", e => e
            .Allowed(t => t.Custom("original_tag", "EXA"))
            .Enable(t => t.HasWar(false))
            .Abort(t => t.HasWar(true))
            .Strategy("declare_war", "EXA", -200)
        );
    }
}
