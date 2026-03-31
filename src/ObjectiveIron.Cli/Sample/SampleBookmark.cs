using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

public class SampleBookmark : BookmarkDefinition
{
    public override string Id => "sample_bookmark";
    public override string Name => "SAMPLE_BOOKMARK";
    public override string Desc => "SAMPLE_BOOKMARK_DESC";
    public override string Date => "1936.1.1.12";
    public override string? Picture => "GFX_select_date_1936";
    public override string? DefaultCountry => "EXA";
    public override LocalizedText? DisplayName => new() { En = "The Rise of Exampleia", Ja = "エグザンプリアの台頭" };
    public override LocalizedText? DisplayDesc => new() { En = "Exampleia stands at a crossroads.", Ja = "エグザンプリアは岐路に立っている。" };

    protected override void Define(BookmarkScope s)
    {
        s.Country("EXA",
            ideology: "democratic",
            ideas: ["volunteer_only", "civilian_economy", "export_focus"],
            focuses: ["industrial_effort", "research_push"]);
    }
}
