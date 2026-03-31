using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

// ─── Dynamic Idea ─────────────────────────────────────────────────
public class DynamicIdea : IdeaDefinition
{
    public override string Id => "dynamic_spirit";
    public override LocalizedText Name => "Dynamic Spirit";
    
    protected override void DynamicName(DynamicLocScope l)
    {
        l.If(t => t.HasGovernment("fascism"), "Fascist Spirit");
        l.If(t => t.HasGovernment("democratic"), "Democratic Spirit");
        l.Default("Neutral Spirit");
    }

    protected override void Modifier(ModifierScope m)
    {
        m.PoliticalPowerFactor(0.15);
    }
}

// ─── Dynamic Decision ─────────────────────────────────────────────
public class MyCategory : DecisionCategoryDefinition
{
    public override string Id => "my_custom_category";
    public override LocalizedText Name => "Custom Category";
}

public class DynamicDecision : DecisionDefinition
{
    public override string Id => "click_me_decision";
    public override LocalizedText Name => "Click Me Decision";
    public override string Category => "my_custom_category";
    public override int? Cost => 25;

    protected override void DynamicDescription(DynamicLocScope l)
    {
        l.If(t => t.HasWar(), "We are at war! Strike now!");
        l.Default("Focus on internal stability.");
    }

    protected override void CompleteEffect(EffectScope e)
    {
        e.AddPoliticalPower(50);
    }
}

// ─── Scripted Effect ──────────────────────────────────────────────
public class GiveBonusEffect : ScriptedEffectDefinition
{
    public override string Id => "give_bonus_effect";

    protected override void Body(EffectScope e)
    {
        e.Custom("add_command_power", 10);
    }
}

// ─── Character ────────────────────────────────────────────────────
public class MyGeneral : CharacterDefinition
{
    public override string Id => "GEN_001";
    public override LocalizedText Name => "General Test";
    public override string PortraitLarge => "GFX_portrait_dummy";

    protected override void DefineRoles(RoleScope r)
    {
        r.AddGeneral(skill: 3, attack: 2, defense: 4);
    }
}
