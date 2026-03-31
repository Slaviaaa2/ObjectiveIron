using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines a country's starting history configuration.
/// Emits to: history/countries/{TAG} - {TAG}.txt
/// </summary>
public abstract class CountryHistoryDefinition
{
    /// <summary>3-letter country tag (e.g., "GER", "SOV").</summary>
    public abstract string Tag { get; }

    /// <summary>Capital state ID.</summary>
    public virtual int? Capital => null;

    /// <summary>OOB filename (without extension). e.g., "GER_1936"</summary>
    public virtual string? Oob => null;

    /// <summary>Naval OOB filename.</summary>
    public virtual string? NavalOob => null;

    /// <summary>Air OOB filename.</summary>
    public virtual string? AirOob => null;

    /// <summary>Starting fuel ratio (0.0 - 1.0).</summary>
    public virtual double? FuelRatio => null;

    /// <summary>Starting convoys.</summary>
    public virtual int? Convoys => null;

    /// <summary>Starting research slots.</summary>
    public virtual int? ResearchSlots => null;

    /// <summary>Starting stability.</summary>
    public virtual double? Stability => null;

    /// <summary>Starting war support.</summary>
    public virtual double? WarSupport => null;

    /// <summary>Define politics (ruling_party, elections_allowed, etc.).</summary>
    protected virtual void Politics(PoliticsScope p) { }

    /// <summary>Define ideology popularities.</summary>
    protected virtual void Popularities(PopularityScope p) { }

    /// <summary>Define starting technologies.</summary>
    protected virtual void Technologies(TechSetScope t) { }

    /// <summary>Define starting ideas (national spirits, laws, etc.).</summary>
    protected virtual void Ideas(IdeaSetScope i) { }

    /// <summary>Define starting effects (recruit_character, add_equipment, etc.).</summary>
    protected virtual void Effects(EffectScope e) { }

    /// <summary>Define date-specific history entries (e.g., 1939.1.1 changes).</summary>
    protected virtual void DateEntries(DateHistoryScope d) { }

    /// <summary>Define conditional blocks (DLC checks, etc.).</summary>
    protected virtual void Conditionals(ConditionalScope c) { }

    public CountryHistoryBuildResult Build()
    {
        var politics = new PoliticsScope();
        Politics(politics);

        var popularities = new PopularityScope();
        Popularities(popularities);

        var techs = new TechSetScope();
        Technologies(techs);

        var ideas = new IdeaSetScope();
        Ideas(ideas);

        var effects = new EffectScope();
        Effects(effects);

        var dates = new DateHistoryScope();
        DateEntries(dates);

        var conditionals = new ConditionalScope();
        Conditionals(conditionals);

        return new CountryHistoryBuildResult(
            Tag,
            Capital,
            Oob,
            NavalOob,
            AirOob,
            FuelRatio,
            Convoys,
            ResearchSlots,
            Stability,
            WarSupport,
            politics.Build(),
            popularities.Build(),
            techs.Build(),
            ideas.Build(),
            effects.Build(),
            dates.Build(),
            conditionals.Build()
        );
    }
}

public record CountryHistoryBuildResult(
    string Tag,
    int? Capital,
    string? Oob,
    string? NavalOob,
    string? AirOob,
    double? FuelRatio,
    int? Convoys,
    int? ResearchSlots,
    double? Stability,
    double? WarSupport,
    PoliticsBuildResult? Politics,
    IReadOnlyList<PopularityEntry>? Popularities,
    IReadOnlyList<string>? Technologies,
    IReadOnlyList<string>? Ideas,
    Block? Effects,
    IReadOnlyList<DateHistoryEntry>? DateEntries,
    IReadOnlyList<ConditionalEntry>? Conditionals
);

// ─── Politics Scope ─────────────────────────────────────────────

public class PoliticsScope
{
    private string? _rulingParty;
    private int? _electionFrequency;
    private bool? _electionsAllowed;

    public PoliticsScope RulingParty(string ideology)
    {
        _rulingParty = ideology;
        return this;
    }

