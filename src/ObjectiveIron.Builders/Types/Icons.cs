namespace ObjectiveIron.Builders.Types;

/// <summary>
/// Pre-defined GFX sprite references for common focus icons.
/// Users can extend by creating new GfxSprite instances.
/// </summary>
public static class Icons
{
    // ─── Generic Goals ────────────────────────────────────────────
    public static readonly GfxSprite GenericProduction = new("GFX_goal_generic_production");
    public static readonly GfxSprite GenericProduction2 = new("GFX_goal_generic_production2");
    public static readonly GfxSprite GenericConsumerGoods = new("GFX_goal_generic_consumer_goods");
    public static readonly GfxSprite GenericConstruction = new("GFX_goal_generic_construction");
    public static readonly GfxSprite GenericConstruction2 = new("GFX_goal_generic_construction2");
    public static readonly GfxSprite GenericTrade = new("GFX_goal_generic_positive_trade_relations");

    // ─── Military ─────────────────────────────────────────────────
    public static readonly GfxSprite GenericArmy = new("GFX_goal_generic_allies_702");
    public static readonly GfxSprite GenericArmyDoctrines = new("GFX_goal_generic_army_doctrines");
    public static readonly GfxSprite GenericSmallArms = new("GFX_goal_generic_small_arms");
    public static readonly GfxSprite GenericMilitaryDealership = new("GFX_goal_generic_military_sphere");
    public static readonly GfxSprite GenericAirForce = new("GFX_goal_generic_build_airforce");
    public static readonly GfxSprite GenericNavy = new("GFX_goal_generic_navy_doctrines");
    public static readonly GfxSprite GenericMajorWar = new("GFX_goal_generic_major_war");

    // ─── Political ────────────────────────────────────────────────
    public static readonly GfxSprite GenericDemandTerritory = new("GFX_goal_generic_demand_territory");
    public static readonly GfxSprite GenericFortifyNation = new("GFX_goal_generic_fortify_nation");
    public static readonly GfxSprite GenericPoliticalPressure = new("GFX_goal_generic_political_pressure");
    public static readonly GfxSprite GenericDangerous = new("GFX_goal_generic_dangerous_deal");
    public static readonly GfxSprite GenericAlliances = new("GFX_goal_generic_alliance");

    // ─── Focus Specific ───────────────────────────────────────────
    public static readonly GfxSprite Research = new("GFX_focus_research");
    public static readonly GfxSprite Research2 = new("GFX_focus_research2");
    public static readonly GfxSprite StrikeAtDemocracy = new("GFX_focus_generic_strike_at_democracy1");
    public static readonly GfxSprite SupportDemocracy = new("GFX_focus_generic_support_the_left_right");
    public static readonly GfxSprite FascismRise = new("GFX_focus_generic_fascism_grows");
    public static readonly GfxSprite CommunismRise = new("GFX_focus_generic_soviet_politics");
    public static readonly GfxSprite SelfManagement = new("GFX_focus_generic_self_management");
    public static readonly GfxSprite ForeignPolicy = new("GFX_focus_generic_diplomatic_treaty");
    public static readonly GfxSprite Tankette = new("GFX_goal_generic_army_tanks");
    public static readonly GfxSprite CivilianIndustry = new("GFX_goal_generic_construct_civ_factory");
    public static readonly GfxSprite MilitaryIndustry = new("GFX_goal_generic_construct_mil_factory");
    public static readonly GfxSprite Radar = new("GFX_goal_generic_radar");
    public static readonly GfxSprite Intelligence = new("GFX_goal_generic_intelligence_agency");

    // ─── Custom ───────────────────────────────────────────────────
    /// <summary>Create a custom GFX sprite reference.</summary>
    public static GfxSprite Custom(string gfxName) => new(gfxName);
}
