using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines a game bookmark (start date scenario).
/// Emits to: common/bookmarks/{Id}.txt
/// </summary>
public abstract class BookmarkDefinition
{
    /// <summary>Bookmark ID / filename.</summary>
    public abstract string Id { get; }

    /// <summary>Localization key for name.</summary>
    public abstract string Name { get; }

    /// <summary>Localization key for description.</summary>
    public abstract string Desc { get; }

    /// <summary>Start date (e.g., "1936.1.1.12").</summary>
    public abstract string Date { get; }

    /// <summary>Picture GFX reference.</summary>
    public virtual string? Picture => null;

    /// <summary>Default country tag to select.</summary>
    public virtual string? DefaultCountry => null;

    /// <summary>Localized display name.</summary>
    public virtual LocalizedText? DisplayName => null;

    /// <summary>Localized description.</summary>
    public virtual LocalizedText? DisplayDesc => null;

    /// <summary>Define featured countries.</summary>
    protected abstract void Define(BookmarkScope s);

    public BookmarkBuildResult Build()
    {
        var scope = new BookmarkScope();
        Define(scope);
        return new BookmarkBuildResult(Id, Name, Desc, Date, Picture, DefaultCountry, scope.Build());
    }
}

public record BookmarkBuildResult(
    string Id,
    string Name,
    string Desc,
    string Date,
    string? Picture,
    string? DefaultCountry,
    IReadOnlyList<BookmarkCountryEntry> Countries
);

public class BookmarkScope
{
    private readonly List<BookmarkCountryEntry> _countries = [];

    public BookmarkScope Country(string tag, string? history = null, string? ideology = null,
        string[]? ideas = null, string[]? focuses = null)
    {
        _countries.Add(new BookmarkCountryEntry(tag, history, ideology,
            ideas?.Length > 0 ? ideas.ToList().AsReadOnly() : null,
            focuses?.Length > 0 ? focuses.ToList().AsReadOnly() : null));
        return this;
    }

    internal IReadOnlyList<BookmarkCountryEntry> Build() => _countries.AsReadOnly();
}

public record BookmarkCountryEntry(
    string Tag,
    string? History,
    string? Ideology,
    IReadOnlyList<string>? Ideas,
    IReadOnlyList<string>? Focuses
);
