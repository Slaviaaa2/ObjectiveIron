using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Core.Models;

public class EventOption
{
    public string NameIdentifier { get; set; } = string.Empty;
    public Block? Trigger { get; set; }
    public Block? AiChance { get; set; }
    public Block? Effects { get; set; }
}
