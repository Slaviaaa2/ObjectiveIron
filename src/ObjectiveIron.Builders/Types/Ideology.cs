namespace ObjectiveIron.Builders.Types;

/// <summary>
/// Ideology types used in HOI4 political system.
/// </summary>
public enum Ideology
{
    Democracy,
    Fascism,
    Communism,
    Neutrality
}

public static class IdeologyExtensions
{
    public static string ToClausewitz(this Ideology ideology) => ideology switch
    {
        Ideology.Democracy => "democratic",
        Ideology.Fascism => "fascism",
        Ideology.Communism => "communism",
        Ideology.Neutrality => "neutrality",
        _ => ideology.ToString().ToLowerInvariant()
    };
}
