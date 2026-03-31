using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Builders.Compiler;

public static class IdeaCompiler
{
    public static Idea Compile(IdeaDefinition def)
    {
        return new Idea
        {
            Id = def.Id,
            Name = def.Name,
            Description = def.Description,
            Picture = def.Picture,
            Category = def.Category,
            Modifier = def.CompileModifier()
            // EquipmentBonus and Rule to be added in future if needed
        };
    }
}
