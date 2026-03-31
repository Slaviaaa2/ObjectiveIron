using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits sub-unit definition files: common/units/{FileName}.txt
/// </summary>
public static class SubUnitEmitter
{
    public static void Emit(SubUnitFileBuildResult file, ClausewitzWriter writer)
    {
        writer.BeginBlock("sub_units");

        foreach (var unit in file.Units)
        {
            writer.BeginBlock(unit.Id);

            if (unit.Abbreviation != null) writer.WriteProperty("abbreviation", $"\"{unit.Abbreviation}\"");
            if (unit.Sprite != null) writer.WriteProperty("sprite", unit.Sprite);
            if (unit.MapIconCategory != null) writer.WriteProperty("map_icon_category", unit.MapIconCategory);
            if (unit.Priority.HasValue) writer.WriteProperty("priority", unit.Priority.Value);
            if (unit.AiPriority.HasValue) writer.WriteProperty("ai_priority", unit.AiPriority.Value);
            if (unit.Active.HasValue) writer.WriteProperty("active", unit.Active.Value);
            if (unit.AffectsSpeed.HasValue) writer.WriteProperty("affects_speed", unit.AffectsSpeed.Value);
            if (unit.IsArtilleryBrigade.HasValue) writer.WriteProperty("is_artillery_brigade", unit.IsArtilleryBrigade.Value);
            if (unit.SpecialForces.HasValue) writer.WriteProperty("special_forces", unit.SpecialForces.Value);
            if (unit.CanBeParachuted.HasValue) writer.WriteProperty("can_be_parachuted", unit.CanBeParachuted.Value);

            writer.WriteBlankLine();

            // Type block
            if (unit.Types is { Count: > 0 })
            {
                writer.BeginBlock("type");
                foreach (var t in unit.Types)
                    writer.WriteUnquoted(t);
                writer.EndBlock();
                writer.WriteBlankLine();
            }

            if (unit.Group != null) writer.WriteProperty("group", unit.Group);

            // Categories
            if (unit.Categories is { Count: > 0 })
            {
                writer.BeginBlock("categories");
                foreach (var cat in unit.Categories)
                    writer.WriteUnquoted(cat);
                writer.EndBlock();
                writer.WriteBlankLine();
            }

            // Combat stats
            if (unit.CombatWidth.HasValue) writer.WriteProperty("combat_width", unit.CombatWidth.Value);
            writer.WriteBlankLine();
            if (unit.MaxStrength.HasValue) writer.WriteProperty("max_strength", unit.MaxStrength.Value);
            if (unit.MaxOrganisation.HasValue) writer.WriteProperty("max_organisation", unit.MaxOrganisation.Value);
            if (unit.DefaultMorale.HasValue) writer.WriteProperty("default_morale", unit.DefaultMorale.Value);
            if (unit.Manpower.HasValue) writer.WriteProperty("manpower", unit.Manpower.Value);
            if (unit.TrainingTime.HasValue) writer.WriteProperty("training_time", unit.TrainingTime.Value);
            if (unit.Suppression.HasValue) writer.WriteProperty("suppression", unit.Suppression.Value);
            if (unit.Weight.HasValue) writer.WriteProperty("weight", unit.Weight.Value);
            if (unit.SupplyConsumption.HasValue) writer.WriteProperty("supply_consumption", unit.SupplyConsumption.Value);
            if (unit.MaximumSpeed.HasValue) writer.WriteProperty("maximum_speed", unit.MaximumSpeed.Value);
            if (unit.Breakthrough.HasValue) writer.WriteProperty("breakthrough", unit.Breakthrough.Value);
            if (unit.Defense.HasValue) writer.WriteProperty("defense", unit.Defense.Value);
            if (unit.SoftAttack.HasValue) writer.WriteProperty("soft_attack", unit.SoftAttack.Value);
            if (unit.HardAttack.HasValue) writer.WriteProperty("hard_attack", unit.HardAttack.Value);
            if (unit.Entrenchment.HasValue) writer.WriteProperty("entrenchment", unit.Entrenchment.Value);

            // Essential
            if (unit.Essential is { Count: > 0 })
            {
                writer.WriteBlankLine();
                writer.BeginBlock("essential");
                foreach (var eq in unit.Essential)
                    writer.WriteUnquoted(eq);
                writer.EndBlock();
            }

            // Need
            if (unit.Need is { Count: > 0 })
            {
                writer.WriteBlankLine();
                writer.BeginBlock("need");
                foreach (var (eq, amount) in unit.Need)
                    writer.WriteProperty(eq, amount);
                writer.EndBlock();
            }

            // Battalion multipliers
            if (unit.BattalionMults is { Count: > 0 })
            {
                foreach (var bm in unit.BattalionMults)
                {
                    writer.WriteBlankLine();
                    writer.BeginBlock("battalion_mult");
                    writer.WriteProperty("category", bm.Category);
                    writer.WriteProperty(bm.Property, bm.Value);
                    if (bm.Add)
                        writer.WriteProperty("add", true);
                    writer.EndBlock();
                }
            }

            // Terrain modifiers
            if (unit.TerrainModifiers is { Count: > 0 })
            {
                writer.WriteBlankLine();
                foreach (var tm in unit.TerrainModifiers)
                {
                    writer.BeginBlock(tm.Terrain);
                    if (tm.Attack.HasValue) writer.WriteProperty("attack", tm.Attack.Value);
                    if (tm.Defence.HasValue) writer.WriteProperty("defence", tm.Defence.Value);
                    if (tm.Movement.HasValue) writer.WriteProperty("movement", tm.Movement.Value);
                    writer.EndBlock();
                }
            }

            writer.EndBlock(); // unit id
            writer.WriteBlankLine();
        }

        writer.EndBlock(); // sub_units
    }
}
