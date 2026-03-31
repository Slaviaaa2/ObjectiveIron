using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders;

/// <summary>
/// Scope for defining Clausewitz modifiers (e.g., in ideas, advisors, traits).
/// Modifiers change values like stability, war support, industry, etc.
/// </summary>
public sealed class ModifierScope
{
    private readonly List<BlockEntry> _entries = [];

    // ─── Economic Modifiers ───────────────────────────────────────
    
    public ModifierScope ProductionSpeedArmsFactoryFactor(double amount)
        => AddFloat("production_speed_arms_factory_factor", amount);

    public ModifierScope ProductionSpeedIndustrialComplexFactor(double amount)
        => AddFloat("production_speed_industrial_complex_factor", amount);

    public ModifierScope IndustrialCapacityFactory(double amount)
        => AddFloat("industrial_capacity_factory", amount);

    public ModifierScope ConsumerGoodsFactor(double amount)
        => AddFloat("consumer_goods_factor", amount);

    public ModifierScope ConversionCostFactor(double amount)
        => AddFloat("conversion_cost_factor", amount);

    public ModifierScope ProductionSpeedCivilianFactor(double amount)
        => AddFloat("production_speed_industrial_complex_factor", amount); // Alias

    public ModifierScope ConstructionSpeedFactor(double amount)
        => AddFloat("construction_speed_factor", amount);

    // ─── Political Modifiers ──────────────────────────────────────
    
    public ModifierScope StabilityFactor(double amount)
        => AddFloat("stability_factor", amount);

    public ModifierScope WarSupportFactor(double amount)
        => AddFloat("war_support_factor", amount);

    public ModifierScope PoliticalPowerFactor(double amount)
        => AddFloat("political_power_factor", amount);

    public ModifierScope PoliticalPowerGain(double amount)
        => AddFloat("political_power_gain", amount);

    public ModifierScope CommandPowerGain(double amount)
        => AddFloat("command_power_gain", amount);

    public ModifierScope IdeologyDrift(string ideology, double amount)
        => AddFloat($"{ideology}_drift_factor", amount);

    // ─── Military Modifiers ───────────────────────────────────────

    public ModifierScope ResearchSpeedFactor(double amount)
        => AddFloat("research_speed_factor", amount);

    public ModifierScope MaxPlanning(double amount)
        => AddFloat("max_planning", amount);

    public ModifierScope PlanningSpeed(double amount)
        => AddFloat("planning_speed", amount);

    public ModifierScope ExperienceGainArmy(double amount)
        => AddFloat("experience_gain_army", amount);

    public ModifierScope ExperienceGainNavy(double amount)
        => AddFloat("experience_gain_navy", amount);

    public ModifierScope ExperienceGainAir(double amount)
        => AddFloat("experience_gain_air", amount);

    public ModifierScope LogisticCapacity(double amount)
        => AddFloat("logistic_capacity", amount);

    public ModifierScope SupplyConsumptionFactor(double amount)
        => AddFloat("supply_consumption_factor", amount);

    public ModifierScope TrainingTimeFactor(double amount)
        => AddFloat("training_time_factor", amount);

    // ─── Army Modifiers ───────────────────────────────────────────

    public ModifierScope ArmyAttackFactor(double amount)
        => AddFloat("army_attack_factor", amount);

    public ModifierScope ArmyDefenceFactor(double amount)
        => AddFloat("army_defence_factor", amount);

    public ModifierScope ArmyOrgFactor(double amount)
        => AddFloat("army_org_factor", amount);

    public ModifierScope ArmyOrg(double amount)
        => AddFloat("army_org", amount);

    public ModifierScope ArmyMorale(double amount)
        => AddFloat("army_morale_factor", amount);

    public ModifierScope ArmySpeedFactor(double amount)
        => AddFloat("army_speed_factor", amount);

    public ModifierScope DivisionRecoveryRate(double amount)
        => AddFloat("army_org_regain", amount);

    public ModifierScope LandAttritionFactor(double amount)
        => AddFloat("attrition", amount);

    public ModifierScope LandReinholdRate(double amount)
        => AddFloat("land_reinforce_rate", amount);

    public ModifierScope OffensiveWarStabilityFactor(double amount)
        => AddFloat("offensive_war_stability_factor", amount);

    public ModifierScope DefensiveWarStabilityFactor(double amount)
        => AddFloat("defensive_war_stability_factor", amount);

    public ModifierScope MaxCommandPower(double amount)
        => AddFloat("max_command_power", amount);

    public ModifierScope MaxCommandPowerMult(double amount)
        => AddFloat("max_command_power_mult", amount);

    // ─── Navy Modifiers ───────────────────────────────────────────

    public ModifierScope NavalSpeedFactor(double amount)
        => AddFloat("naval_speed_factor", amount);

    public ModifierScope NavalRangeModifier(double amount)
        => AddFloat("naval_range_factor", amount);

    public ModifierScope NavalAttack(double amount)
        => AddFloat("navy_attack_factor", amount);

    public ModifierScope NavalDefence(double amount)
        => AddFloat("navy_defence_factor", amount);

    public ModifierScope ConvoyRaidingEfficiency(double amount)
        => AddFloat("convoy_raiding_efficiency_factor", amount);

    public ModifierScope NavalDetectionFactor(double amount)
        => AddFloat("naval_detection", amount);

    public ModifierScope SubmarineDetection(double amount)
        => AddFloat("submarine_detection", amount);

