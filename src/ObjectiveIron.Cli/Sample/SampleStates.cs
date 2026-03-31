using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

public class ExampleiaCapitalState : StateDefinition
{
    public override int Id => 42;
    public override string StateName => "Exampleton";
    public override LocalizedText? DisplayName => new() { En = "Exampleton", Ja = "エグザンプルトン" };
    public override int Manpower => 2500000;
    public override string StateCategory => "large_city";
    public override int[] Provinces => [3001, 3002, 3003, 3004, 3005];
    public override double? LocalSupplies => 0.0;

    protected override void Resources(ResourceScope r)
    {
        r.Steel(12).Aluminium(4);
    }

    protected override void History(StateHistoryScope h)
    {
        h.Owner("EXA")
         .AddCoreOf("EXA")
         .VictoryPoints(3001, 10)
         .Infrastructure(4)
         .IndustrialComplex(3)
         .ArmsFactory(2)
         .AirBase(2)
         .AntiAir(1)
         .NavalBase(3002, 3);
    }
}

public class ExampleiaRuralState : StateDefinition
{
    public override int Id => 43;
    public override string StateName => "Exampleshire";
    public override LocalizedText? DisplayName => new() { En = "Exampleshire", Ja = "エグザンプルシャー" };
    public override int Manpower => 450000;
    public override string StateCategory => "rural";
    public override int[] Provinces => [3010, 3011, 3012];

    protected override void Resources(ResourceScope r)
    {
        r.Oil(6);
    }

    protected override void History(StateHistoryScope h)
    {
        h.Owner("EXA")
         .AddCoreOf("EXA")
         .VictoryPoints(3010, 1)
         .Infrastructure(2)
         .IndustrialComplex(1);
    }
}
