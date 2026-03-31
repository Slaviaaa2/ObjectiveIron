using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines a continuous focus palette.
/// Emits to: common/continuous_focus/{Id}.txt
/// </summary>
public abstract class ContinuousFocusDefinition
{
    /// <summary>Palette ID.</summary>
    public abstract string Id { get; }

    /// <summary>Whether this is the default palette.</summary>
    public virtual bool Default => false;

    /// <summary>Whether to reset on civil war.</summary>
    public virtual bool ResetOnCivilWar => false;

    /// <summary>Country availability condition.</summary>
    protected virtual void Country(TriggerScope t) { }

    /// <summary>Define continuous focuses in this palette.</summary>
    protected abstract void Define(ContinuousFocusScope s);

    public ContinuousFocusPaletteBuild Build()
    {
        var country = new TriggerScope();
        Country(country);
        var countryBlock = country.Build();

        var scope = new ContinuousFocusScope();
        Define(scope);

        return new ContinuousFocusPaletteBuild(
            Id,
            Default,
            ResetOnCivilWar,
            countryBlock.Entries.Count > 0 ? countryBlock : null,
            scope.Build()
        );
    }
}

public record ContinuousFocusPaletteBuild(
    string Id,
    bool Default,
    bool ResetOnCivilWar,
    Block? Country,
    IReadOnlyList<ContinuousFocusEntry> Focuses
);

public class ContinuousFocusScope
{
    private readonly List<ContinuousFocusEntry> _focuses = [];

    public ContinuousFocusScope Add(string id, Action<ContinuousFocusBuilder> configure)
    {
        var builder = new ContinuousFocusBuilder(id);
        configure(builder);
        _focuses.Add(builder.Build());
        return this;
    }

    internal IReadOnlyList<ContinuousFocusEntry> Build() => _focuses.AsReadOnly();
}

public class ContinuousFocusBuilder
{
    private readonly string _id;
    private string? _icon;
    private double? _dailyCost;
    private bool _availableIfCapitulated;
    private string? _supportsAiStrategy;
    private Action<TriggerScope>? _available;
    private Action<TriggerScope>? _enable;
    private Action<ModifierScope>? _modifier;
    private Action<EffectScope>? _selectEffect;
    private Action<EffectScope>? _cancelEffect;

    public ContinuousFocusBuilder(string id) => _id = id;

    public ContinuousFocusBuilder Icon(string v) { _icon = v; return this; }
    public ContinuousFocusBuilder DailyCost(double v) { _dailyCost = v; return this; }
    public ContinuousFocusBuilder AvailableIfCapitulated(bool v) { _availableIfCapitulated = v; return this; }
    public ContinuousFocusBuilder SupportsAiStrategy(string v) { _supportsAiStrategy = v; return this; }
    public ContinuousFocusBuilder Available(Action<TriggerScope> a) { _available = a; return this; }
    public ContinuousFocusBuilder Enable(Action<TriggerScope> a) { _enable = a; return this; }
    public ContinuousFocusBuilder Modifier(Action<ModifierScope> a) { _modifier = a; return this; }
    public ContinuousFocusBuilder SelectEffect(Action<EffectScope> a) { _selectEffect = a; return this; }
    public ContinuousFocusBuilder CancelEffect(Action<EffectScope> a) { _cancelEffect = a; return this; }

    internal ContinuousFocusEntry Build()
    {
        Block? availBlock = null, enableBlock = null, modBlock = null, selBlock = null, canBlock = null;

        if (_available != null) { var s = new TriggerScope(); _available(s); availBlock = s.Build(); }
        if (_enable != null) { var s = new TriggerScope(); _enable(s); enableBlock = s.Build(); }
        if (_modifier != null) { var s = new ModifierScope(); _modifier(s); modBlock = s.Build(); }
        if (_selectEffect != null) { var s = new EffectScope(); _selectEffect(s); selBlock = s.Build(); }
        if (_cancelEffect != null) { var s = new EffectScope(); _cancelEffect(s); canBlock = s.Build(); }

        return new ContinuousFocusEntry(
            _id, _icon, _dailyCost, _availableIfCapitulated, _supportsAiStrategy,
            availBlock, enableBlock, modBlock, selBlock, canBlock
        );
    }
}

public record ContinuousFocusEntry(
    string Id,
    string? Icon,
    double? DailyCost,
    bool AvailableIfCapitulated,
    string? SupportsAiStrategy,
    Block? Available,
    Block? Enable,
    Block? Modifier,
    Block? SelectEffect,
    Block? CancelEffect
);
