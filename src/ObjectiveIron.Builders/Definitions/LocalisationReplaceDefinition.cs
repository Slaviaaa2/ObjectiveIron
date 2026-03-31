using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines localisation overrides that emit to localisation/replace/{language}/.
/// Use this to override vanilla localisation keys.
/// </summary>
public abstract class LocalisationReplaceDefinition
{
    /// <summary>Identifier used for the output filename.</summary>
    public abstract string Id { get; }

    protected abstract void Define(LocalisationReplaceScope s);

    public LocalisationReplaceBuild Build()
    {
        var scope = new LocalisationReplaceScope();
        Define(scope);
        return new LocalisationReplaceBuild(Id, scope.Build());
    }
}

public record LocalisationReplaceEntry(string Key, LocalizedText Text);
public record LocalisationReplaceBuild(string Id, IReadOnlyList<LocalisationReplaceEntry> Entries);

public class LocalisationReplaceScope
{
    private readonly List<LocalisationReplaceEntry> _entries = [];

    /// <summary>
    /// Override a vanilla localisation key with new text.
    /// </summary>
    public LocalisationReplaceScope Replace(string key, LocalizedText text)
    {
        _entries.Add(new LocalisationReplaceEntry(key, text));
        return this;
    }

    internal IReadOnlyList<LocalisationReplaceEntry> Build() => _entries;
}
