using System.Text.RegularExpressions;

namespace ObjectiveIron.Core.Data;

/// <summary>
/// A extremely lightweight lexer for Paradox Clausewitz script files, 
/// enough to extract keys and block structures for data discovery.
/// </summary>
public static class ClausewitzLexer
{
    private static readonly Regex TokenRegex = new(@"\b[a-zA-Z0-9_.-]+\b|[{}]|[=]", RegexOptions.Compiled);

    public static IEnumerable<string> Tokenize(string content)
    {
        // Strip comments first
        var lines = content.Split('\n');
        var stripped = new System.Text.StringBuilder();
        foreach (var line in lines)
        {
            var commentIndex = line.IndexOf('#');
            if (commentIndex >= 0)
                stripped.AppendLine(line.Substring(0, commentIndex));
            else
                stripped.AppendLine(line);
        }

        foreach (Match match in TokenRegex.Matches(stripped.ToString()))
        {
            yield return match.Value;
        }
    }
}
