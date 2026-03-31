namespace ObjectiveIron.Core.Models.Primitives;

/// <summary>
/// Multi-language text container for HoI4 localisation.
/// Set properties for each language you want to support.
/// Any language left null will not be emitted.
/// 
/// <example>
/// <code>
/// public override LocalizedText Title => new()
/// {
///     En = "Industrial Effort",
///     Ja = "産業化への努力",
///     De = "Industrielle Bemühung",
/// };
/// </code>
/// </example>
/// </summary>
public sealed class LocalizedText
{
    /// <summary>English</summary>
    public string? En { get; init; }

    /// <summary>Japanese / 日本語</summary>
    public string? Ja { get; init; }

    /// <summary>French / Français</summary>
    public string? Fr { get; init; }

    /// <summary>German / Deutsch</summary>
    public string? De { get; init; }

    /// <summary>Spanish / Español</summary>
    public string? Es { get; init; }

    /// <summary>Portuguese / Português (Brazilian)</summary>
    public string? Pt { get; init; }

    /// <summary>Russian / Русский</summary>
    public string? Ru { get; init; }

    /// <summary>Polish / Polski</summary>
    public string? Pl { get; init; }

    /// <summary>Chinese Simplified / 简体中文</summary>
    public string? Zh { get; init; }

    /// <summary>Korean / 한국어</summary>
    public string? Ko { get; init; }

    /// <summary>
    /// Create a simple English-only text.
    /// </summary>
    public static implicit operator LocalizedText(string englishText)
        => new() { En = englishText };

    /// <summary>
    /// Get all set languages as (languageCode, hoi4FolderName, hoi4Suffix, text) pairs.
    /// </summary>
    public IEnumerable<(string Code, string Folder, string Suffix, string Text)> GetEntries()
    {
        if (En is not null) yield return ("en", "english", "l_english", En);
        if (Ja is not null) yield return ("ja", "japanese", "l_japanese", Ja);
        if (Fr is not null) yield return ("fr", "french", "l_french", Fr);
        if (De is not null) yield return ("de", "german", "l_german", De);
        if (Es is not null) yield return ("es", "spanish", "l_spanish", Es);
        if (Pt is not null) yield return ("pt", "braz_por", "l_braz_por", Pt);
        if (Ru is not null) yield return ("ru", "russian", "l_russian", Ru);
        if (Pl is not null) yield return ("pl", "polish", "l_polish", Pl);
        if (Zh is not null) yield return ("zh", "chinese", "l_chinese", Zh);
        if (Ko is not null) yield return ("ko", "korean", "l_korean", Ko);
    }

    /// <summary>Check if any language is set.</summary>
    public bool HasAny => En is not null || Ja is not null || Fr is not null ||
                            De is not null || Es is not null || Pt is not null ||
                            Ru is not null || Pl is not null || Zh is not null ||
                            Ko is not null;
}
