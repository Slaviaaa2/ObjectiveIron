using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

namespace ExampleMod;

public class Exampleia1936Oob : OobDefinition
{
    public override string FileName => "EXA_1936";

    protected override void Templates(TemplateScope t)
    {
        t.Add("Infantry Division", b => b
            .Regiment("infantry", 0, 0)
            .Regiment("infantry", 0, 1)
            .Regiment("infantry", 0, 2)
            .Regiment("infantry", 1, 0)
            .Regiment("infantry", 1, 1)
            .Regiment("infantry", 1, 2)
            .Support("engineer", 0, 0)
        );
    }

    protected override void Units(UnitScope u)
    {
        u.Division("Infantry Division", 3000, experience: 0.1)
         .Division("Infantry Division", 3001, experience: 0.1)
         .Division("Infantry Division", 3002);
    }
}
