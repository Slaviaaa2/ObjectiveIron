using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleMio : MioDefinition
{
    public override string Id => "sample_mio";

    protected override void Define(MioFileScope s)
    {
        s.Add("exa_arms_manufacturer", m => m
            .Name("exa_arms_manufacturer")
            .Icon("GFX_idea_generic_arms_manufacturer")
            .OwnerTag("EXA")
            .EquipmentType("infantry_equipment")
            .InitialModifier("build_cost_ic", -0.05)
            .Trait("improved_production", t => t
                .Icon("GFX_mio_trait_improved_production")
                .Modifier("build_cost_ic", -0.05)
            )
        );
    }
}

public class SampleScriptedGui : ScriptedGuiDefinition
{
    public override string Id => "sample_scripted_guis";

    protected override void Define(ScriptedGuiFileScope s)
    {
        s.Add("exa_custom_menu", g => g
            .Context("decision_category")
            .Window("exa_custom_window")
            .Visible(t => t.Custom("tag", "EXA"))
            .Effect("click_button", e => e.AddPoliticalPower(10))
            .Trigger("is_visible", t => t.HasWar(false))
        );
    }
}
