using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleOpinionModifiers : OpinionModifierDefinition
{
    public override string Id => "sample_opinion_modifiers";

    protected override void Define(OpinionModifierScope o)
    {
        o.Add("trade_partner_bonus", value: 20, decay: 1.0, months: 12, trade: true);
        o.Add("alliance_broken", value: -50, decay: 0.5, years: 5);
        o.Add("liberation_opinion", value: 100, months: 24);
        o.Add("border_conflict_penalty", value: -30, days: 180);
    }
}
