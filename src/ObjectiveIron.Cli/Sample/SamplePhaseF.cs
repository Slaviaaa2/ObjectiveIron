using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleBuildings : BuildingDefinition
{
    public override string Id => "sample_buildings";

    protected override void Define(BuildingFileScope s)
    {
        s.Add("research_complex", b => b
            .MaxLevel(5)
            .BaseHealthPerLevel(1500)
            .Provincial(false)
            .Modifier("local_resources_factor", 0.1)
        );
    }
}

public class SampleGameRules : GameRuleDefinition
{
    public override string Id => "sample_game_rules";

    protected override void Define(GameRuleFileScope s)
    {
        s.Add("exa_difficulty_mode", "normal", opts => opts
            .Option("easy", text: "EASY_MODE", allowAchievements: true)
            .Option("normal", text: "NORMAL_MODE", allowAchievements: true)
            .Option("hard", text: "HARD_MODE", allowAchievements: true)
        );
    }
}

public class SampleStateCategories : StateCategoryDefinition
{
    public override string Id => "sample_state_categories";

    protected override void Define(StateCategoryFileScope s)
    {
        s.Add("research_hub", 8);
        s.Add("industrial_center", 10);
    }
}

public class SampleIdeologies : IdeologyDefinition
{
    public override string Id => "sample_ideologies";

    protected override void Define(IdeologyFileScope s)
    {
        s.Add("technocracy", i => i
            .Color(100, 200, 150)
            .CanBeBoosted(true)
            .WarImpact(false)
            .SubType("technocratic_republic")
            .SubType("technocratic_monarchy")
            .Modifier("research_speed_factor", 0.05)
        );
    }
}
