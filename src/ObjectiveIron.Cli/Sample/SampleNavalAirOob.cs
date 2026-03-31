using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class ExampleiaNavalOob : NavalOobDefinition
{
    public override string FileName => "EXA_1936_naval";

    protected override void Fleets(NavalFleetScope f)
    {
        f.Fleet("Exampleia Home Fleet", 3051, fleet => fleet
            .TaskForce("1st Patrol Group", 3051, tf => tf
                .Ship("EXA Defender", "destroyer", "destroyer_1", "Early Destroyer")
                .Ship("EXA Guardian", "destroyer", "destroyer_1", "Early Destroyer")
                .Ship("EXA Valiant", "light_cruiser", "light_cruiser_1", "Early Light Cruiser")
            )
        );
    }
}

public class ExampleiaAirOob : AirOobDefinition
{
    public override string FileName => "EXA_1936_air";

    protected override void AirWings(AirWingScope a)
    {
        a.Wing(800, wing => wing
            .Add("fighter_equipment", "fighter_equipment_0", 100, "1st Fighter Wing")
            .Add("tac_bomber_equipment", "tac_bomber_equipment_0", 36)
        );
    }
}
