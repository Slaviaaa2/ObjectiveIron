using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;
using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// A single Event defined natively in C#.
/// Events have Namespaces (the prefix before the period in the Id).
/// </summary>
public abstract class EventDefinition
{
    /// <summary>
    /// The unique identifier of the event. Must contain a period separating the namespace and the actual ID (e.g. "example_namespace.1").
    /// </summary>
    public abstract string Id { get; }

    public virtual EventType Type => EventType.Country;

    public virtual LocalizedText Title => new();
    public virtual LocalizedText Description => new();

    public virtual GfxSprite? Picture => null;

    public virtual bool IsTriggeredOnly => true;
    public virtual bool FireOnlyOnce => false;
    public virtual bool Hidden => false;
    public virtual bool Major => false;

    // Optional Blocks
    protected virtual void Trigger(TriggerScope t) { }
    protected virtual void Immediate(EffectScope e) { }
    
    protected virtual void Options(EventOptionScope options) { }

    public Block? CompileTrigger()
    {
        var scope = new TriggerScope();
        Trigger(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    public Block? CompileImmediate()
    {
        var scope = new EffectScope();
        Immediate(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    public IReadOnlyList<EventOption> CompileOptions()
    {
        var scope = new EventOptionScope();
        Options(scope);

        var list = new List<EventOption>();
        foreach (var (identifier, name, configure) in scope.Options)
        {
            var builder = new EventOptionBuilder();
            configure?.Invoke(builder);

            list.Add(new EventOption
            {
                NameIdentifier = identifier,
                Trigger = builder.Trigger.Build().Entries.Count > 0 ? builder.Trigger.Build() : null,
                AiChance = builder.AiChance.Build().Entries.Count > 0 ? builder.AiChance.Build() : null,
                Effects = builder.Effects.Build().Entries.Count > 0 ? builder.Effects.Build() : null
            });
        }
        return list.AsReadOnly();
    }

    public IReadOnlyList<(string Identifier, LocalizedText Name)> CompileOptionLocalizations()
    {
        var scope = new EventOptionScope();
        Options(scope);
        
        var list = new List<(string, LocalizedText)>();
        foreach (var (identifier, name, _) in scope.Options)
        {
            if (name.HasAny)
                list.Add((identifier, name));
        }
        return list.AsReadOnly();
    }
}
