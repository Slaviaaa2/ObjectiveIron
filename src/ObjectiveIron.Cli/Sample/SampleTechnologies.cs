using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Cli.Sample;

/// <summary>
/// Example: Basic research with equipment unlock and tech tree path.
/// </summary>
public class AtomicResearch : TechnologyDefinition
{
    public override string Id => "atomic_research";
    public override LocalizedText Name => new() { En = "Atomic Research", Ja = "原子力研究" };
    public override LocalizedText Description => new() { En = "Start the path towards the ultimate weapon.", Ja = "究極の兵器への道を歩み始める。" };
    public override int? StartYear => 1940;
    public override double? ResearchCost => 2.0;
    public override string Folder => "engineering_folder";
    public override (int X, int Y)? Position => (10, 10);

    protected override void Paths(PathScope p)
    {
        p.LeadsTo("nuclear_reactor");
    }

    protected override void Categories(CategoryScope c)
    {
        c.Add("nuclear");
    }

    protected override void Modifier(ModifierScope m)
    {
        m.ResearchSpeedFactor(0.02);
    }

    protected override void AiWillDo(AiScope ai)
    {
        ai.Factor(1);
    }
}

/// <summary>
/// Example: prerequisite chain with equipment unlocks.
/// </summary>
public class NuclearReactorTech : TechnologyDefinition
{
    public override string Id => "nuclear_reactor";
    public override LocalizedText Name => new() { En = "Nuclear Reactor", Ja = "原子炉" };
    public override int? StartYear => 1943;
    public override double? ResearchCost => 1.5;
    public override string Folder => "engineering_folder";
    public override (int X, int Y)? Position => (10, 12);

    protected override void Prerequisites(PrerequisiteScope p)
    {
        p.Require("atomic_research");
    }

    protected override void Paths(PathScope p)
    {
        p.LeadsTo("nuclear_bomb");
    }

    protected override void Categories(CategoryScope c)
    {
        c.Add("nuclear");
    }

    protected override void EnableEquipment(EquipmentScope e)
    {
        e.Unlock("nuclear_reactor_0");
    }

    protected override void AiWillDo(AiScope ai)
    {
        ai.Factor(2);
        ai.Modifier(10, t => t.IsMajor());
    }
}
