using ObjectiveIron.Builders.Types;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Base class for defining a focus tree.
/// Inherit this class, declare focus instances as properties, and define
/// their relationships in Structure().
/// 
/// <example>
/// <code>
/// public class GermanFocusTree : FocusTreeDefinition
/// {
///     public override string Id => "german_focus_tree";
///     public override CountryTag Country => Tags.GER;
///
///     public IndustrialEffort IndustrialEffort { get; } = new();
///     public MilitaryExpansion MilitaryExpansion { get; } = new();
///     public ResearchPush ResearchPush { get; } = new();
///
///     protected override void Structure(FocusGraph graph)
///     {
///         graph.Root(IndustrialEffort);
///         graph.Root(MilitaryExpansion);
///         graph.Add(ResearchPush).After(IndustrialEffort, MilitaryExpansion);
///     }
/// }
/// </code>
/// </example>
/// </summary>
public abstract class FocusTreeDefinition
{
    /// <summary>Unique identifier for this focus tree.</summary>
    public abstract string Id { get; }

    /// <summary>Primary country this tree is assigned to.</summary>
    public abstract CountryTag Country { get; }

    /// <summary>Priority when assigning to the country (higher = preferred).</summary>
    public virtual int CountryPriority => 10;

    /// <summary>Whether this is the default tree.</summary>
    public virtual bool IsDefault => false;

    /// <summary>Additional country tags that use this tree.</summary>
    public virtual IReadOnlyList<CountryTag> AdditionalCountries => [];

    /// <summary>Whether continuous focus override is enabled.</summary>
    public virtual bool ContinuousFocusOverride => false;

    /// <summary>Initial viewport position for the tree.</summary>
    public virtual FocusPosition? InitialShowPosition => null;

    /// <summary>
    /// Define the tree structure: add focuses and their parent-child relationships.
    /// Positions are calculated automatically.
    /// </summary>
    protected abstract void Structure(FocusGraph graph);

    // ─── Internal: called by ModCompiler ────────────────────────────

    internal FocusGraph CompileGraph()
    {
        var graph = new FocusGraph();
        Structure(graph);
        graph.AutoLayout();
        return graph;
    }
}
