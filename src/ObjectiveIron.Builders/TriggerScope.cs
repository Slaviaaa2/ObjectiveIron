using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders;

/// <summary>
/// Scope for defining Clausewitz trigger/condition blocks.
/// Triggers check conditions and return true/false. Used in Available, Bypass, etc.
/// </summary>
public sealed class TriggerScope
{
    private readonly List<BlockEntry> _entries = [];

    // ─── Common Triggers ──────────────────────────────────────────

    public TriggerScope HasWar(bool value = true)
        => AddBool("has_war", value);

    public TriggerScope Tag(string countryTag)
        => AddString("tag", countryTag);

    public TriggerScope IsSubjectOf(string countryTag)
        => AddString("is_subject_of", countryTag);

    public TriggerScope HasPoliticalPower(Operator op, int amount)
        => AddComparison("has_political_power", op, amount);

    public TriggerScope HasStability(Operator op, double amount)
        => AddComparison("has_stability", op, amount);

    public TriggerScope HasWarSupport(Operator op, double amount)
        => AddComparison("has_war_support", op, amount);

    public TriggerScope HasManpower(Operator op, int amount)
        => AddComparison("has_manpower", op, amount);

    public TriggerScope IsInFaction(bool value = true)
        => AddBool("is_in_faction", value);

    public TriggerScope IsFactionLeader(bool value = true)
        => AddBool("is_faction_leader", value);

    public TriggerScope HasGovernment(string ideology)
        => AddString("has_government", ideology);

    public TriggerScope HasIdea(string idea)
        => AddString("has_idea", idea);

    public TriggerScope HasTech(string techId)
        => AddString("has_tech", techId);

    public TriggerScope HasCompletedFocus(string focusId)
        => AddString("has_completed_focus", focusId);

    public TriggerScope IsMajor(bool value = true)
        => AddBool("is_major", value);

    public TriggerScope Exists(bool value = true)
        => AddBool("exists", value);

    public TriggerScope HasCapitulated(bool value = true)
        => AddBool("has_capitulated", value);

    public TriggerScope NumOfFactories(Operator op, int amount)
        => AddComparison("num_of_factories", op, amount);

    public TriggerScope Date(Operator op, string date)
        => AddProperty("date", date, op);

    public TriggerScope OwnsState(int stateId)
        => AddInt("owns_state", stateId);

    public TriggerScope ControlsState(int stateId)
        => AddInt("controls_state", stateId);

    // ─── War & Military Triggers ─────────────────────────────────

    public TriggerScope HasWarWith(string countryTag)
        => AddString("has_war_with", countryTag);

    public TriggerScope HasWarTogetherWith(string countryTag)
        => AddString("has_war_together_with", countryTag);

    public TriggerScope IsInFactionWith(string countryTag)
        => AddString("is_in_faction_with", countryTag);

    public TriggerScope HasArmyManpower(Operator op, int amount)
        => AddComparison("has_army_manpower", op, amount);

