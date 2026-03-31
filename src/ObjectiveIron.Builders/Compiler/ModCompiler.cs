using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;
using ObjectiveIron.Core.Validation;

namespace ObjectiveIron.Builders.Compiler;

/// <summary>
/// Compiles user-defined FocusTreeDefinition classes into Core AST models
/// that can be consumed by the Emitter.
/// 
/// Pipeline: FocusTreeDefinition → FocusTree (Core AST)
/// </summary>
public static class ModCompiler
{
    /// <summary>
    /// Compile a FocusTreeDefinition into a Core FocusTree model.
    /// Runs auto-layout and validation after compilation.
    /// </summary>
    public static CompileResult<FocusTree> Compile(FocusTreeDefinition definition)
    {
        _currentScriptedLocs.Value!.Clear();
        var graph = definition.CompileGraph(); // calls AutoLayout internally

        // Build country filter
        var modifiers = new List<CountryTagModifier>
        {
            new(definition.Country.Value, definition.CountryPriority)
        };
        foreach (var tag in definition.AdditionalCountries)
        {
            modifiers.Add(new CountryTagModifier(tag.Value));
        }

        var country = new CountryFilter
        {
            BaseFactor = 0,
            Modifiers = modifiers.AsReadOnly()
        };

        // Compile each focus node
        var focuses = new List<Focus>();
        foreach (var node in graph.Nodes)
        {
            var focus = CompileFocus(node);
            focuses.Add(focus);
        }

        var tree = new FocusTree
        {
            Id = definition.Id,
            Country = country,
            IsDefault = definition.IsDefault,
            Focuses = focuses.AsReadOnly(),
            ContinuousFocusHasCountryTreeOverride = definition.ContinuousFocusOverride,
            InitialShowPosition = definition.InitialShowPosition is { } pos ? (pos.X, pos.Y) : null
        };

        // Validate
        var validator = new FocusTreeValidator();
        var validation = validator.Validate(tree);

        return new CompileResult<FocusTree>(tree, validation, _currentScriptedLocs.Value);
    }

    private static readonly ThreadLocal<List<ScriptedLocalisation>> _currentScriptedLocs = new(() => []);

    public static IReadOnlyList<ScriptedLocalisation> GetScriptedLocalisations() => _currentScriptedLocs.Value!.AsReadOnly();
    public static void ClearScriptedLocalisations() => _currentScriptedLocs.Value!.Clear();

    private static Focus CompileFocus(FocusNode node)
    {
        var def = node.Focus;
        var scriptedLocs = _currentScriptedLocs.Value!;

        // Position priority: manual override > auto-layout > focus default
        var position = node.ManualPosition ?? node.AutoPosition ?? def.Position;

        // Build prerequisite groups (OR within group, AND between groups)
        var prerequisites = node.PrerequisiteGroups
            .Select(group =>
                (IReadOnlyList<string>)group
                    .Select(f => f.Id)
                    .ToList()
                    .AsReadOnly())
            .ToList()
            .AsReadOnly();

        var exclusives = node.Exclusives
            .Select(f => f.Id)
            .ToList()
            .AsReadOnly();

        bool isDynamic = false;

        // Dynamic Icons
        var dynamicIcons = new List<DynamicIcon>();
        foreach (var (trigger, icon) in def.CompileDynamicIcon())
        {
            Block? block = null;
            if (trigger != null)
            {
                var scope = new TriggerScope();
                trigger(scope);
                var b = scope.Build();
                if (b.Entries.Count > 0) block = b;
            }
            dynamicIcons.Add(new DynamicIcon { Trigger = block, Value = icon.Value });
        }
        if (dynamicIcons.Count > 0) isDynamic = true;

        // Dynamic Title -> Scripted Localisation
        var dynamicTitle = def.CompileDynamicTitle();
        if (dynamicTitle.Count > 0)
        {
            isDynamic = true;
            scriptedLocs.Add(CompileScriptedLoc($"{def.Id}_Title", $"{def.Id}_title", dynamicTitle));
        }

        // Dynamic Description -> Scripted Localisation
        var dynamicDesc = def.CompileDynamicDescription();
        if (dynamicDesc.Count > 0)
        {
            isDynamic = true;
            scriptedLocs.Add(CompileScriptedLoc($"{def.Id}_Desc", $"{def.Id}_desc", dynamicDesc));
        }

        return new Focus
        {
            Id = def.Id,
            Icon = def.Icon?.Value,
            X = position.X,
            Y = position.Y,
            Cost = def.Cost,
            Prerequisites = prerequisites,
            MutuallyExclusive = exclusives,
            Available = def.CompileAvailable(),
            Bypass = def.CompileBypass(),
            CompletionReward = def.CompileCompletionReward(),
            SelectEffect = def.CompileSelectEffect(),
            CancelEffect = def.CompileCancelEffect(),
            AiWillDo = def.CompileAiWillDo(),
            CancelIfInvalid = def.CancelIfInvalid,
            AvailableIfCapitulated = def.AvailableIfCapitulated,
            SearchFilters = def.SearchFilters.ToList().AsReadOnly(),
            Dynamic = isDynamic,
            DynamicIcons = dynamicIcons.AsReadOnly()
        };
    }

