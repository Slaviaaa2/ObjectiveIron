using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Base class for defining on_action hooks.
/// </summary>
public abstract class OnActionDefinition
{
    public abstract string Id { get; }

    protected abstract void Define(OnActionScope a);

    public IReadOnlyList<OnAction> Build()
    {
        var scope = new OnActionScope();
        Define(scope);
        return scope.Build();
    }
}

/// <summary>
/// Scope for defining on_action hooks.
/// </summary>
public class OnActionScope
{
    private readonly List<OnAction> _actions = [];

    // ─── Common Hooks ───────────────────────────────────────────

    public OnActionScope OnStartup(Action<EffectScope> configure)
        => AddEffectHook("on_startup", configure);

    public OnActionScope OnWarDeclaration(Action<EffectScope> configure)
        => AddEffectHook("on_war_declaration", configure);

    public OnActionScope OnPeace(Action<EffectScope> configure)
        => AddEffectHook("on_peace", configure);

    public OnActionScope OnJustifyWarGoal(Action<EffectScope> configure)
        => AddEffectHook("on_justify_wargoal", configure);

    public OnActionScope OnGovernmentChange(Action<EffectScope> configure)
        => AddEffectHook("on_government_change", configure);

    public OnActionScope OnCoupSucceeded(Action<EffectScope> configure)
        => AddEffectHook("on_coup_succeeded", configure);

    public OnActionScope OnUnitLeaderCreated(Action<EffectScope> configure)
        => AddEffectHook("on_unit_leader_created", configure);

    public OnActionScope OnCapitulation(Action<EffectScope> configure)
        => AddEffectHook("on_capitulation", configure);

    public OnActionScope OnAnnex(Action<EffectScope> configure)
        => AddEffectHook("on_annex", configure);

    // ─── Election Hooks (with random events) ────────────────────

    public OnActionScope OnNewTermElection(Action<RandomEventScope> configure)
        => AddRandomEventHook("on_new_term_election", configure);

    // ─── Generic Hook ───────────────────────────────────────────

    public OnActionScope Hook(string hookName, Action<EffectScope> configure)
        => AddEffectHook(hookName, configure);

    public OnActionScope HookWithEvents(string hookName, Action<RandomEventScope> configure)
        => AddRandomEventHook(hookName, configure);

    public OnActionScope HookWithEffectAndEvents(string hookName, Action<EffectScope> effect, Action<RandomEventScope> events)
    {
        var effectScope = new EffectScope();
        effect(effectScope);
        var effectBlock = effectScope.Build();

        var eventScope = new RandomEventScope();
        events(eventScope);
        var randomEvents = eventScope.Build();

        _actions.Add(new OnAction(
            hookName,
            effectBlock.Entries.Count > 0 ? effectBlock : null,
            randomEvents.Count > 0 ? randomEvents : null
        ));
        return this;
    }

    // ─── Internal ───────────────────────────────────────────────

    private OnActionScope AddEffectHook(string hook, Action<EffectScope> configure)
    {
        var scope = new EffectScope();
        configure(scope);
        var block = scope.Build();
        _actions.Add(new OnAction(hook, block.Entries.Count > 0 ? block : null));
        return this;
    }

    private OnActionScope AddRandomEventHook(string hook, Action<RandomEventScope> configure)
    {
        var scope = new RandomEventScope();
        configure(scope);
        var events = scope.Build();
        _actions.Add(new OnAction(hook, RandomEvents: events.Count > 0 ? events : null));
        return this;
    }

    internal IReadOnlyList<OnAction> Build() => _actions.AsReadOnly();
}

/// <summary>
/// Scope for defining weighted random event entries.
/// </summary>
public class RandomEventScope
{
    private readonly List<RandomEvent> _events = [];

    public RandomEventScope Random(int weight, string eventId)
    {
        _events.Add(new RandomEvent(weight, eventId));
        return this;
    }

    internal IReadOnlyList<RandomEvent> Build() => _events.AsReadOnly();
}