    public TriggerScope HasArmySize(Operator op, int amount)
    {
        _entries.Add(new BlockEntry.NestedBlock("has_army_size", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("size", new PropertyValue.IntValue(amount), op)),
            new BlockEntry.PropertyEntry(new Property("type", new PropertyValue.StringValue("land")))
        })));
        return this;
    }

    public TriggerScope HasNavy(bool value = true)
        => AddBool("has_navy_size", value);

    public TriggerScope SurrenderProgress(Operator op, double amount)
        => AddComparison("surrender_progress", op, amount);

    public TriggerScope HasEquipment(string type, Operator op, int amount)
    {
        _entries.Add(new BlockEntry.NestedBlock("has_equipment", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property(type, new PropertyValue.IntValue(amount), op))
        })));
        return this;
    }

    public TriggerScope Strength(Operator op, double amount)
        => AddComparison("strength_ratio", op, amount);

    // ─── Political Triggers ──────────────────────────────────────

    public TriggerScope HasElectionsAllowed(bool value = true)
        => AddBool("has_elections", value);

    public TriggerScope HasPopularity(string ideology, Operator op, double amount)
    {
        _entries.Add(new BlockEntry.NestedBlock($"{ideology}", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("popularity", new PropertyValue.FloatValue(amount), op))
        })));
        return this;
    }

    public TriggerScope IsAlly(string countryTag)
        => AddString("is_ally_with", countryTag);

    public TriggerScope IsGuaranteeing(string countryTag)
        => AddString("is_guaranteeing", countryTag);

    public TriggerScope IsSubject(bool value = true)
        => AddBool("is_subject", value);

    public TriggerScope IsPuppet(bool value = true)
        => AddBool("is_puppet", value);

    public TriggerScope IsPuppetOf(string countryTag)
        => AddString("is_puppet_of", countryTag);

    // ─── Flag Triggers ───────────────────────────────────────────

    public TriggerScope HasCountryFlag(string flag)
        => AddString("has_country_flag", flag);

    public TriggerScope HasGlobalFlag(string flag)
        => AddString("has_global_flag", flag);

    public TriggerScope HasStateFlag(string flag)
        => AddString("has_state_flag", flag);

    // ─── Variable Triggers ───────────────────────────────────────

    public TriggerScope CheckVariable(string name, Operator op, int value)
    {
        _entries.Add(new BlockEntry.NestedBlock("check_variable", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("var", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.IntValue(value), op))
        })));
        return this;
    }

    public TriggerScope CheckVariable(string name, Operator op, double value)
    {
        _entries.Add(new BlockEntry.NestedBlock("check_variable", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("var", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.FloatValue(value), op))
        })));
        return this;
    }

    // ─── DLC & Cosmetic Triggers ─────────────────────────────────

    public TriggerScope HasDlc(string dlcName)
        => AddString("has_dlc", dlcName);

    public TriggerScope HasCosmeticTag(string tag)
        => AddString("has_cosmetic_tag", tag);

    public TriggerScope IsHistoricalFocusOn(bool value = true)
        => AddBool("is_historical_focus_on", value);

    // ─── Resource / Economy Triggers ─────────────────────────────

    public TriggerScope NumOfCivilianFactories(Operator op, int amount)
        => AddComparison("num_of_civilian_factories", op, amount);

    public TriggerScope NumOfMilitaryFactories(Operator op, int amount)
        => AddComparison("num_of_military_factories", op, amount);

    public TriggerScope NumOfNavalFactories(Operator op, int amount)
        => AddComparison("num_of_naval_factories", op, amount);

    public TriggerScope NumOfAvailableCivilianFactories(Operator op, int amount)
        => AddComparison("num_of_available_civilian_factories", op, amount);

    public TriggerScope HasFuelRatio(Operator op, double amount)
        => AddComparison("fuel_ratio", op, amount);

    public TriggerScope NumOfControlledStates(Operator op, int amount)
        => AddComparison("num_of_controlled_states", op, amount);

    // ─── State Triggers ──────────────────────────────────────────

    public TriggerScope IsControlledBy(string tag)
        => AddString("is_controlled_by", tag);

    public TriggerScope IsOwnedBy(string tag)
        => AddString("is_owned_by", tag);

    public TriggerScope IsCoreOf(string tag)
        => AddString("is_core_of", tag);

    public TriggerScope IsClaimedBy(string tag)
        => AddString("is_claimed_by", tag);

    public TriggerScope IsCoastal(bool value = true)
        => AddBool("is_coastal", value);

    public TriggerScope IsFullyControlledBy(string tag)
        => AddString("is_fully_controlled_by", tag);

    public TriggerScope HasStateCategory(string category)
        => AddString("has_state_category", category);

    // ─── Character / Leader Triggers ─────────────────────────────

    public TriggerScope HasCharacter(string characterId)
        => AddString("has_character", characterId);

    public TriggerScope HasUnitLeaderTrait(string trait)
        => AddString("has_unit_leader_trait", trait);

    public TriggerScope HasCountryLeaderTrait(string trait)
        => AddString("has_country_leader_trait", trait);

    public TriggerScope HasTraitSkill(Operator op, int amount)
        => AddComparison("skill", op, amount);

    // ─── Focus / Decision Triggers ───────────────────────────────

    public TriggerScope FocusProgress(string focusId, Operator op, double progress)
    {
        _entries.Add(new BlockEntry.NestedBlock("focus_progress", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("focus", new PropertyValue.StringValue(focusId))),
            new BlockEntry.PropertyEntry(new Property("progress", new PropertyValue.FloatValue(progress), op))
        })));
        return this;
    }

    public TriggerScope HasDecision(string decisionId)
        => AddString("has_decision", decisionId);

    public TriggerScope IsDecisionAvailable(string decisionId)
        => AddString("is_decision_available", decisionId);

    // ─── Logical Operators ────────────────────────────────────────

    public TriggerScope Not(Action<TriggerScope> configure)
        => AddNestedTrigger("NOT", configure);

    public TriggerScope Or(Action<TriggerScope> configure)
        => AddNestedTrigger("OR", configure);

    public TriggerScope And(Action<TriggerScope> configure)
        => AddNestedTrigger("AND", configure);

    public TriggerScope If(Action<TriggerScope> configure)
        => AddNestedTrigger("if", configure);

    // ─── Scope Changes ────────────────────────────────────────────

    public TriggerScope Country(string tag, Action<TriggerScope> configure)
    {
        var inner = new TriggerScope();
        configure(inner);
        _entries.Add(new BlockEntry.NestedBlock(tag, inner.Build()));
        return this;
    }

    public TriggerScope AnyCountry(Action<TriggerScope> configure)
        => AddNestedTrigger("any_country", configure);

    public TriggerScope AnyNeighborCountry(Action<TriggerScope> configure)
        => AddNestedTrigger("any_neighbor_country", configure);

    public TriggerScope AnyOwnedState(Action<TriggerScope> configure)
        => AddNestedTrigger("any_owned_state", configure);

    public TriggerScope AnyControlledState(Action<TriggerScope> configure)
        => AddNestedTrigger("any_controlled_state", configure);

    public TriggerScope AnyEnemyCountry(Action<TriggerScope> configure)
        => AddNestedTrigger("any_enemy_country", configure);

    public TriggerScope AnyAllyCountry(Action<TriggerScope> configure)
        => AddNestedTrigger("any_allied_country", configure);

    public TriggerScope AnyState(Action<TriggerScope> configure)
        => AddNestedTrigger("any_state", configure);

    public TriggerScope AllOwnedState(Action<TriggerScope> configure)
        => AddNestedTrigger("all_owned_state", configure);

    public TriggerScope AllControlledState(Action<TriggerScope> configure)
        => AddNestedTrigger("all_controlled_state", configure);

    public TriggerScope AllEnemyCountry(Action<TriggerScope> configure)
        => AddNestedTrigger("all_enemy_country", configure);

    public TriggerScope AllAllyCountry(Action<TriggerScope> configure)
        => AddNestedTrigger("all_allied_country", configure);

    public TriggerScope AnyUnitLeader(Action<TriggerScope> configure)
        => AddNestedTrigger("any_unit_leader", configure);

    // ─── Scripted Triggers ────────────────────────────────────────

    public TriggerScope ScriptedTrigger(string name)
        => AddBool(name, true);

    public TriggerScope ScriptedTrigger(string name, Action<TriggerScope> configure)
        => AddNestedTrigger(name, configure);

    // ─── Escape Hatch ─────────────────────────────────────────────

    public TriggerScope Custom(string key, string value)
        => AddString(key, value);

    public TriggerScope Custom(string key, int value)
        => AddInt(key, value);

    public TriggerScope Custom(string key, double value)
        => AddFloat(key, value);

    public TriggerScope Custom(string key, bool value)
        => AddBool(key, value);

    public TriggerScope CustomBlock(string key, Action<TriggerScope> configure)
        => AddNestedTrigger(key, configure);

    // ─── Internal Build ───────────────────────────────────────────

    internal Block Build() => new(_entries);

    private TriggerScope AddString(string key, string value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.StringValue(value))));
        return this;
    }

    private TriggerScope AddInt(string key, int value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.IntValue(value))));
        return this;
    }

    private TriggerScope AddBool(string key, bool value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.BoolValue(value))));
        return this;
    }

    private TriggerScope AddFloat(string key, double value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.FloatValue(value))));
        return this;
    }

    private TriggerScope AddProperty(string key, string value, Operator op = Operator.Equals)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.StringValue(value), op)));
        return this;
    }

    private TriggerScope AddComparison(string key, Operator op, int amount)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.IntValue(amount), op)));
        return this;
    }

    private TriggerScope AddComparison(string key, Operator op, double amount)
    {
        _entries.Add(new BlockEntry.PropertyEntry(
            new Property(key, new PropertyValue.FloatValue(amount), op)));
        return this;
    }

    private TriggerScope AddNestedTrigger(string name, Action<TriggerScope> configure)
    {
        var inner = new TriggerScope();
        configure(inner);
        _entries.Add(new BlockEntry.NestedBlock(name, inner.Build()));
        return this;
    }
}
