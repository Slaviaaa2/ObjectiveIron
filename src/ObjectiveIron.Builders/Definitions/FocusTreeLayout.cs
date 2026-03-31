using ObjectiveIron.Builders.Types;

namespace ObjectiveIron.Builders.Definitions;

/// <summary>
/// Defines the structure of a focus tree using simple parent-child relationships.
/// Positioning is handled automatically based on the tree graph.
/// 
/// Usage in FocusTreeDefinition:
/// <code>
/// protected override void Structure(FocusGraph graph)
/// {
///     graph.Root(IndustrialEffort);
///     graph.Root(MilitaryExpansion);
///
///     graph.Add(ResearchPush).After(IndustrialEffort, MilitaryExpansion);
///     graph.Add(PoliticalManeuvering).After(IndustrialEffort);
///     graph.Add(WarPreparation).After(MilitaryExpansion)
///         .ExclusiveWith(PoliticalManeuvering);
///     graph.Add(UltimateDecision)
///         .After(ResearchPush)
///         .After(PoliticalManeuvering, WarPreparation);
/// }
/// </code>
/// </summary>
public sealed class FocusGraph
{
    private readonly List<FocusNode> _nodes = [];

    /// <summary>All registered focus nodes.</summary>
    internal IReadOnlyList<FocusNode> Nodes => _nodes.AsReadOnly();

    /// <summary>
    /// Add a root focus (top of the tree, no prerequisites).
    /// Multiple roots are placed side-by-side at the top.
    /// </summary>
    public FocusNode Root(FocusDefinition focus)
    {
        var node = new FocusNode(focus) { IsRoot = true };
        _nodes.Add(node);
        return node;
    }

    /// <summary>
    /// Add a focus to the tree. Chain .After() to define its parent(s).
    /// Position is calculated automatically.
    /// </summary>
    public FocusNode Add(FocusDefinition focus)
    {
        var node = new FocusNode(focus);
        _nodes.Add(node);
        return node;
    }

    // ─── Auto-Layout Algorithm ───────────────────────────────────

    /// <summary>
    /// Calculate X/Y positions for all nodes based on the tree graph.
    /// - Roots are placed at Y=0, spread evenly.
    /// - Children are placed at Y = max(parent Y) + 1.
    /// - X is centered under parents, with collision resolution.
    /// </summary>
    internal void AutoLayout(int spacing = 2)
    {
        if (_nodes.Count == 0) return;

        // Build parent lookup: focus id → list of parent focus ids
        var childToParents = new Dictionary<string, List<string>>();
        var idToNode = new Dictionary<string, FocusNode>();

        foreach (var node in _nodes)
        {
            idToNode[node.Focus.Id] = node;
            childToParents[node.Focus.Id] = [];
        }

        foreach (var node in _nodes)
        {
            foreach (var group in node.PrerequisiteGroups)
            {
                foreach (var parent in group)
                {
                    if (!childToParents.ContainsKey(node.Focus.Id))
                        childToParents[node.Focus.Id] = [];
                    childToParents[node.Focus.Id].Add(parent.Id);
                }
            }
        }

        // 1. Assign Y levels using topological order
        var yLevels = new Dictionary<string, int>();

        // Roots get Y=0
        foreach (var node in _nodes.Where(n => n.IsRoot || n.PrerequisiteGroups.Count == 0))
        {
            yLevels[node.Focus.Id] = 0;
        }

        // BFS to assign Y levels
        var changed = true;
        while (changed)
        {
            changed = false;
            foreach (var node in _nodes)
            {
                if (yLevels.ContainsKey(node.Focus.Id)) continue;

                var parents = childToParents[node.Focus.Id];
                if (parents.Count == 0)
                {
                    yLevels[node.Focus.Id] = 0;
                    changed = true;
                    continue;
                }

                // All parents must have Y assigned
                if (parents.All(p => yLevels.ContainsKey(p)))
                {
                    yLevels[node.Focus.Id] = parents.Max(p => yLevels[p]) + 1;
                    changed = true;
                }
            }
        }

        // Fallback for any unresolved nodes
        foreach (var node in _nodes)
        {
            if (!yLevels.ContainsKey(node.Focus.Id))
                yLevels[node.Focus.Id] = 0;
        }

        // 2. Group nodes by Y level
        var levels = _nodes
            .GroupBy(n => yLevels[n.Focus.Id])
            .OrderBy(g => g.Key)
            .ToList();

        // 3. Assign X positions
        var xPositions = new Dictionary<string, int>();

        // First pass: place roots evenly
        var rootLevel = levels.FirstOrDefault(l => l.Key == 0);
        if (rootLevel != null)
        {
            var rootNodes = rootLevel.ToList();
            for (int i = 0; i < rootNodes.Count; i++)
            {
                xPositions[rootNodes[i].Focus.Id] = i * spacing;
            }
        }

        // Subsequent levels: center children under parents
        foreach (var level in levels.Where(l => l.Key > 0))
        {
            var levelNodes = level.ToList();

            // Sort by average parent X position for better layout
            levelNodes.Sort((a, b) =>
            {
                var aParentX = GetAverageParentX(a, childToParents, xPositions);
                var bParentX = GetAverageParentX(b, childToParents, xPositions);
                return aParentX.CompareTo(bParentX);
            });

            for (int i = 0; i < levelNodes.Count; i++)
            {
                var node = levelNodes[i];
                var parentX = GetAverageParentX(node, childToParents, xPositions);

                // Try to place at parent center
                var x = (int)Math.Round(parentX);

                // Resolve collisions
                while (xPositions.Values.Any(existingX =>
                    Math.Abs(existingX - x) < spacing &&
                    _nodes.Any(n => xPositions.ContainsKey(n.Focus.Id) &&
                                    xPositions[n.Focus.Id] == x &&
                                    yLevels[n.Focus.Id] == level.Key)))
                {
                    x += spacing;
                }

                xPositions[node.Focus.Id] = x;
            }
        }

        // 4. Normalize X positions (shift so minimum is 0)
        var minX = xPositions.Values.DefaultIfEmpty(0).Min();
        foreach (var node in _nodes)
        {
            var x = xPositions.GetValueOrDefault(node.Focus.Id, 0) - minX;
            var y = yLevels.GetValueOrDefault(node.Focus.Id, 0);
            node.AutoPosition = new FocusPosition(x, y);
        }
    }

