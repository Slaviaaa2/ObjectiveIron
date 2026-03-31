namespace ObjectiveIron.Builders.Types;

/// <summary>
/// Pre-defined country tags for major and common nations.
/// Users can extend by creating new CountryTag instances.
/// </summary>
public static class Tags
{
    // ─── Major Powers ─────────────────────────────────────────────
    public static readonly CountryTag GER = new("GER");
    public static readonly CountryTag SOV = new("SOV");
    public static readonly CountryTag ENG = new("ENG");
    public static readonly CountryTag FRA = new("FRA");
    public static readonly CountryTag USA = new("USA");
    public static readonly CountryTag JAP = new("JAP");
    public static readonly CountryTag ITA = new("ITA");

    // ─── European Minors ──────────────────────────────────────────
    public static readonly CountryTag POL = new("POL");
    public static readonly CountryTag CZE = new("CZE");
    public static readonly CountryTag ROM = new("ROM");
    public static readonly CountryTag HUN = new("HUN");
    public static readonly CountryTag YUG = new("YUG");
    public static readonly CountryTag BUL = new("BUL");
    public static readonly CountryTag GRE = new("GRE");
    public static readonly CountryTag TUR = new("TUR");
    public static readonly CountryTag SPA = new("SPA");
    public static readonly CountryTag SPR = new("SPR");
    public static readonly CountryTag POR = new("POR");
    public static readonly CountryTag HOL = new("HOL");
    public static readonly CountryTag BEL = new("BEL");
    public static readonly CountryTag SWE = new("SWE");
    public static readonly CountryTag NOR = new("NOR");
    public static readonly CountryTag DEN = new("DEN");
    public static readonly CountryTag FIN = new("FIN");
    public static readonly CountryTag SWI = new("SWI");
    public static readonly CountryTag AUS = new("AUS");
    public static readonly CountryTag IRE = new("IRE");

    // ─── Asia & Pacific ───────────────────────────────────────────
    public static readonly CountryTag CHI = new("CHI");
    public static readonly CountryTag PRC = new("PRC");
    public static readonly CountryTag MAN = new("MAN");
    public static readonly CountryTag SIA = new("SIA");
    public static readonly CountryTag RAJ = new("RAJ");
    public static readonly CountryTag AST = new("AST");
    public static readonly CountryTag NZL = new("NZL");
    public static readonly CountryTag PHI = new("PHI");

    // ─── Americas ─────────────────────────────────────────────────
    public static readonly CountryTag CAN = new("CAN");
    public static readonly CountryTag MEX = new("MEX");
    public static readonly CountryTag BRA = new("BRA");
    public static readonly CountryTag ARG = new("ARG");

    // ─── Africa & Middle East ─────────────────────────────────────
    public static readonly CountryTag SAF = new("SAF");
    public static readonly CountryTag ETH = new("ETH");
    public static readonly CountryTag EGY = new("EGY");
    public static readonly CountryTag IRQ = new("IRQ");
    public static readonly CountryTag PER = new("PER");
}
