using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ExampleMod;

public class ExampleiaMainland : StateDefinition
{
    public override int Id => 800;
    public override string StateName => "Exampleia";
    public override int Manpower => 2_000_000;
    public override string StateCategory => "city";
    public override int[] Provinces => [3000, 3001, 3002, 3003];

    public override LocalizedText? DisplayName => new()
    {
        En = "Exampleia",
        Ja = "エグザンプリア"
    };

    protected override void Resources(ResourceScope r)
    {
        r.Steel(20).Aluminium(5);
    }

    protected override void History(StateHistoryScope h)
    {
        h.Owner("EXA")
         .AddCoreOf("EXA")
         .VictoryPoints(3000, 10)
         .Infrastructure(4)
         .IndustrialComplex(2)
         .ArmsFactory(1)
         .AirBase(2);
    }
}
