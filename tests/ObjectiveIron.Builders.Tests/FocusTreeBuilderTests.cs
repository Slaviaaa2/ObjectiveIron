using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Compiler;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Tests;

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//  Test Focus Definitions
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

public class TestFocusA : FocusDefinition
{
    public override string Id => "test_a";
    public override GfxSprite Icon => Icons.GenericProduction;
}

public class TestFocusB : FocusDefinition
{
    public override string Id => "test_b";
}

public class TestFocusC : FocusDefinition
{
    public override string Id => "test_c";
    public override int Cost => 7;

    protected override void Available(TriggerScope t)
    {
        t.HasWar(false);
        t.HasPoliticalPower(Operator.GreaterThan, 100);
        t.Not(n => n.HasCapitulated());
    }

    protected override void CompletionReward(EffectScope e)
    {
        e.AddPoliticalPower(150);
        e.AddStability(0.05);
        e.CountryEvent("test.1", days: 30);
    }

    protected override void AiWillDo(AiScope ai)
    {
        ai.Factor(1);
        ai.Modifier(2, t => t.IsMajor());
    }
}

public class DuplicateFocus : FocusDefinition
{
    private readonly string _id;
    public DuplicateFocus(string id) { _id = id; }
    public override string Id => _id;
}

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//  Test Trees
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

public class SimpleTree : FocusTreeDefinition
{
    public override string Id => "simple_tree";
    public override CountryTag Country => Tags.GER;

    public TestFocusA FocusA { get; } = new();

    protected override void Structure(FocusGraph graph)
    {
        graph.Root(FocusA);
    }
}

public class MultiCountryTree : FocusTreeDefinition
{
    public override string Id => "multi_tree";
    public override CountryTag Country => Tags.GER;
    public override IReadOnlyList<CountryTag> AdditionalCountries => [Tags.ITA];
    public override int CountryPriority => 5;

    public TestFocusA FocusA { get; } = new();

    protected override void Structure(FocusGraph graph)
    {
        graph.Root(FocusA);
    }
}

public class PrerequisiteTree : FocusTreeDefinition
{
    public override string Id => "prereq_tree";
    public override CountryTag Country => Tags.GER;

    public TestFocusA RootA { get; } = new();
    public TestFocusB RootB { get; } = new();
    public TestFocusC Child { get; } = new();

    protected override void Structure(FocusGraph graph)
    {
        graph.Root(RootA);
        graph.Root(RootB);
        graph.Add(Child)
            .After(RootA)               // AND group 1
            .After(RootA, RootB);       // AND group 2 (OR within)
    }
}

public class ExclusiveTree2 : FocusTreeDefinition
{
    public override string Id => "exclusive_tree2";
    public override CountryTag Country => Tags.GER;

    public TestFocusA PathA { get; } = new();
    public TestFocusB PathB { get; } = new();

    protected override void Structure(FocusGraph graph)
    {
        graph.Root(PathA);
        graph.Root(PathB).ExclusiveWith(PathA);
    }
}

public class DuplicateIdTree : FocusTreeDefinition
{
    public override string Id => "dup_tree";
    public override CountryTag Country => Tags.GER;

    public DuplicateFocus Dup1 { get; } = new("dup");
    public DuplicateFocus Dup2 { get; } = new("dup");

    protected override void Structure(FocusGraph graph)
    {
        graph.Root(Dup1);
        graph.Root(Dup2);
    }
}

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//  Tests
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

public class FocusDefinitionTests
{
    [Fact]
    public void Compile_SimpleTree_Succeeds()
    {
        var result = ModCompiler.Compile(new SimpleTree());

        Assert.True(result.IsValid);
        Assert.Equal("simple_tree", result.Model.Id);
        Assert.Single(result.Model.Focuses);
        Assert.Equal("test_a", result.Model.Focuses[0].Id);
    }

