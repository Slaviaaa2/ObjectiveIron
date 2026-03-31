using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Compiler;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;
using ObjectiveIron.Emitter;
using ObjectiveIron.Emitter.Emitters;

namespace ObjectiveIron.Emitter.Tests;

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//  Test Focus Definitions
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

public class EmitFocusA : FocusDefinition
{
    public override string Id => "test_focus";
    public override GfxSprite Icon => new("GFX_test");
}

public class EmitFocusB : FocusDefinition
{
    public override string Id => "focus_b";
}

public class EmitFocusWithTrigger : FocusDefinition
{
    public override string Id => "conditioned";

    protected override void Available(TriggerScope t)
    {
        t.HasWar(false);
        t.HasPoliticalPower(Operator.GreaterThan, 50);
    }
}

public class EmitFocusWithReward : FocusDefinition
{
    public override string Id => "reward_focus";

    protected override void CompletionReward(EffectScope e)
    {
        e.AddPoliticalPower(100);
        e.HiddenEffect(h => h.CountryEvent("test.1", days: 5));
    }
}

public class EmitFocusWithAi : FocusDefinition
{
    public override string Id => "ai_focus";

    protected override void AiWillDo(AiScope ai)
    {
        ai.Factor(1);
        ai.Modifier(2, t => t.IsMajor());
    }
}

public class EmitFocusWithNot : FocusDefinition
{
    public override string Id => "not_test";

    protected override void Available(TriggerScope t)
    {
        t.Not(n => n.HasCapitulated());
    }
}

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//  Test Trees
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

public class SimpleEmitTree : FocusTreeDefinition
{
    public override string Id => "test_tree";
    public override CountryTag Country => Tags.GER;
    public EmitFocusA Focus { get; } = new();
    protected override void Structure(FocusGraph graph) { graph.Root(Focus); }
}

public class PrereqEmitTree : FocusTreeDefinition
{
    public override string Id => "test";
    public override CountryTag Country => Tags.GER;
    public EmitFocusA A { get; } = new();
    public EmitFocusB B { get; } = new();
    protected override void Structure(FocusGraph graph)
    {
        graph.Root(A);
        graph.Add(B).After(A);
    }
}

public class ExclusiveEmitTree : FocusTreeDefinition
{
    public override string Id => "test";
    public override CountryTag Country => Tags.GER;
    public EmitFocusA PathA { get; } = new();
    public EmitFocusB PathB { get; } = new();
    protected override void Structure(FocusGraph graph)
    {
        graph.Root(PathA);
        graph.Root(PathB).ExclusiveWith(PathA);
    }
}

public class TriggerEmitTree : FocusTreeDefinition
{
    public override string Id => "test";
    public override CountryTag Country => Tags.GER;
    public EmitFocusWithTrigger Focus { get; } = new();
    protected override void Structure(FocusGraph graph) { graph.Root(Focus); }
}

public class RewardEmitTree : FocusTreeDefinition
{
    public override string Id => "test";
    public override CountryTag Country => Tags.GER;
    public EmitFocusWithReward Focus { get; } = new();
    protected override void Structure(FocusGraph graph) { graph.Root(Focus); }
}

public class AiEmitTree : FocusTreeDefinition
{
    public override string Id => "test";
    public override CountryTag Country => Tags.GER;
    public EmitFocusWithAi Focus { get; } = new();
    protected override void Structure(FocusGraph graph) { graph.Root(Focus); }
}

public class NotEmitTree : FocusTreeDefinition
{
    public override string Id => "test";
    public override CountryTag Country => Tags.GER;
    public EmitFocusWithNot Focus { get; } = new();
    protected override void Structure(FocusGraph graph) { graph.Root(Focus); }
}

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//  Tests
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

public class FocusTreeEmitterTests
{
    [Fact]
    public void Emit_SimpleFocus_CorrectOutput()
    {
        var output = CompileAndEmit(new SimpleEmitTree());

        Assert.Contains("focus_tree = {", output);
        Assert.Contains("id = test_tree", output);
        Assert.Contains("tag = GER", output);
        Assert.Contains("id = test_focus", output);
        Assert.Contains("icon = GFX_test", output);
        Assert.Contains("cost = 10", output);
    }

    [Fact]
    public void Emit_Prerequisite_CorrectOutput()
    {
        var output = CompileAndEmit(new PrereqEmitTree());

        Assert.Contains("prerequisite = {", output);
        Assert.Contains("focus = test_focus", output);
    }

    [Fact]
    public void Emit_MutuallyExclusive_CorrectOutput()
    {
        var output = CompileAndEmit(new ExclusiveEmitTree());

        Assert.Contains("mutually_exclusive = {", output);
        Assert.Contains("focus = test_focus", output);
    }

    [Fact]
    public void Emit_AvailableTrigger_CorrectOutput()
    {
        var output = CompileAndEmit(new TriggerEmitTree());

        Assert.Contains("available = {", output);
        Assert.Contains("has_war = no", output);
        Assert.Contains("has_political_power > 50", output);
    }

    [Fact]
    public void Emit_CompletionReward_WithHiddenEffect()
    {
        var output = CompileAndEmit(new RewardEmitTree());

        Assert.Contains("completion_reward = {", output);
        Assert.Contains("add_political_power = 100", output);
        Assert.Contains("hidden_effect = {", output);
        Assert.Contains("country_event = {", output);
        Assert.Contains("id = test.1", output);
        Assert.Contains("days = 5", output);
    }

    [Fact]
    public void Emit_AiWillDo_WithModifier()
    {
        var output = CompileAndEmit(new AiEmitTree());

        Assert.Contains("ai_will_do = {", output);
        Assert.Contains("factor = 1", output);
        Assert.Contains("modifier = {", output);
        Assert.Contains("factor = 2", output);
        Assert.Contains("is_major = yes", output);
    }

    [Fact]
    public void Emit_DefaultNo_Present()
    {
        var output = CompileAndEmit(new SimpleEmitTree());
        Assert.Contains("default = no", output);
    }

    [Fact]
    public void Emit_NotTrigger_CorrectNesting()
    {
        var output = CompileAndEmit(new NotEmitTree());

        Assert.Contains("NOT = {", output);
        Assert.Contains("has_capitulated = yes", output);
    }

    private static string CompileAndEmit(FocusTreeDefinition definition)
    {
        var compiled = ModCompiler.Compile(definition);
        Assert.True(compiled.IsValid, $"Compilation failed: {compiled.Validation}");

        using var stringWriter = new StringWriter();
        using var writer = new ClausewitzWriter(stringWriter, new WriterOptions { DisposeWriter = false });
        FocusTreeEmitter.Emit(compiled.Model, writer);
        return stringWriter.ToString();
    }
}
