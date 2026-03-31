using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits equipment definition files: common/units/equipment/{FileName}.txt
/// </summary>
public static class EquipmentEmitter
{
    public static void Emit(EquipmentFileBuildResult file, ClausewitzWriter writer)
    {
        writer.BeginBlock("equipments");

        foreach (var eq in file.Equipment)
        {
            writer.WriteBlankLine();
            writer.BeginBlock(eq.Id);

            if (eq.Year.HasValue) writer.WriteProperty("year", eq.Year.Value);

            if (eq.IsArchetype)
            {
                writer.WriteProperty("is_archetype", true);
                if (eq.IsBuildable.HasValue) writer.WriteProperty("is_buildable", eq.IsBuildable.Value);
            }
            else
            {
                if (eq.Archetype != null) writer.WriteProperty("archetype", eq.Archetype);
                if (eq.Parent != null) writer.WriteProperty("parent", eq.Parent);
                if (eq.Priority.HasValue) writer.WriteProperty("priority", eq.Priority.Value);
                if (eq.VisualLevel.HasValue) writer.WriteProperty("visual_level", eq.VisualLevel.Value);
            }

            if (eq.Picture != null) writer.WriteProperty("picture", eq.Picture);

            // Type
            if (eq.Types is { Count: > 0 })
            {
                if (eq.Types.Count == 1)
                {
                    writer.WriteProperty("type", eq.Types[0]);
                }
                else
                {
                    writer.BeginBlock("type");
                    foreach (var t in eq.Types)
                        writer.WriteUnquoted(t);
                    writer.EndBlock();
                }
            }

            if (eq.GroupBy != null) writer.WriteProperty("group_by", eq.GroupBy);
            if (eq.InterfaceCategory != null) writer.WriteProperty("interface_category", eq.InterfaceCategory);
            if (eq.Active.HasValue) writer.WriteProperty("active", eq.Active.Value);

            writer.WriteBlankLine();

            // Stats
            if (eq.Reliability.HasValue) writer.WriteProperty("reliability", eq.Reliability.Value);
            if (eq.MaximumSpeed.HasValue) writer.WriteProperty("maximum_speed", eq.MaximumSpeed.Value);
            if (eq.Defense.HasValue) writer.WriteProperty("defense", eq.Defense.Value);
            if (eq.Breakthrough.HasValue) writer.WriteProperty("breakthrough", eq.Breakthrough.Value);
            if (eq.Hardness.HasValue) writer.WriteProperty("hardness", eq.Hardness.Value);
            if (eq.ArmorValue.HasValue) writer.WriteProperty("armor_value", eq.ArmorValue.Value);
            if (eq.SoftAttack.HasValue) writer.WriteProperty("soft_attack", eq.SoftAttack.Value);
            if (eq.HardAttack.HasValue) writer.WriteProperty("hard_attack", eq.HardAttack.Value);
            if (eq.ApAttack.HasValue) writer.WriteProperty("ap_attack", eq.ApAttack.Value);
            if (eq.AirAttack.HasValue) writer.WriteProperty("air_attack", eq.AirAttack.Value);

            // Logistics
            if (eq.LendLeaseCost.HasValue) writer.WriteProperty("lend_lease_cost", eq.LendLeaseCost.Value);
            if (eq.BuildCostIc.HasValue) writer.WriteProperty("build_cost_ic", eq.BuildCostIc.Value);
            if (eq.FuelConsumption.HasValue) writer.WriteProperty("fuel_consumption", eq.FuelConsumption.Value);

            // Resources
            if (eq.Resources is { Count: > 0 })
            {
                writer.BeginBlock("resources");
                foreach (var (res, amount) in eq.Resources)
                    writer.WriteProperty(res, amount);
                writer.EndBlock();
            }

            writer.EndBlock(); // equipment id
        }

        writer.EndBlock(); // equipments
    }
}
