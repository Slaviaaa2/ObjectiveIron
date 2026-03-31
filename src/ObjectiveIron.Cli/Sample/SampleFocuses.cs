using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//  Individual Focus Definitions — each is its own class
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

public class IndustrialEffort : FocusDefinition
{
    public override string Id => "EXA_industrial_effort";
    public override LocalizedText Title => new() { En = "Industrial Effort", Ja = "産業への注力" };
    public override LocalizedText Description => new() { En = "Expand our industrial base to support future endeavors.", Ja = "将来の取り組みを支援するため、産業基盤を拡大する。" };
    public override GfxSprite Icon => Icons.GenericProduction;
    public override FocusPosition Position => new(1, 0);

    protected override void CompletionReward(EffectScope e)
    {
        e.AddIdeas("EXA_economic_recovery");
        e.ScriptedEffect("EXA_standard_industrial_boost");
        e.AddBuildingConstruction(BuildingType.IndustrialComplex, 1, instantBuild: true);
    }
}

public class MilitaryExpansion : FocusDefinition
{
    public override string Id => "EXA_military_expansion";
    public override LocalizedText Title => new() { En = "Military Expansion", Ja = "軍備拡張" };
    public override LocalizedText Description => new() { En = "Strengthen our armed forces to defend our nation.", Ja = "我が国を防衛するため、軍隊を強化する。" };
    public override GfxSprite Icon => Icons.GenericArmyDoctrines;
    public override FocusPosition Position => new(5, 0);

    protected override void CompletionReward(EffectScope e)
    {
        e.AddIdeas("EXA_militarized_society");
        e.AddManpower(50000);
        e.AddWarSupport(0.1);
    }
}

public class ResearchPush : FocusDefinition
{
    public override string Id => "EXA_research_push";
    public override LocalizedText Title => new() { En = "Research Push", Ja = "研究推進" };
    public override LocalizedText Description => new() { En = "Invest heavily in new technologies.", Ja = "新技術に多額の投資を行う。" };
    public override GfxSprite Icon => Icons.Research;
    public override FocusPosition Position => new(3, 1);

    protected override void CompletionReward(EffectScope e)
    {
        e.AddResearchSlot(1);
        e.AddTechBonus("ahead_research", 100, 2);
    }

    protected override void AiWillDo(AiScope ai)
    {
        ai.Factor(1);
        ai.Modifier(2, t => t.Date(Operator.GreaterThan, "1937.1.1"));
    }
}

public class PoliticalManeuvering : FocusDefinition
{
    public override string Id => "EXA_political_maneuvering";
    public override LocalizedText Title => new() { En = "Political Maneuvering", Ja = "政治的駆け引き" };
    public override LocalizedText Description => new() { En = "Navigate the complex political landscape to gain an advantage.", Ja = "複雑な政治情勢を巧みに操り、優位に立つ。" };
    public override GfxSprite Icon => Icons.GenericDemandTerritory;
    public override FocusPosition Position => new(1, 2);
    public override int Cost => 7;

    protected override void Available(TriggerScope t)
    {
        t.HasWar(false);
        t.ScriptedTrigger("EXA_is_high_stability");
    }

    protected override void CompletionReward(EffectScope e)
    {
        e.AddPoliticalPower(150);
        e.AddStability(0.05);
        e.CountryEvent("example_events.1", days: 30);
        e.RecruitCharacter("EXA_mikhail_groman");
    }
}

public class WarPreparation : FocusDefinition
{
    public override string Id => "EXA_war_preparation";
    public override LocalizedText Title => new() { En = "War Preparation", Ja = "戦争準備" };
    public override LocalizedText Description => new() { En = "Prepare our nation for the inevitable conflict.", Ja = "避けられない紛争に向けて国家を準備する。" };
    public override GfxSprite Icon => Icons.GenericMajorWar;
    public override FocusPosition Position => new(5, 2);

    protected override void Available(TriggerScope t)
    {
        t.Not(n => n.HasCapitulated());
    }

    protected override void CompletionReward(EffectScope e)
    {
        e.AddWarSupport(0.15);
        e.RecruitCharacter("EXA_vasily_lutov");
        e.CustomBlock("add_building_construction", b => {
            b.Custom("type", "super_factory"); // INVALID BUILDING
            b.Custom("level", 1);
        });
        e.HiddenEffect(h => h.CountryEvent("example_events.2"));
    }
}

public class UltimateDecision : FocusDefinition
{
    public override string Id => "EXA_ultimate_decision";
    public override FocusPosition Position => new(3, 3);
    public override int Cost => 14;

    protected override void DynamicTitle(DynamicLocScope l)
    {
        l.If(t => t.HasGovernment("fascism"), new() { En = "Fascist Path", Ja = "ファシズムの道" });
        l.If(t => t.HasGovernment("communism"), new() { En = "Communist Path", Ja = "共産主義の道" });
        l.Default(new() { En = "Ultimate Decision", Ja = "究極の決断" });
    }

    protected override void DynamicDescription(DynamicLocScope l)
    {
        l.If(t => t.HasGovernment("fascism"), new() { En = "Lead the nation through strength.", Ja = "力による国家指導を行う。" });
        l.If(t => t.HasGovernment("communism"), new() { En = "Lead the workers to revolution.", Ja = "労働者を革命へと導く。" });
        l.Default(new() { En = "Make the final choice that will change the course of history.", Ja = "歴史の針路を変える最終決定を下す。" });
    }

    protected override void DynamicIcon(DynamicGfxScope i)
    {
        i.If(t => t.HasGovernment("fascism"), Icons.FascismRise);
        i.If(t => t.HasGovernment("communism"), Icons.CommunismRise);
        i.Default(Icons.StrikeAtDemocracy);
    }

    protected override void CompletionReward(EffectScope e)
    {
        e.AddPoliticalPower(300);
        e.AddIdeas("EXA_national_spirit");
    }
}

// ─── SAMPLE EVENTS ──────────────────────────────────────────────────────────

public class SampleEvent : EventDefinition
{
    public override string Id => "example_events.1";
    public override EventType Type => EventType.Country;
    
    public override LocalizedText Title => new() 
    { 
        En = "A Political Shift", 
        Ja = "政治的な変化" 
    };
    
    public override LocalizedText Description => new() 
    { 
        En = "Our recent political maneuvering has caused a significant shift in the nation's political landscape. We must decide how to proceed.", 
        Ja = "最近の政治工作により、国内の政治情勢に大きな変化が生じました。今後の対応を決定しなければなりません。" 
    };
    
    public override GfxSprite Picture => Icons.GenericDemandTerritory; 

    public override bool IsTriggeredOnly => true;

    protected override void Immediate(EffectScope e)
    {
        e.Custom("set_country_flag", "political_shift_occurred");
    }

    protected override void Options(EventOptionScope options)
    {
        options.Add("example_events.1.a", new() { En = "Embrace the change.", Ja = "変化を受け入れる。" }, o =>
        {
            o.SetAiChance(0.8);
            o.AddPoliticalPower(50);
            o.AddStability(0.05);
        });

        options.Add("example_events.1.b", new() { En = "Maintain the status quo.", Ja = "現状維持を図る。" }, o =>
        {
            o.SetAiChance(0.2);
            o.AddPoliticalPower(-20);
            o.AddStability(-0.02);
        });
    }
}
