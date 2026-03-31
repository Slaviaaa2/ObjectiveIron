using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleOnActions : OnActionDefinition
{
    public override string Id => "sample_on_actions";

    protected override void Define(OnActionScope a)
    {
        a.OnStartup(e =>
        {
            e.Custom("log", "[ObjectiveIron] Mod loaded successfully");
        });

        a.OnWarDeclaration(e =>
        {
            e.AddWarSupport(0.05);
        });

        a.OnGovernmentChange(e =>
        {
            e.CountryEvent("example_events.1", 1);
        });

        a.HookWithEvents("on_new_term_election", events =>
        {
            events.Random(90, "example_events.1");
        });
    }
}
