using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits on_actions blocks to Clausewitz format.
/// </summary>
public static class OnActionEmitter
{
    public static void Emit(IEnumerable<OnAction> actions, ClausewitzWriter writer)
    {
        foreach (var action in actions)
        {
            writer.BeginBlock(action.Hook);

            if (action.Effect != null)
            {
                BlockEmitter.Emit("effect", action.Effect, writer);
            }

            if (action.RandomEvents != null)
            {
                writer.BeginBlock("random_events");
                foreach (var re in action.RandomEvents)
                {
                    writer.WriteProperty(re.Weight.ToString(), re.EventId);
                }
                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}
