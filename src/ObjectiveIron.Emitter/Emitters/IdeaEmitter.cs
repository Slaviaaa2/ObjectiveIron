using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Emitter.Emitters;

public static class IdeaEmitter
{
    public static void Emit(IEnumerable<Idea> ideas, ClausewitzWriter writer)
    {
        writer.BeginBlock("ideas");
        
        var ideasByCategory = ideas.GroupBy(i => i.Category);
        
        foreach (var categoryGroup in ideasByCategory)
        {
            writer.BeginBlock(categoryGroup.Key);
            
            foreach (var idea in categoryGroup)
            {
                writer.BeginBlock(idea.Id);

                if (!string.IsNullOrEmpty(idea.DynamicName))
                    writer.WriteProperty("name", idea.DynamicName);

                if (!string.IsNullOrEmpty(idea.DynamicDescription))
                    writer.WriteProperty("desc", idea.DynamicDescription);
                
                if (!string.IsNullOrEmpty(idea.Picture))
                    writer.WriteProperty("picture", idea.Picture);

                if (idea.Available != null)
                    BlockEmitter.Emit("allowed", idea.Available, writer);

                if (idea.Visible != null)
                    BlockEmitter.Emit("visible", idea.Visible, writer);
                
                if (idea.Modifier != null && idea.Modifier.Entries.Count > 0)
                {
                    writer.BeginBlock("modifier");
                    BlockEmitter.EmitContents(idea.Modifier, writer);
                    writer.EndBlock();
                }
                
                writer.EndBlock();
            }
            
            writer.EndBlock();
        }
        
        writer.EndBlock();
    }
}
