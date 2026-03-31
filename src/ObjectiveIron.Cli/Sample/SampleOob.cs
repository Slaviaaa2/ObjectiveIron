using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class Exampleia1936Oob : OobDefinition
{
    public override string FileName => "EXA_1936";

    protected override void Templates(TemplateScope t)
    {
        t.Add("Infantry Division", d =>
        {
            d.Regiment("infantry", 0, 0)
             .Regiment("infantry", 0, 1)
             .Regiment("infantry", 0, 2)
             .Regiment("infantry", 1, 0)
             .Regiment("infantry", 1, 1)
             .Regiment("infantry", 1, 2)
             .Regiment("artillery_brigade", 2, 0)
             .Regiment("artillery_brigade", 2, 1)
             .Support("engineer", 0, 0)
             .Support("recon", 0, 1);
        });

        t.Add("Armored Division", d =>
        {
            d.Regiment("light_armor", 0, 0)
             .Regiment("light_armor", 0, 1)
             .Regiment("motorized", 1, 0)
             .Regiment("motorized", 1, 1)
             .Support("engineer", 0, 0)
             .Support("mot_recon", 0, 1);
        });
    }

    protected override void Units(UnitScope u)
    {
        u.Division("Infantry Division", 3001, experience: 0.3, nameOrder: 1)
         .Division("Infantry Division", 3001, experience: 0.2, nameOrder: 2)
         .Division("Infantry Division", 3002, experience: 0.1, nameOrder: 3)
         .Division("Armored Division", 3003, experience: 0.3, nameOrder: 1);
    }
}
