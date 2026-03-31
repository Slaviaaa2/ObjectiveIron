using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Emitter.Emitters;

public static class CharacterEmitter
{
    public static void Emit(ClausewitzWriter writer, IReadOnlyList<Character> characters)
    {
        writer.BeginBlock("characters");

        foreach (var character in characters)
        {
            EmitCharacter(writer, character);
        }

        writer.EndBlock();
    }

    private static void EmitCharacter(ClausewitzWriter writer, Character character)
    {
        writer.BeginBlock(character.Id);

        // 1. Name is usually handled in localisation, but we emit the key here
        // Actually, the key is the ID itself for characters in v1.11
        writer.WriteProperty("name", character.Id);

        // 2. Portraits
        if (character.Portraits != null)
        {
            writer.BeginBlock("portraits");
            
            if (character.Portraits.ArmyLarge != null || character.Portraits.ArmySmall != null)
            {
                writer.BeginBlock("army");
                if (character.Portraits.ArmyLarge != null) writer.WriteProperty("large", character.Portraits.ArmyLarge);
                if (character.Portraits.ArmySmall != null) writer.WriteProperty("small", character.Portraits.ArmySmall);
                writer.EndBlock();
            }
            
            if (character.Portraits.CivilianLarge != null || character.Portraits.CivilianSmall != null)
            {
                writer.BeginBlock("civilian");
                if (character.Portraits.CivilianLarge != null) writer.WriteProperty("large", character.Portraits.CivilianLarge);
                if (character.Portraits.CivilianSmall != null) writer.WriteProperty("small", character.Portraits.CivilianSmall);
                writer.EndBlock();
            }

            writer.EndBlock();
        }

        // 3. Roles
        foreach (var role in character.Roles)
        {
            EmitRole(writer, role);
        }

        writer.EndBlock();
    }

    private static void EmitRole(ClausewitzWriter writer, CharacterRole role)
    {
        if (role is CountryLeaderRole leader)
        {
            writer.BeginBlock("country_leader");
            writer.WriteProperty("ideology", leader.Ideology);
            if (leader.Traits.Any())
            {
                writer.WriteProperty("traits", $"{{ {string.Join(" ", leader.Traits)} }}");
            }
            writer.EndBlock();
        }
        else if (role is AdvisorRole advisor)
        {
            writer.BeginBlock("advisor");
            writer.WriteProperty("slot", advisor.Slot);
            if (advisor.Traits.Any())
            {
                writer.WriteProperty("traits", $"{{ {string.Join(" ", advisor.Traits)} }}");
            }
            writer.WriteProperty("cost", advisor.Cost);
            writer.EndBlock();
        }
        else if (role is CommanderRole commander)
        {
            var key = commander.Type switch
            {
                CommanderType.CorpsCommander => "corps_commander",
                CommanderType.FieldMarshal => "field_marshal",
                CommanderType.Admiral => "navy_leader",
                _ => "corps_commander"
            };
            
            writer.BeginBlock(key);
            writer.WriteProperty("skill", commander.Skill);
            writer.WriteProperty("attack_skill", commander.Attack);
            writer.WriteProperty("defense_skill", commander.Defense);
            writer.WriteProperty("planning_skill", commander.Planning);
            writer.WriteProperty("logistics_skill", commander.Logistics);
            if (commander.Traits != null && commander.Traits.Any())
            {
                writer.WriteProperty("traits", $"{{ {string.Join(" ", commander.Traits)} }}");
            }
            writer.EndBlock();
        }
    }
}
