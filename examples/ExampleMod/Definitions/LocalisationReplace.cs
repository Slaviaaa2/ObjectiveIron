using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ExampleMod;

/// <summary>
/// バニラのローカライゼーションを上書きする例.
/// localisation/replace/{language}/ に出力されます.
/// </summary>
public class ExampleiaLocReplace : LocalisationReplaceDefinition
{
    public override string Id => "exampleia_replace";

    protected override void Define(LocalisationReplaceScope s)
    {
        // バニラのイデオロギー名を差し替え
        s.Replace("POLITICS_NEUTRALITY", new LocalizedText
        {
            En = "Monarchism",
            Ja = "君主制"
        });
    }
}