    public static Character CompileCharacter(CharacterDefinition def)
    {
        var portraits = new CharacterPortraits(
            ArmyLarge: def.PortraitLarge,
            ArmySmall: def.PortraitSmall,
            CivilianLarge: def.PortraitLarge,
            CivilianSmall: def.PortraitSmall
        );
        return new Character(def.Id, def.Name, def.CompileRoles(), portraits);
    }

    public static Idea CompileIdea(IdeaDefinition def)
    {
        var scriptedLocs = _currentScriptedLocs.Value!;
        string? dynamicName = null;
        string? dynamicDesc = null;

        var dynTitle = def.CompileDynamicName();
        if (dynTitle.Count > 0)
        {
            var loc = CompileScriptedLoc($"{def.Id}_Title", $"idea_{def.Id}_title", dynTitle);
            scriptedLocs.Add(loc);
            dynamicName = loc.Name;
        }

        var dynDesc = def.CompileDynamicDescription();
        if (dynDesc.Count > 0)
        {
            var loc = CompileScriptedLoc($"{def.Id}_Desc", $"idea_{def.Id}_desc", dynDesc);
            scriptedLocs.Add(loc);
            dynamicDesc = loc.Name;
        }

        return new Idea
        {
            Id = def.Id,
            Name = def.Name,
            Description = def.Description,
            Picture = def.Picture,
            Category = def.Category,
            Modifier = def.CompileModifier(),
            DynamicName = dynamicName,
            DynamicDescription = dynamicDesc
        };
    }

    public static Decision CompileDecision(DecisionDefinition def)
    {
        var scriptedLocs = _currentScriptedLocs.Value!;
        string? dynamicName = null;
        string? dynamicDesc = null;

        var dynTitle = def.CompileDynamicName();
        if (dynTitle.Count > 0)
        {
            var loc = CompileScriptedLoc($"{def.Id}_Title", $"decision_{def.Id}_title", dynTitle);
            scriptedLocs.Add(loc);
            dynamicName = loc.Name;
        }

        var dynDesc = def.CompileDynamicDescription();
        if (dynDesc.Count > 0)
        {
            var loc = CompileScriptedLoc($"{def.Id}_Desc", $"decision_{def.Id}_desc", dynDesc);
            scriptedLocs.Add(loc);
            dynamicDesc = loc.Name;
        }

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
            TimeoutEffect = def.CompileTimeout(),
            DynamicName = dynamicName,
            DynamicDescription = dynamicDesc
        };
    }

    private static ScriptedLocalisation CompileScriptedLoc(string macroName, string keyPrefix, IReadOnlyList<(Action<TriggerScope>?, LocalizedText)> entries)
    {
        var texts = new List<ScriptedLocalisationText>();
        int index = 0;
        
        foreach (var (trigger, _) in entries)
        {
            Block? block = null;
            if (trigger != null)
            {
                var scope = new TriggerScope();
                trigger(scope);
                var b = scope.Build();
                if (b.Entries.Count > 0) block = b;
            }

            string locKey = trigger == null ? $"{keyPrefix}_default" : $"{keyPrefix}_{index++}";
            texts.Add(new ScriptedLocalisationText { Trigger = block, LocalizationKey = locKey });
        }

        return new ScriptedLocalisation
        {
            Name = $"Get_{macroName}",
            Texts = texts.AsReadOnly()
        };
    }
}

/// <summary>
/// Result of a compilation, containing the model and validation result.
/// </summary>
public sealed class CompileResult<T>
{
    public T Model { get; }
    public ValidationResult Validation { get; }
    public IReadOnlyList<ScriptedLocalisation> ScriptedLocalisations { get; }
    public bool IsValid => Validation.IsValid;

    internal CompileResult(T model, ValidationResult validation, List<ScriptedLocalisation>? scriptedLocs = null)
    {
        Model = model;
        Validation = validation;
        ScriptedLocalisations = scriptedLocs?.AsReadOnly() ?? new List<ScriptedLocalisation>().AsReadOnly();
    }
}