    public PoliticsScope ElectionsAllowed(bool value)
    {
        _electionsAllowed = value;
        return this;
    }

    private string? _lastElectionDate;

    public PoliticsScope LastElection(string date)
    {
        _lastElectionDate = date;
        return this;
    }

    public PoliticsScope ElectionFrequency(int months)
    {
        _electionFrequency = months;
        return this;
    }

    internal PoliticsBuildResult? Build()
    {
        if (_rulingParty == null && _electionsAllowed == null && _lastElectionDate == null && _electionFrequency == null)
            return null;
        return new PoliticsBuildResult(_rulingParty, _electionsAllowed, _lastElectionDate, _electionFrequency);
    }
}

public record PoliticsBuildResult(
    string? RulingParty,
    bool? ElectionsAllowed,
    string? LastElection,
    int? ElectionFrequency
);

// ─── Popularity Scope ───────────────────────────────────────────

public class PopularityScope
{
    private readonly List<PopularityEntry> _entries = [];

    public PopularityScope Set(string ideology, int percentage)
    {
        _entries.Add(new PopularityEntry(ideology, percentage));
        return this;
    }

    public PopularityScope Democratic(int percentage) => Set("democratic", percentage);
    public PopularityScope Fascism(int percentage) => Set("fascism", percentage);
    public PopularityScope Communism(int percentage) => Set("communism", percentage);
    public PopularityScope Neutrality(int percentage) => Set("neutrality", percentage);

    internal IReadOnlyList<PopularityEntry>? Build()
        => _entries.Count > 0 ? _entries.AsReadOnly() : null;
}

public record PopularityEntry(string Ideology, int Percentage);

// ─── Technology Set Scope ───────────────────────────────────────

public class TechSetScope
{
    private readonly List<string> _techs = [];

    public TechSetScope Add(string techId)
    {
        _techs.Add(techId);
        return this;
    }

    public TechSetScope Add(params string[] techIds)
    {
        _techs.AddRange(techIds);
        return this;
    }

    internal IReadOnlyList<string>? Build()
        => _techs.Count > 0 ? _techs.AsReadOnly() : null;
}

// ─── Idea Set Scope ─────────────────────────────────────────────

public class IdeaSetScope
{
    private readonly List<string> _ideas = [];

    public IdeaSetScope Add(string ideaId)
    {
        _ideas.Add(ideaId);
        return this;
    }

    public IdeaSetScope Add(params string[] ideaIds)
    {
        _ideas.AddRange(ideaIds);
        return this;
    }

    internal IReadOnlyList<string>? Build()
        => _ideas.Count > 0 ? _ideas.AsReadOnly() : null;
}

// ─── Date History Scope ─────────────────────────────────────────

public class DateHistoryScope
{
    private readonly List<DateHistoryEntry> _entries = [];

    public DateHistoryScope At(string date, Action<EffectScope> configure)
    {
        var scope = new EffectScope();
        configure(scope);
        _entries.Add(new DateHistoryEntry(date, scope.Build()));
        return this;
    }

    internal IReadOnlyList<DateHistoryEntry>? Build()
        => _entries.Count > 0 ? _entries.AsReadOnly() : null;
}

public record DateHistoryEntry(string Date, Block Effects);

// ─── Conditional Scope ──────────────────────────────────────────

public class ConditionalScope
{
    private readonly List<ConditionalEntry> _entries = [];

    public ConditionalScope If(Action<TriggerScope> limit, Action<EffectScope> then)
    {
        var triggerScope = new TriggerScope();
        limit(triggerScope);
        var effectScope = new EffectScope();
        then(effectScope);
        _entries.Add(new ConditionalEntry(triggerScope.Build(), effectScope.Build()));
        return this;
    }

    internal IReadOnlyList<ConditionalEntry>? Build()
        => _entries.Count > 0 ? _entries.AsReadOnly() : null;
}

public record ConditionalEntry(Block Limit, Block Effects);
