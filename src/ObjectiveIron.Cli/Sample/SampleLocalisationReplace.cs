using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

public class SampleLocalisationReplace : LocalisationReplaceDefinition
{
    public override string Id => "exampleia_replace";

    protected override void Define(LocalisationReplaceScope s)
    {
        s.Replace("POLITICS_FASCISM", new LocalizedText
            {
                En = "Ultranationalism",
                Ja = "超国家主義"
            })
         .Replace("POLITICS_DEMOCRATIC", new LocalizedText
            {
                En = "Liberal Democracy",
                Ja = "自由民主主義"
            });
    }
}
