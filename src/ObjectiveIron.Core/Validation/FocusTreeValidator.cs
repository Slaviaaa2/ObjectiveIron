using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;
using ObjectiveIron.Core.Data;

namespace ObjectiveIron.Core.Validation;

/// <summary>
/// Validates a FocusTree definition for structural correctness.
/// Checks: required fields, duplicate IDs, prerequisite references, circular dependencies.
/// </summary>
public sealed class FocusTreeValidator : IValidator<FocusTree>
{
    private readonly Hoi4DataService? _dataService;

    public FocusTreeValidator(Hoi4DataService? dataService = null)
    {
        _dataService = dataService;
    }

    public ValidationResult Validate(FocusTree tree)
    {
        var result = ValidationResult.Success();

        // 1. Tree-level validation
        if (string.IsNullOrWhiteSpace(tree.Id))
            result = result.AddError("Focus tree ID is required.", "FocusTree");

        if (tree.Country.Modifiers.Count == 0)
            result = result.AddWarning("No country modifiers defined. Tree may not be assigned to any country.", tree.Id);

        if (tree.Focuses.Count == 0)
            result = result.AddWarning("Focus tree has no focuses.", tree.Id);

        // 2. Focus-level validation
        var focusIds = new HashSet<string>();
        var duplicates = new HashSet<string>();

        foreach (var focus in tree.Focuses)
        {
            if (string.IsNullOrWhiteSpace(focus.Id))
            {
                result = result.AddError("Focus ID is required.", tree.Id);
                continue;
            }

            if (!focusIds.Add(focus.Id))
            {
                duplicates.Add(focus.Id);
            }

            if (focus.Cost <= 0)
                result = result.AddWarning($"Focus cost is {focus.Cost} (should be positive).", focus.Id);

            if (_dataService != null)
            {
                ValidateBlock(focus.CompletionReward, focus.Id, _dataService, ref result);
            }
        }

        foreach (var dup in duplicates)
            result = result.AddError($"Duplicate focus ID: '{dup}'", tree.Id);

        // 3. Prerequisite reference validation
        foreach (var focus in tree.Focuses)
        {
            foreach (var group in focus.Prerequisites)
            {
                foreach (var prereqId in group)
                {
                    if (!focusIds.Contains(prereqId))
                        result = result.AddError($"Prerequisite '{prereqId}' does not exist in the focus tree.", focus.Id);
                }
            }

            foreach (var exclusiveId in focus.MutuallyExclusive)
            {
                if (!focusIds.Contains(exclusiveId))
                    result = result.AddError($"Mutually exclusive focus '{exclusiveId}' does not exist in the focus tree.", focus.Id);
            }

            if (focus.RelativePositionId != null && !focusIds.Contains(focus.RelativePositionId))
                result = result.AddError($"Relative position focus '{focus.RelativePositionId}' does not exist.", focus.Id);
        }

        // 4. Circular dependency detection
        result = DetectCycles(tree, focusIds, result);

        return result;
    }

    private static ValidationResult DetectCycles(FocusTree tree, HashSet<string> allIds, ValidationResult result)
    {
        var prereqMap = new Dictionary<string, List<string>>();
        foreach (var focus in tree.Focuses)
        {
            var allPrereqs = new List<string>();
            foreach (var group in focus.Prerequisites)
                allPrereqs.AddRange(group);
            prereqMap[focus.Id] = allPrereqs;
        }

        // DFS-based cycle detection
        var visited = new HashSet<string>();
        var inStack = new HashSet<string>();

        foreach (var id in allIds)
        {
            if (!visited.Contains(id))
            {
                if (HasCycle(id, prereqMap, visited, inStack, out var cyclePath))
                {
                    result = result.AddError(
                        $"Circular dependency detected: {string.Join(" → ", cyclePath)}",
                        tree.Id);
                }
            }
        }

        return result;
    }

    private static bool HasCycle(string node, Dictionary<string, List<string>> graph,
        HashSet<string> visited, HashSet<string> inStack, out List<string> cyclePath)
    {
        cyclePath = [];
        visited.Add(node);
        inStack.Add(node);

        if (graph.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    if (HasCycle(neighbor, graph, visited, inStack, out cyclePath))
                    {
                        cyclePath.Insert(0, node);
                        return true;
                    }
                }
                else if (inStack.Contains(neighbor))
                {
                    cyclePath = [node, neighbor];
                    return true;
                }
            }
        }

        inStack.Remove(node);
        return false;
    }

    private static void ValidateBlock(Block block, string focusId, Hoi4DataService dataService, ref ValidationResult result)
    {
        foreach (var entry in block.Entries)
        {
            if (entry is BlockEntry.PropertyEntry prop)
            {
                if (prop.Property.Key == "add_ideas")
                {
                    // validation for ideas if we had them.
                }
            }
            else if (entry is BlockEntry.NestedBlock nested)
            {
                if (nested.Name == "add_building_construction")
                {
                    var buildingFound = false;
                    foreach (var bProp in nested.Block.Entries)
                    {
                        if (bProp is BlockEntry.PropertyEntry bp && bp.Property.Key == "type")
                        {
                            var bType = bp.Property.Value.ToClausewitz();
                            if (!dataService.GetBuildings().Contains(bType))
                            {
                                result = result.AddWarning($"Invalid building type '{bType}' in {focusId}.", focusId);
                            }
                            buildingFound = true;
                        }
                    }
                    if (!buildingFound)
                        result = result.AddWarning($"add_building_construction in {focusId} missing 'type' property.", focusId);
                }
                
                // Recursively validate nested blocks
                ValidateBlock(nested.Block, focusId, dataService, ref result);
            }
        }
    }
}
