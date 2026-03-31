using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

public class SampleCountryExampleia : CountryDefinition
{
    public override string Tag => "EXA";
    public override LocalizedText? Name => new() { En = "Exampleia", Ja = "エグザンプリア" };
    public override (int R, int G, int B) Color => (180, 60, 60);
    public override (int R, int G, int B)? ColorUi => (210, 80, 80);
    public override string GraphicalCulture => "western_european_gfx";
    public override string? GraphicalCulture2d => "western_european_2d";
}
