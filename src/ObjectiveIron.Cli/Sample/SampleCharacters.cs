using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

public class MikhailGroman : CharacterDefinition
{
    public override string Id => "EXA_mikhail_groman";
    public override LocalizedText Name => new() { En = "Mikhail Groman", Ja = "ミハイル・グローマン" };

    public override string PortraitLarge => "GFX_portrait_soviet_mikhail_groman";
    public override string PortraitSmall => "GFX_portrait_soviet_mikhail_groman_small";

    protected override void DefineRoles(RoleScope r)
    {
        // He starts as a country leader for the Neutrality (non-aligned) party
        r.AddCountryLeader("neutrality", "stabilizer_1");

        // He can also be hired as a political advisor
        r.AddAdvisor("political_advisor", cost: 150, "administrative_genius");
    }
}

public class VasilyLutov : CharacterDefinition
{
    public override string Id => "EXA_vasily_lutov";
    public override LocalizedText Name => new() { En = "Vasily Lutov", Ja = "ワシーリー・ルトフ" };

    public override string PortraitLarge => "GFX_portrait_russian_general_1";

    protected override void DefineRoles(RoleScope r)
    {
        // A simple general
        r.AddGeneral(skill: 3, attack: 3, defense: 2, "brilliant_strategist");
    }
}
