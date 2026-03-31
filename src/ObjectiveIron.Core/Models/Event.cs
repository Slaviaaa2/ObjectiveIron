using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

public class Event
{
    public string Id { get; set; } = string.Empty;
    public EventType Type { get; set; }
    public string? Picture { get; set; }
    
    public bool IsTriggeredOnly { get; set; }
    public bool FireOnlyOnce { get; set; }
    public bool Hidden { get; set; }
    public bool Major { get; set; }

    public Block? Trigger { get; set; }
    public Block? Immediate { get; set; }

    public IReadOnlyList<EventOption> Options { get; set; } = [];
}
