using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Emitter.Emitters;

/// <summary>
/// Emits Events grouped by namespace to Clausewitz format.
/// 
/// Output structure:
/// <code>
/// add_namespace = test
/// 
/// country_event = {
///     id = test.1
///     ...
/// }
/// </code>
/// </summary>
public static class EventEmitter
{
    public static void Emit(string namespaceName, IEnumerable<Event> events, ClausewitzWriter writer)
    {
        writer.WriteProperty("add_namespace", namespaceName);
        writer.WriteBlankLine();

        foreach (var ev in events)
        {
            var eventTypeBlock = ev.Type switch
            {
                EventType.Country => "country_event",
                EventType.State => "state_event",
                EventType.UnitLeader => "unit_leader_event",
                EventType.Operative => "operative_leader_event",
                EventType.News => "news_event",
                _ => "country_event"
            };

            writer.BeginBlock(eventTypeBlock);

            writer.WriteProperty("id", ev.Id);
            writer.WriteProperty("title", ev.Id + ".t");
            writer.WriteProperty("desc", ev.Id + ".d");

            if (ev.Picture is not null)
                writer.WriteProperty("picture", ev.Picture);

            if (ev.Major)
                writer.WriteProperty("major", true);

            if (ev.Hidden)
                writer.WriteProperty("hidden", true);

            if (ev.IsTriggeredOnly)
                writer.WriteProperty("is_triggered_only", true);

            if (ev.FireOnlyOnce)
                writer.WriteProperty("fire_only_once", true);

            if (ev.Trigger is not null)
                BlockEmitter.Emit("trigger", ev.Trigger, writer);

            if (ev.Immediate is not null)
                BlockEmitter.Emit("immediate", ev.Immediate, writer);

            foreach (var opt in ev.Options)
            {
                writer.BeginBlock("option");
                writer.WriteProperty("name", opt.NameIdentifier);

                if (opt.Trigger is not null)
                    BlockEmitter.Emit("trigger", opt.Trigger, writer);

                if (opt.AiChance is not null)
                    BlockEmitter.Emit("ai_chance", opt.AiChance, writer);

                if (opt.Effects is not null)
                {
                    foreach (var entry in opt.Effects.Entries)
                    {
                        if (entry is BlockEntry.NestedBlock b)
                            BlockEmitter.Emit(b.Name, b.Block, writer);
                        else if (entry is BlockEntry.PropertyEntry p)
                            writer.WriteProperty(p.Property);
                    }
                }

                writer.EndBlock();
            }

            writer.EndBlock();
            writer.WriteBlankLine();
        }
    }
}
