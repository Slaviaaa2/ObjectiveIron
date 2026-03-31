using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

public class PoliticalDecisions : DecisionCategoryDefinition
{
    public override string Id => "EXA_political_decisions";
    public override LocalizedText Name => new() { En = "Political Decisions", Ja = "政治的決断" };
    public override GfxSprite Icon => Icons.GenericDemandTerritory;
}

public class CommissionIndustrialReport : DecisionDefinition
{
    public override string Id => "EXA_commission_industrial_report";
    public override LocalizedText Name => new() { En = "Commission Industrial Report", Ja = "産業報告書の委託" };
    public override LocalizedText Description => new() { En = "Gather data on our current industrial capacity.", Ja = "現在の産業能力に関するデータを収集します。" };
    public override string Category => "EXA_political_decisions";

    public override int? Cost => 50;

    protected override void Available(TriggerScope t)
    {
        t.HasPoliticalPower(Operator.GreaterThanOrEqual, 50);
    }

    protected override void CompleteEffect(EffectScope e)
    {
        e.AddPoliticalPower(-50);
        e.AddStability(0.02);
        e.Custom("set_country_flag", "industrial_report_commissioned");
    }

    protected override void AiWillDo(AiScope ai)
    {
        ai.Factor(1);
    }
}
