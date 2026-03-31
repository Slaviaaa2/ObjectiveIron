using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// A category that groups decisions together.
/// Decisions must refer to a Category ID.
/// </summary>
public abstract class DecisionCategoryDefinition
{
    public abstract string Id { get; }
    public abstract LocalizedText Name { get; }
    
    public virtual GfxSprite? Icon => null;
    
    protected virtual void Visible(TriggerScope t) { }
    protected virtual void Available(TriggerScope t) { }

    public Block? CompileVisible()
    {
        var scope = new TriggerScope();
        Visible(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }

    public Block? CompileAvailable()
    {
        var scope = new TriggerScope();
        Available(scope);
        var block = scope.Build();
        return block.Entries.Count > 0 ? block : null;
    }
}
