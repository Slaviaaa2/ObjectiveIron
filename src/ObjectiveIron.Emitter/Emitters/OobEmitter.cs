using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits Order of Battle files: history/units/{FileName}.txt
/// </summary>
public static class OobEmitter
{
    public static void Emit(OobBuildResult oob, ClausewitzWriter writer)
    {
        // Division templates
        if (oob.Templates is { Count: > 0 })
        {
            foreach (var template in oob.Templates)
            {
                writer.BeginBlock("division_template");
                writer.WriteProperty("name", $"\"{template.Name}\"");

                if (template.DivisionNamesGroup != null)
                    writer.WriteProperty("division_names_group", template.DivisionNamesGroup);

                if (template.Regiments is { Count: > 0 })
                {
                    writer.BeginBlock("regiments");
                    foreach (var reg in template.Regiments)
                    {
                        writer.BeginBlock(reg.Type);
                        writer.WriteProperty("x", reg.X);
                        writer.WriteProperty("y", reg.Y);
                        writer.EndBlock();
                    }
                    writer.EndBlock();
                }

                if (template.Support is { Count: > 0 })
                {
                    writer.BeginBlock("support");
                    foreach (var sup in template.Support)
                    {
                        writer.BeginBlock(sup.Type);
                        writer.WriteProperty("x", sup.X);
                        writer.WriteProperty("y", sup.Y);
                        writer.EndBlock();
                    }
                    writer.EndBlock();
                }

                writer.EndBlock(); // division_template
                writer.WriteBlankLine();
            }
        }

        // Deployed units
        if (oob.Divisions is { Count: > 0 })
        {
            writer.BeginBlock("units");
            foreach (var div in oob.Divisions)
            {
                writer.BeginBlock("division");

                if (div.NameOrder.HasValue)
                {
                    writer.BeginBlock("division_name");
                    writer.WriteProperty("is_name_ordered", true);
                    writer.WriteProperty("name_order", div.NameOrder.Value);
                    writer.EndBlock();
                }

                writer.WriteProperty("location", div.Location);
                writer.WriteProperty("division_template", $"\"{div.Template}\"");

                if (div.StartExperienceFactor.HasValue)
                    writer.WriteProperty("start_experience_factor", div.StartExperienceFactor.Value);

                writer.EndBlock(); // division
            }
            writer.EndBlock(); // units
            writer.WriteBlankLine();
        }

        // Instant effect
        if (oob.InstantEffect is { Entries.Count: > 0 })
        {
            writer.BeginBlock("instant_effect");
            BlockEmitter.EmitContents(oob.InstantEffect, writer);
            writer.EndBlock();
        }
    }

    public static void EmitNaval(NavalOobBuildResult oob, ClausewitzWriter writer)
    {
        if (oob.Fleets is not { Count: > 0 }) return;

        foreach (var fleet in oob.Fleets)
        {
            writer.BeginBlock("fleet");
            writer.WriteProperty("name", $"\"{fleet.Name}\"");
            writer.WriteProperty("naval_base", fleet.NavalBase);

            if (fleet.TaskForces is { Count: > 0 })
            {
                foreach (var tf in fleet.TaskForces)
                {
                    writer.BeginBlock("task_force");
                    writer.WriteProperty("name", $"\"{tf.Name}\"");
                    writer.WriteProperty("location", tf.Location);

                    if (tf.Ships is { Count: > 0 })
                    {
                        foreach (var ship in tf.Ships)
                        {
                            writer.BeginBlock("ship");
                            writer.WriteProperty("name", $"\"{ship.Name}\"");
                            writer.WriteProperty("definition", ship.Definition);

                            writer.BeginBlock("equipment");
                            writer.BeginBlock(ship.Equipment);
                            writer.WriteProperty("amount", 1);
                            writer.WriteProperty("owner", "THIS");
                            writer.EndBlock();
                            writer.EndBlock();

                            writer.WriteProperty("version_name", $"\"{ship.Version}\"");

                            if (ship.Experience.HasValue)
                                writer.WriteProperty("start_experience_factor", ship.Experience.Value);

                            writer.EndBlock(); // ship
                        }
                    }

                    writer.EndBlock(); // task_force
                }
            }

            writer.EndBlock(); // fleet
            writer.WriteBlankLine();
        }
    }

    public static void EmitAir(AirOobBuildResult oob, ClausewitzWriter writer)
    {
        if (oob.Wings is not { Count: > 0 }) return;

        writer.BeginBlock("air_wings");

        foreach (var wing in oob.Wings)
        {
            writer.BeginBlock(wing.StateId.ToString());

            if (wing.Entries is { Count: > 0 })
            {
                foreach (var entry in wing.Entries)
                {
                    writer.BeginBlock(entry.Type);

                    if (entry.Name != null)
                        writer.WriteProperty("name", $"\"{entry.Name}\"");

                    writer.WriteProperty("owner", "THIS");
                    writer.WriteProperty("amount", entry.Amount);

                    writer.BeginBlock("equipment");
                    writer.WriteProperty(entry.Equipment, 1);
                    writer.EndBlock();

                    writer.EndBlock(); // type
                }
            }

            writer.EndBlock(); // state
        }

        writer.EndBlock(); // air_wings
    }
}