    public ModifierScope ShipBuildSpeedFactor(double amount)
        => AddFloat("industrial_capacity_dockyard", amount);

    // ─── Air Modifiers ────────────────────────────────────────────

    public ModifierScope AirAttackFactor(double amount)
        => AddFloat("air_attack_factor", amount);

    public ModifierScope AirDefenceFactor(double amount)
        => AddFloat("air_defence_factor", amount);

    public ModifierScope AirAgilityFactor(double amount)
        => AddFloat("air_agility_factor", amount);

    public ModifierScope AirAccidentChance(double amount)
        => AddFloat("air_accidents_factor", amount);

    public ModifierScope AirRangeModifier(double amount)
        => AddFloat("air_range_factor", amount);

    public ModifierScope CasRatio(double amount)
        => AddFloat("cas_damage_factor", amount);

    public ModifierScope StrategicBombingFactor(double amount)
        => AddFloat("strategic_bombing_factor", amount);

    public ModifierScope AirSuperiority(double amount)
        => AddFloat("air_superiority_factor", amount);

    // ─── Production Modifiers ─────────────────────────────────────

    public ModifierScope ProductionEfficiency(double amount)
        => AddFloat("production_factory_efficiency_gain_factor", amount);

    public ModifierScope ProductionEfficiencyCap(double amount)
        => AddFloat("production_factory_max_efficiency_factor", amount);

    public ModifierScope LocalResourcesFactor(double amount)
        => AddFloat("local_resources_factor", amount);

    public ModifierScope GlobalResourcesFactor(double amount)
        => AddFloat("global_resource_to_market_factor", amount);

    public ModifierScope MinExportFactor(double amount)
        => AddFloat("min_export", amount);

    public ModifierScope ProductionOilFactor(double amount)
        => AddFloat("production_oil_factor", amount);

    // ─── Construction Modifiers ───────────────────────────────────

    public ModifierScope ProductionSpeedBuildingsFactor(string buildingType, double amount)
        => AddFloat($"production_speed_{buildingType}_factor", amount);

    public ModifierScope InfrastructureConstructionSpeed(double amount)
        => AddFloat("production_speed_infrastructure_factor", amount);

    // ─── Manpower Modifiers ───────────────────────────────────────

    public ModifierScope MonthlyPopulation(double amount)
        => AddFloat("monthly_population", amount);

    public ModifierScope ConscriptionFactor(double amount)
        => AddFloat("conscription_factor", amount);

    public ModifierScope Conscription(double amount)
        => AddFloat("conscription", amount);

    public ModifierScope NonCoreManhour(double amount)
        => AddFloat("non_core_manpower", amount);

    // ─── Diplomacy Modifiers ──────────────────────────────────────

    public ModifierScope JustifyWarGoalTime(double amount)
        => AddFloat("justify_war_goal_time", amount);

    public ModifierScope ImprovingRelationsModifier(double amount)
        => AddFloat("improve_relations_maintain_cost_factor", amount);

    public ModifierScope OpinionGainMonthly(double amount)
        => AddFloat("opinion_gain_monthly_factor", amount);

    public ModifierScope TradeOpinionFactor(double amount)
        => AddFloat("trade_opinion_factor", amount);

    public ModifierScope GenerateWargoalTensionFactor(double amount)
        => AddFloat("generate_wargoal_tension", amount);

    // ─── Intelligence Modifiers ───────────────────────────────────

    public ModifierScope IntelNetworkGain(double amount)
        => AddFloat("intel_network_gain", amount);

    public ModifierScope IntelNetworkGainFactor(double amount)
        => AddFloat("intel_network_gain_factor", amount);

    public ModifierScope DecryptionFactor(double amount)
        => AddFloat("decryption", amount);

    public ModifierScope EncryptionFactor(double amount)
        => AddFloat("encryption", amount);

    public ModifierScope OperativeSlots(int amount)
        => AddInt("operative_slots", amount);

    // ─── Misc Modifiers ───────────────────────────────────────────

    public ModifierScope DailyPoliticalPower(double amount)
        => AddFloat("political_power_gain", amount);

    public ModifierScope MaxOrganisation(double amount)
        => AddFloat("max_org", amount);

    public ModifierScope CombatWidth(double amount)
        => AddFloat("combat_width_factor", amount);

    public ModifierScope SupplyReach(double amount)
        => AddFloat("supply_node_range", amount);

    public ModifierScope Stability(double amount)
        => AddFloat("stability_weekly", amount);

    public ModifierScope WarSupport(double amount)
        => AddFloat("war_support_weekly", amount);

    public ModifierScope LandNightAttack(double amount)
        => AddFloat("land_night_attack", amount);

    public ModifierScope FortLevel(double amount)
        => AddFloat("max_fort_level", amount);

    // ─── Escape Hatch ─────────────────────────────────────────────

    public ModifierScope Custom(string key, double value) => AddFloat(key, value);
    public ModifierScope Custom(string key, int value) => AddInt(key, value);
    public ModifierScope Custom(string key, string value) => AddString(key, value);

    // ─── Internal Build ───────────────────────────────────────────

    internal Block Build() => new(_entries);

    private ModifierScope AddString(string key, string value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.StringValue(value))));
        return this;
    }

    private ModifierScope AddInt(string key, int value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.IntValue(value))));
        return this;
    }

    private ModifierScope AddFloat(string key, double value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.FloatValue(value))));
        return this;
    }
}
