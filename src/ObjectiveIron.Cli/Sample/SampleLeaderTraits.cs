using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleCountryLeaderTraits : CountryLeaderTraitDefinition
{
    public override string Id => "sample_country_traits";

    protected override void Define(LeaderTraitFileScope s)
    {
        s.Add("exa_industrialist", t => t
            .Modifier("production_speed_industrial_complex_factor", 0.1)
            .Modifier("consumer_goods_factor", -0.05)
        );
    }
}

public class SampleUnitLeaderTraits : UnitLeaderTraitDefinition
{
    public override string Id => "sample_unit_traits";

    protected override void Define(LeaderTraitFileScope s)
    {
        s.Add("exa_aggressive_commander", t => t
            .Type("corps_commander")
            .Attack(0.1)
            .Planning(-0.05)
            .Sprite("GFX_leader_trait_aggressive")
        );

        s.Add("exa_defensive_strategist", t => t
            .Type("field_marshal")
            .Defense(0.15)
            .Entrenchment(0.1)
            .Speed(-0.05)
        );
    }
}
