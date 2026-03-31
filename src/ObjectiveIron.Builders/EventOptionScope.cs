using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders;

public class EventOptionBuilder
{
    internal readonly EffectScope Effects = new();
    internal readonly TriggerScope Trigger = new();
    internal readonly AiScope AiChance = new();

    public void AddTrigger(Action<TriggerScope> configure) => configure(Trigger);
    
    public void SetAiChance(double baseFactor, Action<AiScope>? configureModifiers = null)
    {
        AiChance.Factor(baseFactor);
        configureModifiers?.Invoke(AiChance);
    }
    
    // Delegate common EffectScope methods for convenience
    public void AddPoliticalPower(int amount) => Effects.AddPoliticalPower(amount);
    public void AddStability(double amount) => Effects.AddStability(amount);
    public void AddWarSupport(double amount) => Effects.AddWarSupport(amount);
    public void AddManpower(int amount) => Effects.AddManpower(amount);
    public void AddIdeas(params string[] ideas) => Effects.AddIdeas(ideas);
    public void RemoveIdeas(params string[] ideas) => Effects.RemoveIdeas(ideas);
    public void TriggerCountryEvent(string eventId, int? days = null, int? hours = null) => Effects.CountryEvent(eventId, days, hours);
    public void Custom(string key, string value) => Effects.Custom(key, value);
    public void Custom(string key, int value) => Effects.Custom(key, value);
    public void Custom(string key, double value) => Effects.Custom(key, value);
    public void Custom(string key, bool value) => Effects.Custom(key, value);
    public void CustomBlock(string key, Action<EffectScope> configure) => Effects.CustomBlock(key, configure);
}

public class EventOptionScope
{
    internal readonly List<(string Identifier, LocalizedText Name, Action<EventOptionBuilder>? Configure)> Options = [];

    /// <summary>
    /// Adds a new option to the event. The identifier becomes the translation key for the option button.
    /// Example: options.Add("example.1.a", new() { En = "Accept" }, o => o.AddPoliticalPower(100));
    /// </summary>
    public void Add(string identifier, LocalizedText name, Action<EventOptionBuilder>? configure = null)
    {
        Options.Add((identifier, name, configure));
    }
}