    [Fact]
    public void Compile_MultipleCountries_AllRecorded()
    {
        var result = ModCompiler.Compile(new MultiCountryTree());

        Assert.True(result.IsValid);
        Assert.Equal(2, result.Model.Country.Modifiers.Count);
        Assert.Equal("GER", result.Model.Country.Modifiers[0].Tag);
        Assert.Equal(5, result.Model.Country.Modifiers[0].Add);
        Assert.Equal("ITA", result.Model.Country.Modifiers[1].Tag);
    }

    [Fact]
    public void Compile_Prerequisites_AND_OR_Groups()
    {
        var result = ModCompiler.Compile(new PrerequisiteTree());

        Assert.True(result.IsValid);
        var child = result.Model.Focuses[2];
        Assert.Equal(2, child.Prerequisites.Count);
        Assert.Single(child.Prerequisites[0]);           // group 1: just root_a
        Assert.Equal(2, child.Prerequisites[1].Count);   // group 2: root_a OR root_b
    }

    [Fact]
    public void Compile_MutuallyExclusive()
    {
        var result = ModCompiler.Compile(new ExclusiveTree2());

        Assert.True(result.IsValid);
        Assert.Single(result.Model.Focuses[1].MutuallyExclusive);
        Assert.Equal("test_a", result.Model.Focuses[1].MutuallyExclusive[0]);
    }

    [Fact]
    public void Compile_Triggers_FromOverride()
    {
        var result = ModCompiler.Compile(new PrerequisiteTree());

        var child = result.Model.Focuses[2]; // TestFocusC
        Assert.NotNull(child.Available);
        Assert.Equal(3, child.Available.Entries.Count);
    }

    [Fact]
    public void Compile_Effects_FromOverride()
    {
        var result = ModCompiler.Compile(new PrerequisiteTree());

        var child = result.Model.Focuses[2]; // TestFocusC
        Assert.NotNull(child.CompletionReward);
        Assert.Equal(3, child.CompletionReward.Entries.Count);
    }

    [Fact]
    public void Compile_AiWillDo_FromOverride()
    {
        var result = ModCompiler.Compile(new PrerequisiteTree());

        var child = result.Model.Focuses[2]; // TestFocusC
        Assert.NotNull(child.AiWillDo);
    }

    [Fact]
    public void Compile_DuplicateIds_ReturnsError()
    {
        var result = ModCompiler.Compile(new DuplicateIdTree());

        Assert.False(result.IsValid);
        Assert.Contains(result.Validation.Errors, e => e.Message.Contains("Duplicate"));
    }

    [Fact]
    public void Compile_IconFromGfxSprite()
    {
        var result = ModCompiler.Compile(new SimpleTree());

        Assert.Equal("GFX_goal_generic_production", result.Model.Focuses[0].Icon);
    }

    [Fact]
    public void Compile_CostFromOverride()
    {
        var result = ModCompiler.Compile(new PrerequisiteTree());

        var child = result.Model.Focuses[2]; // TestFocusC has Cost = 7
        Assert.Equal(7, child.Cost);
    }

    [Fact]
    public void AutoLayout_RootsAtY0()
    {
        var result = ModCompiler.Compile(new PrerequisiteTree());

        Assert.Equal(0, result.Model.Focuses[0].Y); // RootA
        Assert.Equal(0, result.Model.Focuses[1].Y); // RootB
    }

    [Fact]
    public void AutoLayout_ChildBelowParent()
    {
        var result = ModCompiler.Compile(new PrerequisiteTree());

        var child = result.Model.Focuses[2]; // TestFocusC
        Assert.Equal(1, child.Y); // should be below roots
    }

    [Fact]
    public void AutoLayout_RootsSpacedApart()
    {
        var result = ModCompiler.Compile(new PrerequisiteTree());

        var rootA = result.Model.Focuses[0];
        var rootB = result.Model.Focuses[1];
        Assert.NotEqual(rootA.X, rootB.X); // should be spaced apart
    }
}
