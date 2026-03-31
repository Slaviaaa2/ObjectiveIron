using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders;

/// <summary>
/// Scope for defining Clausewitz effect blocks.
/// Effects perform actions (add resources, fire events, etc.).
/// Used in CompletionReward, SelectEffect, CancelEffect overrides.
/// </summary>
public sealed class EffectScope
{
    private readonly List<BlockEntry> _entries = [];

    // ─── Political Effects ────────────────────────────────────────

    public EffectScope AddPoliticalPower(int amount)
        => AddInt("add_political_power", amount);

    public EffectScope AddStability(double amount)
        => AddFloat("add_stability", amount);

    public EffectScope AddWarSupport(double amount)
        => AddFloat("add_war_support", amount);

    public EffectScope SetPolitics(Action<EffectScope> configure)
        => AddNestedEffect("set_politics", configure);

    public EffectScope AddPopularity(Ideology ideology, double amount)
    {
        _entries.Add(new BlockEntry.NestedBlock("add_popularity", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("ideology", new PropertyValue.StringValue(ideology.ToClausewitz()))),
            new BlockEntry.PropertyEntry(new Property("popularity", new PropertyValue.FloatValue(amount)))
        })));
        return this;
    }

    // ─── Military Effects ─────────────────────────────────────────

    public EffectScope AddManpower(int amount)
        => AddInt("add_manpower", amount);

    public EffectScope AddEquipmentToStockpile(string type, int amount)
    {
        _entries.Add(new BlockEntry.NestedBlock("add_equipment_to_stockpile", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("type", new PropertyValue.StringValue(type))),
            new BlockEntry.PropertyEntry(new Property("amount", new PropertyValue.IntValue(amount)))
        })));
        return this;
    }

    public EffectScope CreateWargoal(CountryTag target, string type)
    {
        _entries.Add(new BlockEntry.NestedBlock("create_wargoal", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("target", new PropertyValue.StringValue(target.Value))),
            new BlockEntry.PropertyEntry(new Property("type", new PropertyValue.StringValue(type)))
        })));
        return this;
    }

    public EffectScope DeclareWarOn(CountryTag target, string type)
    {
        _entries.Add(new BlockEntry.NestedBlock("declare_war_on", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("target", new PropertyValue.StringValue(target.Value))),
            new BlockEntry.PropertyEntry(new Property("type", new PropertyValue.StringValue(type)))
        })));
        return this;
    }

    // ─── Economy Effects ──────────────────────────────────────────

    public EffectScope AddBuildingConstruction(BuildingType type, int level, bool instantBuild = false)
    {
        var entries = new List<BlockEntry>
        {
            new BlockEntry.PropertyEntry(new Property("type", new PropertyValue.StringValue(type.ToClausewitz()))),
            new BlockEntry.PropertyEntry(new Property("level", new PropertyValue.IntValue(level))),
            new BlockEntry.PropertyEntry(new Property("instant_build", new PropertyValue.BoolValue(instantBuild)))
        };
        _entries.Add(new BlockEntry.NestedBlock("add_building_construction", new Block(entries)));
        return this;
    }

    public EffectScope AddResearchSlot(int amount)
        => AddInt("add_research_slot", amount);

    public EffectScope SetTechnology(string techId, int researched = 1)
    {
        _entries.Add(new BlockEntry.NestedBlock("set_technology", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property(techId, new PropertyValue.IntValue(researched)))
        })));
        return this;
    }

    public EffectScope AddTechBonus(string name, int bonusPercent, int uses, string? category = null)
    {
        var entries = new List<BlockEntry>
        {
            new BlockEntry.PropertyEntry(new Property("name", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("bonus", new PropertyValue.FloatValue(bonusPercent / 100.0))),
            new BlockEntry.PropertyEntry(new Property("uses", new PropertyValue.IntValue(uses)))
        };
        if (category != null)
            entries.Add(new BlockEntry.PropertyEntry(new Property("category", new PropertyValue.StringValue(category))));
        _entries.Add(new BlockEntry.NestedBlock("add_tech_bonus", new Block(entries)));
        return this;
    }

    // ─── Diplomatic Effects ───────────────────────────────────────

    public EffectScope AddOpinionModifier(CountryTag target, string modifier)
    {
        _entries.Add(new BlockEntry.NestedBlock("add_opinion_modifier", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("target", new PropertyValue.StringValue(target.Value))),
            new BlockEntry.PropertyEntry(new Property("modifier", new PropertyValue.StringValue(modifier)))
        })));
        return this;
    }

    public EffectScope GiveMilitaryAccess(CountryTag target)
        => AddString("give_military_access", target.Value);

    public EffectScope Annex(CountryTag target)
        => AddString("annex_country", target.Value);

    public EffectScope Puppet(CountryTag target)
        => AddString("puppet", target.Value);

    // ─── Events ───────────────────────────────────────────────────

    public EffectScope CountryEvent(string eventId, int? days = null, int? hours = null)
    {
        var entries = new List<BlockEntry>
        {
            new BlockEntry.PropertyEntry(new Property("id", new PropertyValue.StringValue(eventId)))
        };
        if (days.HasValue)
            entries.Add(new BlockEntry.PropertyEntry(new Property("days", new PropertyValue.IntValue(days.Value))));
        if (hours.HasValue)
            entries.Add(new BlockEntry.PropertyEntry(new Property("hours", new PropertyValue.IntValue(hours.Value))));
        _entries.Add(new BlockEntry.NestedBlock("country_event", new Block(entries)));
        return this;
    }

    public EffectScope NewsEvent(string eventId, int? days = null, int? hours = null)
    {
        var entries = new List<BlockEntry>
        {
            new BlockEntry.PropertyEntry(new Property("id", new PropertyValue.StringValue(eventId)))
        };
        if (days.HasValue)
            entries.Add(new BlockEntry.PropertyEntry(new Property("days", new PropertyValue.IntValue(days.Value))));
        if (hours.HasValue)
            entries.Add(new BlockEntry.PropertyEntry(new Property("hours", new PropertyValue.IntValue(hours.Value))));
        _entries.Add(new BlockEntry.NestedBlock("news_event", new Block(entries)));
        return this;
    }

    // ─── Character Effects ────────────────────────────────────────

    public EffectScope RecruitCharacter(string characterId)
        => AddString("recruit_character", characterId);

    public EffectScope RetireCharacter(string characterId)
        => AddString("retire_character", characterId);

    public EffectScope PromoteCharacter(string characterId, string role)
    {
        _entries.Add(new BlockEntry.NestedBlock("promote_character", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("character", new PropertyValue.StringValue(characterId))),
            new BlockEntry.PropertyEntry(new Property("role", new PropertyValue.StringValue(role)))
        })));
        return this;
    }

    // ─── Ideas & National Spirits ─────────────────────────────────

    public EffectScope AddIdeas(params string[] ideas)
    {
        foreach (var idea in ideas)
            AddString("add_ideas", idea);
        return this;
    }

    public EffectScope RemoveIdeas(params string[] ideas)
    {
        foreach (var idea in ideas)
            AddString("remove_ideas", idea);
        return this;
    }

    public EffectScope SwapIdeas(string removeIdea, string addIdea)
    {
        _entries.Add(new BlockEntry.NestedBlock("swap_ideas", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("remove_idea", new PropertyValue.StringValue(removeIdea))),
            new BlockEntry.PropertyEntry(new Property("add_idea", new PropertyValue.StringValue(addIdea)))
        })));
        return this;
    }

    // ─── State Effects ────────────────────────────────────────────

    public EffectScope TransferState(int stateId)
        => AddInt("transfer_state", stateId);

    public EffectScope SetStateOwner(int stateId)
        => AddInt("set_state_owner", stateId);

    public EffectScope SetStateController(int stateId)
        => AddInt("set_state_controller", stateId);

    public EffectScope AddStateClaim(int stateId)
        => AddInt("add_state_claim", stateId);

    public EffectScope RemoveStateClaim(int stateId)
        => AddInt("remove_state_claim", stateId);

    public EffectScope AddStateCore(int stateId)
        => AddInt("add_state_core", stateId);

    public EffectScope RemoveStateCore(int stateId)
        => AddInt("remove_state_core", stateId);

    public EffectScope SetCapital(int stateId)
        => AddInt("set_capital", stateId);

    // ─── Variable Effects ─────────────────────────────────────────

    public EffectScope SetVariable(string name, int value)
    {
        _entries.Add(new BlockEntry.NestedBlock("set_variable", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("var", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.IntValue(value)))
        })));
        return this;
    }

    public EffectScope SetVariable(string name, double value)
    {
        _entries.Add(new BlockEntry.NestedBlock("set_variable", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("var", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.FloatValue(value)))
        })));
        return this;
    }

    public EffectScope AddToVariable(string name, int value)
    {
        _entries.Add(new BlockEntry.NestedBlock("add_to_variable", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("var", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.IntValue(value)))
        })));
        return this;
    }

    public EffectScope SubtractFromVariable(string name, int value)
    {
        _entries.Add(new BlockEntry.NestedBlock("subtract_from_variable", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("var", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.IntValue(value)))
        })));
        return this;
    }

    public EffectScope MultiplyVariable(string name, double value)
    {
        _entries.Add(new BlockEntry.NestedBlock("multiply_variable", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("var", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.FloatValue(value)))
        })));
        return this;
    }

    public EffectScope DivideVariable(string name, double value)
    {
        _entries.Add(new BlockEntry.NestedBlock("divide_variable", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("var", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.FloatValue(value)))
        })));
        return this;
    }

    public EffectScope ClampVariable(string name, double min, double max)
    {
        _entries.Add(new BlockEntry.NestedBlock("clamp_variable", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("var", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("min", new PropertyValue.FloatValue(min))),
            new BlockEntry.PropertyEntry(new Property("max", new PropertyValue.FloatValue(max)))
        })));
        return this;
    }

    // ─── Flag Effects ─────────────────────────────────────────────

    public EffectScope SetCountryFlag(string flag)
        => AddString("set_country_flag", flag);

    public EffectScope SetCountryFlag(string flag, int days)
    {
        _entries.Add(new BlockEntry.NestedBlock("set_country_flag", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("flag", new PropertyValue.StringValue(flag))),
            new BlockEntry.PropertyEntry(new Property("days", new PropertyValue.IntValue(days)))
        })));
        return this;
    }

    public EffectScope ClearCountryFlag(string flag)
        => AddString("clr_country_flag", flag);

    public EffectScope SetGlobalFlag(string flag)
        => AddString("set_global_flag", flag);

    public EffectScope ClearGlobalFlag(string flag)
        => AddString("clr_global_flag", flag);

    public EffectScope SetStateFlag(string flag)
        => AddString("set_state_flag", flag);

    public EffectScope ClearStateFlag(string flag)
        => AddString("clr_state_flag", flag);

    // ─── Cosmetic / Name Effects ──────────────────────────────────

    public EffectScope SetCosmeticTag(string tag)
        => AddString("set_cosmetic_tag", tag);

    public EffectScope DropCosmeticTag(bool value = true)
        => AddBool("drop_cosmetic_tag", value);

    public EffectScope SetPartyName(string ideology, string longName, string name)
    {
        _entries.Add(new BlockEntry.NestedBlock("set_party_name", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("ideology", new PropertyValue.StringValue(ideology))),
            new BlockEntry.PropertyEntry(new Property("long_name", new PropertyValue.StringValue(longName))),
            new BlockEntry.PropertyEntry(new Property("name", new PropertyValue.StringValue(name)))
        })));
        return this;
    }

    // ─── Autonomy / Faction Effects ───────────────────────────────

    public EffectScope SetAutonomy(CountryTag target, string autonomyState)
    {
        _entries.Add(new BlockEntry.NestedBlock("set_autonomy", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("target", new PropertyValue.StringValue(target.Value))),
            new BlockEntry.PropertyEntry(new Property("autonomy_state", new PropertyValue.StringValue(autonomyState)))
        })));
        return this;
    }

    public EffectScope CreateFaction(string name)
        => AddString("create_faction", name);

    public EffectScope AddToFaction(CountryTag target)
        => AddString("add_to_faction", target.Value);

    public EffectScope RemoveFromFaction(CountryTag target)
        => AddString("remove_from_faction", target.Value);

    public EffectScope DismantleFaction(bool value = true)
        => AddBool("dismantle_faction", value);

    public EffectScope LeaveAlliance(bool value = true)
        => AddBool("leave_faction", value);

    // ─── War / Peace Effects ──────────────────────────────────────

    public EffectScope WhitePeace(CountryTag target)
        => AddString("white_peace", target.Value);

    public EffectScope AddNamedThreat(string threat, int value)
    {
        _entries.Add(new BlockEntry.NestedBlock("add_named_threat", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("threat", new PropertyValue.StringValue(threat))),
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.IntValue(value)))
        })));
        return this;
    }

    public EffectScope AddThreat(int amount)
        => AddInt("add_threat", amount);

    // ─── Resource / Economy Effects ───────────────────────────────

    public EffectScope AddResource(string type, int amount, int? stateId = null)
    {
        var entries = new List<BlockEntry>
        {
            new BlockEntry.PropertyEntry(new Property("type", new PropertyValue.StringValue(type))),
            new BlockEntry.PropertyEntry(new Property("amount", new PropertyValue.IntValue(amount)))
        };
        if (stateId.HasValue)
            entries.Add(new BlockEntry.PropertyEntry(new Property("state", new PropertyValue.IntValue(stateId.Value))));
        _entries.Add(new BlockEntry.NestedBlock("add_resource", new Block(entries)));
        return this;
    }

    public EffectScope AddFuelRatio(double amount)
        => AddFloat("add_fuel", amount);

    public EffectScope SetFuelRatio(double amount)
        => AddFloat("set_fuel_ratio", amount);

    public EffectScope AddConvoys(int amount)
        => AddInt("add_convoys", amount);

    public EffectScope AddOffsite(int amount)
        => AddInt("add_offsite_building", amount);

    // ─── Research / Tech Effects ──────────────────────────────────

    public EffectScope AddDoctrineBonus(string name, int bonusPercent, int uses, string? category = null)
    {
        var entries = new List<BlockEntry>
        {
            new BlockEntry.PropertyEntry(new Property("name", new PropertyValue.StringValue(name))),
            new BlockEntry.PropertyEntry(new Property("bonus", new PropertyValue.FloatValue(bonusPercent / 100.0))),
            new BlockEntry.PropertyEntry(new Property("uses", new PropertyValue.IntValue(uses)))
        };
        if (category != null)
            entries.Add(new BlockEntry.PropertyEntry(new Property("category", new PropertyValue.StringValue(category))));
        _entries.Add(new BlockEntry.NestedBlock("add_doctrine_cost_reduction", new Block(entries)));
        return this;
    }

    public EffectScope ModifyTechSharingBonus(string id, double bonus)
    {
        _entries.Add(new BlockEntry.NestedBlock("modify_tech_sharing_bonus", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("id", new PropertyValue.StringValue(id))),
            new BlockEntry.PropertyEntry(new Property("bonus", new PropertyValue.FloatValue(bonus)))
        })));
        return this;
    }

    // ─── Modifier Effects ─────────────────────────────────────────

    public EffectScope AddTimedIdeaWithModifier(string idea, int days)
    {
        _entries.Add(new BlockEntry.NestedBlock("add_timed_idea", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("idea", new PropertyValue.StringValue(idea))),
            new BlockEntry.PropertyEntry(new Property("days", new PropertyValue.IntValue(days)))
        })));
        return this;
    }

    public EffectScope AddDynamicModifier(string modifier, int? days = null, string? scope = null)
    {
        var entries = new List<BlockEntry>
        {
            new BlockEntry.PropertyEntry(new Property("modifier", new PropertyValue.StringValue(modifier)))
        };
        if (days.HasValue)
            entries.Add(new BlockEntry.PropertyEntry(new Property("days", new PropertyValue.IntValue(days.Value))));
        if (scope != null)
            entries.Add(new BlockEntry.PropertyEntry(new Property("scope", new PropertyValue.StringValue(scope))));
        _entries.Add(new BlockEntry.NestedBlock("add_dynamic_modifier", new Block(entries)));
        return this;
    }

    public EffectScope RemoveDynamicModifier(string modifier)
    {
        _entries.Add(new BlockEntry.NestedBlock("remove_dynamic_modifier", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("modifier", new PropertyValue.StringValue(modifier)))
        })));
        return this;
    }

    // ─── Country Scope Effects ────────────────────────────────────

    public EffectScope SetRulingParty(string ideology)
        => AddString("set_ruling_party", ideology);

    public EffectScope SetPartyLeader(string ideology, string characterId)
    {
        _entries.Add(new BlockEntry.NestedBlock("set_party_leader", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("ideology", new PropertyValue.StringValue(ideology))),
            new BlockEntry.PropertyEntry(new Property("character", new PropertyValue.StringValue(characterId)))
        })));
        return this;
    }

    public EffectScope Release(CountryTag target)
        => AddString("release", target.Value);

    public EffectScope ReleaseAsSubject(CountryTag target)
        => AddString("release_puppet", target.Value);

    public EffectScope AddCommandPower(double amount)
        => AddFloat("add_command_power", amount);

    public EffectScope AddNamedCommand(int amount)
        => AddInt("add_named_command", amount);

    public EffectScope SetConvoys(int amount)
        => AddInt("set_convoys", amount);

    public EffectScope SetResearchSlots(int amount)
        => AddInt("set_research_slots", amount);

    public EffectScope SetStability(double amount)
        => AddFloat("set_stability", amount);

    public EffectScope SetWarSupport(double amount)
        => AddFloat("set_war_support", amount);

    public EffectScope SetPoliticalPower(double amount)
        => AddFloat("set_political_power", amount);

    public EffectScope AddAutonomy(double amount)
    {
        _entries.Add(new BlockEntry.NestedBlock("add_autonomy_ratio", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("value", new PropertyValue.FloatValue(amount)))
        })));
        return this;
    }

    // ─── Character / Leader Effects ───────────────────────────────

    public EffectScope AddCountryLeaderTrait(string trait)
        => AddString("add_country_leader_trait", trait);

    public EffectScope RemoveCountryLeaderTrait(string trait)
        => AddString("remove_country_leader_trait", trait);

    public EffectScope AddUnitLeaderTrait(string trait)
        => AddString("add_unit_leader_trait", trait);

    public EffectScope RemoveUnitLeaderTrait(string trait)
        => AddString("remove_unit_leader_trait", trait);

    public EffectScope KillCountryLeader(bool value = true)
        => AddBool("kill_country_leader", value);

    public EffectScope CreateCountryLeader(Action<EffectScope> configure)
        => AddNestedEffect("create_country_leader", configure);

    public EffectScope CreateFieldMarshal(Action<EffectScope> configure)
        => AddNestedEffect("create_field_marshal", configure);

    public EffectScope CreateCorpsCommander(Action<EffectScope> configure)
        => AddNestedEffect("create_corps_commander", configure);

    public EffectScope CreateNavyLeader(Action<EffectScope> configure)
        => AddNestedEffect("create_navy_leader", configure);

    // ─── Operative Effects ────────────────────────────────────────

    public EffectScope CreateOperativeLeader(Action<EffectScope> configure)
        => AddNestedEffect("create_operative_leader", configure);

    public EffectScope CaptureOperative(Action<EffectScope> configure)
        => AddNestedEffect("capture_operative", configure);

    // ─── Hidden Effects ───────────────────────────────────────────

    public EffectScope HiddenEffect(Action<EffectScope> configure)
        => AddNestedEffect("hidden_effect", configure);

    public EffectScope IfBlock(Action<EffectScope> configure)
        => AddNestedEffect("if", configure);

    public EffectScope ElseIfBlock(Action<EffectScope> configure)
        => AddNestedEffect("else_if", configure);

    public EffectScope ElseBlock(Action<EffectScope> configure)
        => AddNestedEffect("else", configure);

    public EffectScope Limit(Action<TriggerScope> configure)
    {
        var inner = new TriggerScope();
        configure(inner);
        _entries.Add(new BlockEntry.NestedBlock("limit", inner.Build()));
        return this;
    }

    // ─── Scope Changes ────────────────────────────────────────────

    public EffectScope Every(string scope, Action<EffectScope> configure)
        => AddNestedEffect($"every_{scope}", configure);

    public EffectScope Random(string scope, Action<EffectScope> configure)
        => AddNestedEffect($"random_{scope}", configure);

    public EffectScope TargetCountry(CountryTag tag, Action<EffectScope> configure)
        => AddNestedEffect(tag.Value, configure);

    public EffectScope TargetState(int stateId, Action<EffectScope> configure)
        => AddNestedEffect(stateId.ToString(), configure);

    public EffectScope EveryOwnedState(Action<EffectScope> configure)
        => AddNestedEffect("every_owned_state", configure);

    public EffectScope EveryControlledState(Action<EffectScope> configure)
        => AddNestedEffect("every_controlled_state", configure);

    public EffectScope RandomOwnedState(Action<EffectScope> configure)
        => AddNestedEffect("random_owned_state", configure);

    public EffectScope EveryCountry(Action<EffectScope> configure)
        => AddNestedEffect("every_country", configure);

    public EffectScope RandomCountry(Action<EffectScope> configure)
        => AddNestedEffect("random_country", configure);

    public EffectScope EveryNeighborCountry(Action<EffectScope> configure)
        => AddNestedEffect("every_neighbor_country", configure);

    public EffectScope EveryEnemyCountry(Action<EffectScope> configure)
        => AddNestedEffect("every_enemy_country", configure);

    public EffectScope EveryOccupiedCountry(Action<EffectScope> configure)
        => AddNestedEffect("every_occupied_country", configure);

    public EffectScope EveryUnitLeader(Action<EffectScope> configure)
        => AddNestedEffect("every_unit_leader", configure);

    public EffectScope RandomUnitLeader(Action<EffectScope> configure)
        => AddNestedEffect("random_unit_leader", configure);

    // ─── Scripted Effects ─────────────────────────────────────────

    public EffectScope ScriptedEffect(string name)
        => AddBool(name, true);

    public EffectScope ScriptedEffect(string name, Action<EffectScope> configure)
        => AddNestedEffect(name, configure);

    // ─── Misc Effects ─────────────────────────────────────────────

    public EffectScope SetAllowTraining(bool value)
        => AddBool("set_allow_training", value);

    public EffectScope Log(string message)
        => AddString("log", message);

    public EffectScope ActivateMission(string mission)
        => AddString("activate_mission", mission);

    public EffectScope CompleteMission(string mission)
        => AddString("complete_national_focus", mission);

    public EffectScope UnlockDecisionTooltip(string decisionId)
        => AddString("unlock_decision_tooltip", decisionId);

    public EffectScope ActivateDecision(string decisionId)
        => AddString("activate_targeted_decision", decisionId);

    public EffectScope RemoveDecision(string decisionId)
        => AddString("remove_targeted_decision", decisionId);

    public EffectScope LoadOob(string oobName)
        => AddString("load_oob", oobName);

    public EffectScope SetOob(string oobName)
        => AddString("set_oob", oobName);

    public EffectScope SetNavalOob(string oobName)
        => AddString("set_naval_oob", oobName);

    public EffectScope SetAirOob(string oobName)
        => AddString("set_air_oob", oobName);

    public EffectScope AddAiStrategy(Action<EffectScope> configure)
        => AddNestedEffect("add_ai_strategy", configure);

    public EffectScope ForceGovernment(string ideology)
        => AddString("force_government_change", ideology);

    public EffectScope StartCivilWar(Action<EffectScope> configure)
        => AddNestedEffect("start_civil_war", configure);

    public EffectScope ModifyBuildingResources(string building, int amount)
    {
        _entries.Add(new BlockEntry.NestedBlock("modify_building_resources", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("building", new PropertyValue.StringValue(building))),
            new BlockEntry.PropertyEntry(new Property("amount", new PropertyValue.IntValue(amount)))
        })));
        return this;
    }

    public EffectScope RetireCharacterById(string characterId)
        => AddString("retire_character", characterId);

    public EffectScope SetNationality(CountryTag country, string characterId)
    {
        _entries.Add(new BlockEntry.NestedBlock("set_nationality", new Block(new BlockEntry[]
        {
            new BlockEntry.PropertyEntry(new Property("target_country", new PropertyValue.StringValue(country.Value))),
            new BlockEntry.PropertyEntry(new Property("character", new PropertyValue.StringValue(characterId)))
        })));
        return this;
    }

    // ─── Escape Hatch ─────────────────────────────────────────────

    public EffectScope Custom(string key, string value) => AddString(key, value);
    public EffectScope Custom(string key, int value) => AddInt(key, value);
    public EffectScope Custom(string key, double value) => AddFloat(key, value);
    public EffectScope Custom(string key, bool value) => AddBool(key, value);
    public EffectScope CustomBlock(string key, Action<EffectScope> configure) => AddNestedEffect(key, configure);

    // ─── Internal Build ───────────────────────────────────────────

    internal Block Build() => new(_entries);

    private EffectScope AddString(string key, string value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(new Property(key, new PropertyValue.StringValue(value))));
        return this;
    }

    private EffectScope AddInt(string key, int value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(new Property(key, new PropertyValue.IntValue(value))));
        return this;
    }

    private EffectScope AddFloat(string key, double value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(new Property(key, new PropertyValue.FloatValue(value))));
        return this;
    }

    private EffectScope AddBool(string key, bool value)
    {
        _entries.Add(new BlockEntry.PropertyEntry(new Property(key, new PropertyValue.BoolValue(value))));
        return this;
    }

    private EffectScope AddNestedEffect(string name, Action<EffectScope> configure)
    {
        var inner = new EffectScope();
        configure(inner);
        _entries.Add(new BlockEntry.NestedBlock(name, inner.Build()));
        return this;
    }
}
