using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class BuildingEmitter
{
    public static void Emit(BuildingFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("buildings");
        foreach (var b in file.Buildings)
        {
            writer.BeginBlock(b.Id);
            if (b.MaxLevel.HasValue) writer.WriteProperty("max_level", b.MaxLevel.Value);
            if (b.BaseHealthPerLevel.HasValue) writer.WriteProperty("base_health", b.BaseHealthPerLevel.Value);
            if (b.Value.HasValue) writer.WriteProperty("value", b.Value.Value);
            if (b.IconFrame != null) writer.WriteProperty("icon_frame", b.IconFrame);
            if (b.Provincial.HasValue) writer.WriteProperty("provincial", b.Provincial.Value);
            if (b.PerState.HasValue) writer.WriteProperty("per_state", b.PerState.Value);
            if (b.OnlyCoastal.HasValue) writer.WriteProperty("only_costal", b.OnlyCoastal.Value);
            if (b.Modifiers is { Count: > 0 })
            {
                writer.BeginBlock("modifier");
                foreach (var (k, v) in b.Modifiers) writer.WriteProperty(k, v);
                writer.EndBlock();
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
        writer.EndBlock();
    }
}

public static class StaticModifierEmitter
{
    public static void Emit(StaticModifierFileBuild file, ClausewitzWriter writer)
    {
        foreach (var mod in file.Modifiers)
        {
            writer.BeginBlock(mod.Id);
            foreach (var (k, v) in mod.Modifiers) writer.WriteProperty(k, v);
            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}

public static class GameRuleEmitter
{
    public static void Emit(GameRuleFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("game_rules");
        foreach (var rule in file.Rules)
        {
            writer.BeginBlock(rule.Id);
            writer.WriteProperty("default", rule.DefaultOption);
            foreach (var opt in rule.Options)
            {
                writer.BeginBlock(opt.Name);
                if (opt.Text != null) writer.WriteProperty("text", $"\"{opt.Text}\"");
                if (opt.Desc != null) writer.WriteProperty("desc", $"\"{opt.Desc}\"");
                if (opt.AllowAchievements.HasValue) writer.WriteProperty("allow_achievements", opt.AllowAchievements.Value);
                writer.EndBlock();
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
        writer.EndBlock();
    }
}

public static class TerrainEmitter
{
    public static void Emit(TerrainFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("categories");
        foreach (var t in file.Terrains)
        {
            writer.BeginBlock(t.Id);
            if (t.Color.HasValue) writer.WriteProperty("color", t.Color.Value);
            if (t.MovementCost.HasValue) writer.WriteProperty("movement_cost", t.MovementCost.Value);
            if (t.Attrition.HasValue) writer.WriteProperty("attrition", t.Attrition.Value);
            if (t.IsWater.HasValue) writer.WriteProperty("is_water", t.IsWater.Value);
            if (t.Modifiers is { Count: > 0 })
            {
                foreach (var (k, v) in t.Modifiers) writer.WriteProperty(k, v);
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
        writer.EndBlock();
    }
}

public static class StateCategoryEmitter
{
    public static void Emit(StateCategoryFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("state_categories");
        foreach (var cat in file.Categories)
        {
            writer.BeginBlock(cat.Id);
            writer.WriteProperty("local_building_slots", cat.LocalBuildingSlots);
            if (cat.Color.HasValue) writer.WriteProperty("color", cat.Color.Value);
            writer.EndBlock();
            writer.WriteBlankLine();
        }
        writer.EndBlock();
    }
}

public static class ResourceDefEmitter
{
    public static void Emit(ResourceDefFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("resources");
        foreach (var r in file.Resources)
        {
            writer.BeginBlock(r.Id);
            if (r.IconFrame.HasValue) writer.WriteProperty("icon_frame", r.IconFrame.Value);
            if (r.Cic.HasValue) writer.WriteProperty("cic", r.Cic.Value);
            writer.EndBlock();
        }
        writer.EndBlock();
    }
}

public static class TechSharingEmitter
{
    public static void Emit(TechSharingFileBuild file, ClausewitzWriter writer)
    {
        foreach (var group in file.Groups)
        {
            writer.BeginBlock(group.Id);
            if (group.Category != null) writer.WriteProperty("category", group.Category);
            if (group.ResearchBonus.HasValue) writer.WriteProperty("research_bonus", group.ResearchBonus.Value);
            writer.BeginBlock("members");
            foreach (var m in group.Members) writer.WriteUnquoted(m);
            writer.EndBlock();
            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}

public static class IdeologyEmitter
{
    public static void Emit(IdeologyFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("ideologies");
        foreach (var ideo in file.Ideologies)
        {
            writer.BeginBlock(ideo.Id);
            if (ideo.Color.HasValue)
            {
                var c = ideo.Color.Value;
                writer.WriteProperty("color", $"{{ {c.R} {c.G} {c.B} }}");
            }
            if (ideo.CanHostGovernmentInExile.HasValue) writer.WriteProperty("can_host_government_in_exile", ideo.CanHostGovernmentInExile.Value);
            if (ideo.WarImpact.HasValue) writer.WriteProperty("war_impact_on_world_tension", ideo.WarImpact.Value);
            if (ideo.CanBeBoosted.HasValue) writer.WriteProperty("can_be_boosted", ideo.CanBeBoosted.Value);
            if (ideo.SubTypes is { Count: > 0 })
            {
                writer.BeginBlock("types");
                foreach (var t in ideo.SubTypes)
                {
                    writer.BeginBlock(t);
                    writer.EndBlock();
                }
                writer.EndBlock();
            }
            if (ideo.Modifiers is { Count: > 0 })
            {
                writer.BeginBlock("modifiers");
                foreach (var (k, v) in ideo.Modifiers) writer.WriteProperty(k, v);
                writer.EndBlock();
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
        writer.EndBlock();
    }
}

public static class AutonomyStateEmitter
{
    public static void Emit(AutonomyStateFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("autonomy_states");
        foreach (var state in file.States)
        {
            writer.BeginBlock(state.Id);
            if (state.MinFreedomLevel.HasValue) writer.WriteProperty("min_freedom_level", state.MinFreedomLevel.Value);
            if (state.IsSubject.HasValue) writer.WriteProperty("is_subject", state.IsSubject.Value);
            if (state.CanForceGovernment.HasValue) writer.WriteProperty("can_force_government", state.CanForceGovernment.Value);
            if (state.Modifiers is { Count: > 0 })
            {
                writer.BeginBlock("modifier");
                foreach (var (k, v) in state.Modifiers) writer.WriteProperty(k, v);
                writer.EndBlock();
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
        writer.EndBlock();
    }
}

public static class DifficultyEmitter
{
    public static void Emit(DifficultyFileBuild file, ClausewitzWriter writer)
    {
        writer.BeginBlock("difficulty_settings");
        foreach (var d in file.Settings)
        {
            writer.BeginBlock(d.Id);
            if (d.Name != null) writer.WriteProperty("name", $"\"{d.Name}\"");
            if (d.IsDefault.HasValue) writer.WriteProperty("default", d.IsDefault.Value);
            if (d.Modifiers is { Count: > 0 })
            {
                writer.BeginBlock("modifier");
                foreach (var (k, v) in d.Modifiers) writer.WriteProperty(k, v);
                writer.EndBlock();
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
        writer.EndBlock();
    }
}

public static class NameEmitter
{
    public static void Emit(NameFileBuild file, ClausewitzWriter writer)
    {
        foreach (var group in file.Groups)
        {
            writer.BeginBlock(group.Tag);
            foreach (var (key, names) in group.NameGroups)
            {
                writer.BeginBlock(key);
                foreach (var name in names) writer.WriteUnquoted($"\"{name}\"");
                writer.EndBlock();
            }
            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}
