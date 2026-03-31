using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class ExampleiaHistory : CountryHistoryDefinition
{
    public override string Tag => "EXA";
    public override int? Capital => 42;
    public override string? Oob => "EXA_1936";
    public override double? FuelRatio => 0.5;
    public override int? Convoys => 20;
    public override int? ResearchSlots => 3;
    public override double? Stability => 0.6;
    public override double? WarSupport => 0.3;

    protected override void Politics(PoliticsScope p)
    {
        p.RulingParty("democratic")
         .LastElection("1934.1.1")
         .ElectionFrequency(48)
         .ElectionsAllowed(true);
    }

    protected override void Popularities(PopularityScope p)
    {
        p.Democratic(60)
         .Fascism(10)
         .Communism(15)
         .Neutrality(15);
    }

    protected override void Technologies(TechSetScope t)
    {
        t.Add("infantry_weapons", "infantry_weapons1", "tech_support",
              "tech_engineers", "gw_artillery", "interwar_fighter",
              "early_destroyer", "early_light_cruiser", "transport",
              "fuel_silos", "basic_train");
    }

    protected override void Ideas(IdeaSetScope i)
    {
        i.Add("volunteer_only", "civilian_economy", "export_focus", "culture_of_innovation");
    }

    protected override void Effects(EffectScope e)
    {
        e.Custom("recruit_character", "EXA_mikhail_groman");
        e.Custom("recruit_character", "EXA_vasily_lutov");
    }

    protected override void DateEntries(DateHistoryScope d)
    {
        d.At("1939.1.1", e =>
        {
            e.AddPoliticalPower(50);
            e.AddStability(0.05);
        });
    }
}
