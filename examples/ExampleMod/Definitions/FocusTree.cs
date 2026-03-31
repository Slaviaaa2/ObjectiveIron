using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ExampleMod;

// ─── フォーカス定義 ──────────────────────────────────────────

public class IndustrialEffort : FocusDefinition
{
    public override string Id => "EXA_industrial_effort";
    public override LocalizedText Title => new() { En = "Industrial Effort", Ja = "産業への注力" };
    public override LocalizedText Description => new()
    {
        En = "Invest in our factories to build a strong industrial base.",
        Ja = "工場への投資で強力な産業基盤を構築する。"
    };
    public override GfxSprite Icon => Icons.GenericProduction;
    public override FocusPosition Position => new(0, 0);
    public override int Cost => 10;

    protected override void CompletionReward(EffectScope e)
    {
        e.AddBuildingConstruction(BuildingType.IndustrialComplex, 2, instantBuild: true);
        e.CountryEvent("exampleia_industry.1", days: 1);
    }
}

public class MilitaryBuildup : FocusDefinition
{
    public override string Id => "EXA_military_buildup";
    public override LocalizedText Title => new() { En = "Military Buildup", Ja = "軍備増強" };
    public override GfxSprite Icon => Icons.GenericArmyDoctrines;
    public override FocusPosition Position => new(3, 0);
    public override int Cost => 10;

    protected override void CompletionReward(EffectScope e)
    {
        e.AddManpower(50000);
        e.AddBuildingConstruction(BuildingType.ArmsFactory, 2, instantBuild: true);
    }
}

public class Diplomacy : FocusDefinition
{
    public override string Id => "EXA_diplomacy";
    public override LocalizedText Title => new() { En = "Diplomatic Overtures", Ja = "外交攻勢" };
    public override GfxSprite Icon => Icons.GenericPoliticalPressure;
    public override FocusPosition Position => new(1, 2);
    public override int Cost => 10;

    protected override void Available(TriggerScope t)
    {
        t.HasCompletedFocus("EXA_industrial_effort");
    }

    protected override void CompletionReward(EffectScope e)
    {
        e.AddPoliticalPower(100);
        e.AddStability(0.05);
    }
}

// ─── ツリー構造 ──────────────────────────────────────────────

public class ExampleiaFocusTree : FocusTreeDefinition
{
    public override string Id => "exampleia_focus";
    public override CountryTag Country => new("EXA");

    public IndustrialEffort IndustrialEffort { get; } = new();
    public MilitaryBuildup MilitaryBuildup { get; } = new();
    public Diplomacy Diplomacy { get; } = new();

    protected override void Structure(FocusGraph graph)
    {
        graph.Root(IndustrialEffort);
        graph.Root(MilitaryBuildup);
        graph.Add(Diplomacy).After(IndustrialEffort, MilitaryBuildup);
    }
}