    private static double GetAverageParentX(FocusNode node,
        Dictionary<string, List<string>> childToParents,
        Dictionary<string, int> xPositions)
    {
        var parents = childToParents.GetValueOrDefault(node.Focus.Id, []);
        if (parents.Count == 0) return 0;

        var parentXValues = parents
            .Where(p => xPositions.ContainsKey(p))
            .Select(p => xPositions[p])
            .ToList();

        return parentXValues.Count > 0 ? parentXValues.Average() : 0;
    }
}

/// <summary>
/// A focus in the tree graph with its relationships.
/// </summary>
public sealed class FocusNode
{
    internal FocusDefinition Focus { get; }
    internal bool IsRoot { get; set; }

    /// <summary>Prerequisite OR-groups (ANDed between groups).</summary>
    internal List<List<FocusDefinition>> PrerequisiteGroups { get; } = [];

    /// <summary>Mutually exclusive focuses.</summary>
    internal List<FocusDefinition> Exclusives { get; } = [];

    /// <summary>Auto-calculated position (set by AutoLayout).</summary>
    internal FocusPosition? AutoPosition { get; set; }

    /// <summary>Manual position override.</summary>
    internal FocusPosition? ManualPosition { get; private set; }

    internal FocusNode(FocusDefinition focus)
    {
        Focus = focus;
    }

    /// <summary>
    /// This focus comes after (requires) the given focuses.
    /// Multiple focuses in one call = OR (any one satisfies).
    /// Multiple After() calls = AND (all groups must be satisfied).
    /// 
    /// Example:
    ///   .After(A, B)     → need A OR B
    ///   .After(C)         → AND need C
    ///   Result: (A OR B) AND C
    /// </summary>
    public FocusNode After(params FocusDefinition[] parents)
    {
        PrerequisiteGroups.Add([..parents]);
        return this;
    }

    /// <summary>
    /// Mark as mutually exclusive with the given focuses.
    /// Only one of the exclusive group can be completed.
    /// </summary>
    public FocusNode ExclusiveWith(params FocusDefinition[] focuses)
    {
        Exclusives.AddRange(focuses);
        return this;
    }

    /// <summary>
    /// Manually override the auto-calculated position.
    /// Only use this if auto-layout doesn't place it where you want.
    /// </summary>
    public FocusNode At(int x, int y)
    {
        ManualPosition = new FocusPosition(x, y);
        return this;
    }
}
