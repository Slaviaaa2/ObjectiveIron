using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ExampleMod;

// ─── 国家タグ・基本情報 ─────────────────────────────────────

public class ExampleiaCountry : CountryDefinition
{
    public override string Tag => "EXA";
    public override (int R, int G, int B) Color => (45, 120, 180);
    public override LocalizedText? Name => new() { En = "Exampleia", Ja = "エグザンプリア" };
    public override string GraphicalCulture => "western_european_gfx";
}

// ─── 国家ヒストリー ─────────────────────────────────────────

public class ExampleiaHistory : CountryHistoryDefinition
{
    public override string Tag => "EXA";
    public override int? Capital => 800;
    public override string? Oob => "EXA_1936";
    public override int? ResearchSlots => 3;
    public override double? Stability => 0.6;
    public override double? WarSupport => 0.3;

    protected override void Politics(PoliticsScope p)
    {
        p.RulingParty("neutrality")
         .ElectionsAllowed(true)
         .LastElection("1935.6.1")
         .ElectionFrequency(48);
    }

    protected override void Popularities(PopularityScope p)
    {
        p.Democratic(30)
         .Fascism(5)
         .Communism(10)
         .Neutrality(55);
    }

    protected override void Technologies(TechSetScope t)
    {
        t.Add("infantry_weapons", "tech_support", "tech_engineers");
    }

    protected override void Ideas(IdeaSetScope i)
    {
        i.Add("EXA_national_unity");
    }

    protected override void Effects(EffectScope e)
    {
        e.RecruitCharacter("EXA_leader_name");
    }
}
