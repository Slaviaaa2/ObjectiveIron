namespace ObjectiveIron.Builders.Types;

/// <summary>
/// Building types available in HOI4.
/// Used in add_building_construction and related effects.
/// </summary>
public enum BuildingType
{
    IndustrialComplex,
    ArmsFactory,
    Infrastructure,
    AirBase,
    AntiAir,
    Radar,
    RocketSite,
    NuclearReactor,
    NavalBase,
    Bunker,
    CoastalBunker,
    DockYard,
    FuelSilo,
    SyntheticRefinery,
    SupplyNode,
    Railway
}

public static class BuildingTypeExtensions
{
    public static string ToClausewitz(this BuildingType type) => type switch
    {
        BuildingType.IndustrialComplex => "industrial_complex",
        BuildingType.ArmsFactory => "arms_factory",
        BuildingType.Infrastructure => "infrastructure",
        BuildingType.AirBase => "air_base",
        BuildingType.AntiAir => "anti_air",
        BuildingType.Radar => "radar_station",
        BuildingType.RocketSite => "rocket_site",
        BuildingType.NuclearReactor => "nuclear_reactor",
        BuildingType.NavalBase => "naval_base",
        BuildingType.Bunker => "bunker",
        BuildingType.CoastalBunker => "coastal_bunker",
        BuildingType.DockYard => "dockyard",
        BuildingType.FuelSilo => "fuel_silo",
        BuildingType.SyntheticRefinery => "synthetic_refinery",
        BuildingType.SupplyNode => "supply_node",
        BuildingType.Railway => "railway",
        _ => type.ToString().ToLowerInvariant()
    };
}
