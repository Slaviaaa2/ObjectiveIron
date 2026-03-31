using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Builders.Types;

namespace ObjectiveIron.Cli.Sample;

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//  Focus Tree — just declare and connect
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

public class ExampleFocusTree : FocusTreeDefinition
{
    public override string Id => "example_focus_tree";
    public override CountryTag Country => Tags.GER;

    // ─── Focuses ───────────────────────────────────────────────────
    public IndustrialEffort IndustrialEffort { get; } = new();
    public MilitaryExpansion MilitaryExpansion { get; } = new();
    public ResearchPush ResearchPush { get; } = new();
    public PoliticalManeuvering PoliticalManeuvering { get; } = new();
    public WarPreparation WarPreparation { get; } = new();
    public UltimateDecision UltimateDecision { get; } = new();

    protected override void Structure(FocusGraph graph)
    {
        // Root focuses (automatically placed at top)
        graph.Root(IndustrialEffort);
        graph.Root(MilitaryExpansion);

        // Research comes after either root (OR)
        graph.Add(ResearchPush).After(IndustrialEffort, MilitaryExpansion);

        // Political path comes after industrial
        graph.Add(PoliticalManeuvering).After(IndustrialEffort);

        // War path comes after military, exclusive with political
        graph.Add(WarPreparation).After(MilitaryExpansion)
            .ExclusiveWith(PoliticalManeuvering);

        // Final focus needs research AND one of the paths
        graph.Add(UltimateDecision)
            .After(ResearchPush)
            .After(PoliticalManeuvering, WarPreparation);
    }
}
