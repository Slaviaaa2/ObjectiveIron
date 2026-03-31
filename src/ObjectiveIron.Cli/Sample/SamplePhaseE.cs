using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleOccupationLaws : OccupationLawDefinition
{
    public override string Id => "sample_occupation_laws";

    protected override void Define(OccupationLawFileScope s)
    {
        s.Add("exa_harsh_occupation", l => l
            .StateFrame(1)
            .GuiOrder(1)
            .Modifier("resistance_target", -0.3)
            .Modifier("compliance_growth", 0.05)
            .Modifier("resistance_damage_to_garrison", 0.1)
        );
    }
}

public class SampleWargoals : WargoalDefinition
{
    public override string Id => "sample_wargoals";

    protected override void Define(WargoalFileScope s)
    {
        s.Add("exa_liberation_war", wg => wg
            .WarDesirability(50)
            .WorldTension(0.5)
            .JustifyWarGoalCost(10)
            .OnSuccess(e => e.AddWarSupport(0.1))
        );
    }
}

public class SampleOperations : OperationDefinition
{
    public override string Id => "sample_operations";

    protected override void Define(OperationFileScope s)
    {
        s.Add("exa_steal_blueprints", op => op
            .Icon("GFX_operation_steal_blueprints")
            .Days(60)
            .NetworkStrength(50)
            .OperativesNeeded(1)
            .Cost(50)
            .RiskChance(0.2)
            .Outcome(e => e.AddPoliticalPower(50))
        );
    }
}

public class SampleAbilities : AbilityDefinition
{
    public override string Id => "sample_abilities";

    protected override void Define(AbilityFileScope s)
    {
        s.Add("exa_forced_march", a => a
            .AbilityType("army_leader")
            .Icon("GFX_ability_forced_march")
            .Cost(5)
            .Duration(72)
            .Cooldown(168)
            .UnitModifier("speed_factor", 0.25)
            .UnitModifier("org_loss_when_moving", 0.5)
        );
    }
}
