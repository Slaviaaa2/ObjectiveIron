using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleEliteInfantry : SubUnitDefinition
{
    public override string Id => "elite_infantry";

    protected override void Define(SubUnitScope s)
    {
        s.Add("elite_infantry", u => u
            .Abbreviation("ELI")
            .Sprite("infantry")
            .MapIconCategory("infantry")
            .Priority(610)
            .AiPriority(250)
            .Active(false)
            .Type("infantry")
            .Group("infantry")
            .Categories("category_front_line", "category_light_infantry",
                         "category_all_infantry", "category_army")
            .CombatWidth(2)
            .MaxStrength(30)
            .MaxOrganisation(65)
            .DefaultMorale(0.35)
            .Manpower(1000)
            .TrainingTime(120)
            .Suppression(1.5)
            .Weight(0.5)
            .SupplyConsumption(0.07)
            .Defense(0.1)
            .Breakthrough(0.05)
            .Need("infantry_equipment", 120)
            .Need("support_equipment", 10)
            .Terrain("forest", attack: 0.1, defence: 0.1, movement: 0.05)
            .Terrain("urban", attack: 0.05, defence: 0.1)
        );
    }
}
