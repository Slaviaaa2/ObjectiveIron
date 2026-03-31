using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Builders.Compiler;

public static class DecisionCompiler
{
    public static DecisionCategory CompileCategory(DecisionCategoryDefinition def, IEnumerable<DecisionDefinition> decisionDefs)
    {
        var category = new DecisionCategory
        {
            Id = def.Id,
            NameIdentifier = def.Id, // Default name key
            Icon = def.Icon?.Value,
            Visible = def.CompileVisible(),
            Available = def.CompileAvailable(),
            Decisions = []
        };

        foreach (var dDef in decisionDefs)
        {
            category.Decisions.Add(CompileDecision(dDef));
        }

        return category;
    }

    public static Decision CompileDecision(DecisionDefinition def)
    {
        return new Decision
        {
            Id = def.Id,
            NameIdentifier = def.Id,
            Icon = def.Icon?.Value,
            Cost = def.Cost,
            DaysToRemove = def.DaysToRemove,
            IsVisible = def.VisibleByDefault,
            Visible = def.CompileVisible(),
            Available = def.CompileAvailable(),
            CompleteEffect = def.CompileComplete(),
            AiWillDo = def.CompileAi(),
            TimeoutEffect = def.CompileTimeout()
        };
    }
}
