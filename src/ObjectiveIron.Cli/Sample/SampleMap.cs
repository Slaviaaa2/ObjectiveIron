using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleDefaultMap : DefaultMapDefinition
{
    public override int MaxProvinces => 20000;

    public override int[] SeaStarts => [5000, 5001, 5002];
    public override int[] LakeStarts => [9000];

    protected override void Continents(ContinentScope c)
    {
        c.Add("europe", 1, 2, 3, 4, 5, 100, 101, 102)
         .Add("asia", 200, 201, 202, 300, 301);
    }
}
