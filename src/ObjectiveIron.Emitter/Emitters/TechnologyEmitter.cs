using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits Technology nodes to Clausewitz format.
/// Matches vanilla HOI4 technology file structure.
/// </summary>
public static class TechnologyEmitter
{
    public static void Emit(IEnumerable<Technology> technologies, ClausewitzWriter writer)
    {
        writer.BeginBlock("technologies");

        foreach (var tech in technologies)
        {
            writer.BeginBlock(tech.Id);

            // ── Unit category modifiers (before metadata) ──
            if (tech.UnitCategoryModifiers != null)
            {
                foreach (var ucm in tech.UnitCategoryModifiers)
                {
                    BlockEmitter.Emit(ucm.Category, ucm.Modifiers, writer);
                }
            }

            // ── Equipment unlocks ──
            if (tech.EquipmentUnlocks != null)
            {
                writer.BeginBlock("enable_equipments");
                foreach (var unlock in tech.EquipmentUnlocks)
                {
                    writer.WriteUnquoted(unlock.EquipmentId);
                }
                writer.EndBlock();
            }

            // ── Equipment module unlocks ──
            if (tech.EnableEquipmentModules != null)
            {
                writer.BeginBlock("enable_equipment_modules");
                foreach (var mod in tech.EnableEquipmentModules)
                {
                    writer.WriteUnquoted(mod);
                }
                writer.EndBlock();
            }

            // ── Subunit unlocks ──
            if (tech.EnableSubunits != null)
            {
                writer.BeginBlock("enable_subunits");
                foreach (var sub in tech.EnableSubunits)
                {
                    writer.WriteUnquoted(sub);
                }
                writer.EndBlock();
            }

            // ── Paths (arrows) ──
            if (tech.Paths != null)
            {
                foreach (var path in tech.Paths)
                {
                    writer.BeginBlock("path");
                    writer.WriteProperty("leads_to_tech", path.LeadsToTech);
                    writer.WriteProperty("research_cost_coeff", path.ResearchCostCoeff);
                    writer.EndBlock();
                }
            }

            // ── Core properties ──
            if (tech.Cost.HasValue)
                writer.WriteProperty("research_cost", tech.Cost.Value);

            if (tech.Year.HasValue)
                writer.WriteProperty("start_year", tech.Year.Value);

            if (tech.Folder != null)
            {
                writer.BeginBlock("folder");
                writer.WriteProperty("name", tech.Folder);
                if (tech.Position != null)
                {
                    writer.BeginBlock("position");
                    writer.WriteProperty("x", tech.Position.X);
                    writer.WriteProperty("y", tech.Position.Y);
                    writer.EndBlock();
                }
                writer.EndBlock();
            }

            // ── Categories ──
            if (tech.Categories != null)
            {
                writer.BeginBlock("categories");
                foreach (var cat in tech.Categories)
                {
                    writer.WriteUnquoted(cat);
                }
                writer.EndBlock();
            }

            // ── Prerequisites ──
            if (tech.Prerequisites != null)
            {
                foreach (var prereq in tech.Prerequisites)
                {
                    writer.BeginBlock("prerequisites");
                    writer.WriteUnquoted(prereq);
                    writer.EndBlock();
                }
            }

            // ── Conditional blocks ──
            if (tech.Allow != null)
                BlockEmitter.Emit("allow", tech.Allow, writer);

            if (tech.Modifier != null)
                BlockEmitter.Emit("modifier", tech.Modifier, writer);

            if (tech.AiWillDo != null)
                BlockEmitter.Emit("ai_will_do", tech.AiWillDo, writer);

            writer.EndBlock();
            writer.WriteBlankLine();
        }

        writer.EndBlock();
    }
}
