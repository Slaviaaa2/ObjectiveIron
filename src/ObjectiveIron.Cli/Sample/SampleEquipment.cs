using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleInfantryEquipment : EquipmentDefinition
{
    public override string Id => "sample_infantry";

    protected override void Define(EquipmentSetScope s)
    {
        s.Archetype("sample_infantry_equipment", a => a
            .Year(1936)
            .IsBuildable(false)
            .Picture("archetype_infantry_equipment")
            .Type("infantry")
            .GroupBy("archetype")
            .InterfaceCategory("interface_category_land")
            .Active(true)
            .Reliability(0.9)
            .MaximumSpeed(4)
            .Defense(20)
            .Breakthrough(2)
            .Hardness(0)
            .ArmorValue(0)
            .SoftAttack(3)
            .HardAttack(0.5)
            .ApAttack(1)
            .AirAttack(0)
            .LendLeaseCost(1)
            .BuildCostIc(0.43)
            .Steel(2)
        );

        s.Variant("sample_infantry_equipment_1", "sample_infantry_equipment", v => v
            .Year(1936)
            .Parent("sample_infantry_equipment_0")
            .Priority(10)
            .VisualLevel(1)
            .Defense(22)
            .Breakthrough(3)
            .SoftAttack(6)
            .HardAttack(1)
            .ApAttack(4)
            .BuildCostIc(0.5)
        );

        s.Variant("sample_infantry_equipment_2", "sample_infantry_equipment", v => v
            .Year(1939)
            .Parent("sample_infantry_equipment_1")
            .Priority(10)
            .VisualLevel(2)
            .Defense(28)
            .Breakthrough(4)
            .SoftAttack(9)
            .HardAttack(1.5)
            .ApAttack(5)
            .BuildCostIc(0.58)
            .Steel(3)
        );
    }
}
