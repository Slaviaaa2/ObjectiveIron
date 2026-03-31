using ObjectiveIron.Builders.Compiler;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Validation;
using ObjectiveIron.Core.Data;
using ObjectiveIron.Emitter.Emitters;

namespace ObjectiveIron.Emitter;

/// <summary>
/// Represents a complete HOI4 mod project.
/// Manages all mod content and emits files to the correct directory structure.
/// 
/// Usage:
/// <code>
/// var project = new ModProject("MyMod", "output/MyMod");
/// project.AddFocusTree(tree);
/// var result = project.Emit();
/// </code>
/// </summary>
public sealed class ModProject
{
    private readonly List<FocusTree> _focusTrees = [];
    private readonly List<FocusTreeDefinition> _focusTreeDefinitions = [];
    private readonly List<FocusDefinition> _focusDefinitions = [];
    private readonly List<EventDefinition> _eventDefinitions = [];
    private readonly List<DecisionCategoryDefinition> _decisionCategoryDefinitions = [];
    private readonly List<DecisionDefinition> _decisionDefinitions = [];
    private readonly List<ScriptedTriggerDefinition> _scriptedTriggerDefinitions = [];
    private readonly List<ScriptedEffectDefinition> _scriptedEffectDefinitions = [];
    private readonly List<SpriteDefinition> _spriteDefinitions = [];
    private readonly List<IdeaDefinition> _ideaDefinitions = [];
    private readonly List<CharacterDefinition> _characterDefinitions = [];
    private readonly List<TechnologyDefinition> _technologyDefinitions = [];
    private readonly List<OnActionDefinition> _onActionDefinitions = [];
    private readonly List<OpinionModifierDefinition> _opinionModifierDefinitions = [];
    private readonly List<DynamicModifierDefinition> _dynamicModifierDefinitions = [];
    private readonly List<CountryDefinition> _countryDefinitions = [];
    private readonly List<CountryHistoryDefinition> _countryHistoryDefinitions = [];
    private readonly List<StateDefinition> _stateDefinitions = [];
    private readonly List<OobDefinition> _oobDefinitions = [];
    private readonly List<NavalOobDefinition> _navalOobDefinitions = [];
    private readonly List<AirOobDefinition> _airOobDefinitions = [];
    private readonly List<SubUnitDefinition> _subUnitDefinitions = [];
    private readonly List<EquipmentDefinition> _equipmentDefinitions = [];
    private readonly List<ContinuousFocusDefinition> _continuousFocusDefinitions = [];
    private readonly List<BookmarkDefinition> _bookmarkDefinitions = [];
    private readonly List<AiStrategyDefinition> _aiStrategyDefinitions = [];
    private readonly List<CountryLeaderTraitDefinition> _countryLeaderTraitDefinitions = [];
    private readonly List<UnitLeaderTraitDefinition> _unitLeaderTraitDefinitions = [];
    private readonly List<OccupationLawDefinition> _occupationLawDefinitions = [];
    private readonly List<WargoalDefinition> _wargoalDefinitions = [];
    private readonly List<OperationDefinition> _operationDefinitions = [];
    private readonly List<AbilityDefinition> _abilityDefinitions = [];
    private readonly List<BuildingDefinition> _buildingDefinitions = [];
    private readonly List<StaticModifierDefinition> _staticModifierDefinitions = [];
    private readonly List<GameRuleDefinition> _gameRuleDefinitions = [];
    private readonly List<TerrainDefinition> _terrainDefinitions = [];
    private readonly List<StateCategoryDefinition> _stateCategoryDefinitions = [];
    private readonly List<ResourceDefinition> _resourceDefinitions = [];
    private readonly List<TechSharingDefinition> _techSharingDefinitions = [];
    private readonly List<IdeologyDefinition> _ideologyDefinitions = [];
    private readonly List<AutonomyStateDefinition> _autonomyStateDefinitions = [];
    private readonly List<DifficultySettingDefinition> _difficultyDefinitions = [];
    private readonly List<NameDefinition> _nameDefinitions = [];
    private readonly List<MioDefinition> _mioDefinitions = [];
    private readonly List<IntelligenceAgencyDefinition> _intelAgencyDefinitions = [];
    private readonly List<PeaceConferenceDefinition> _peaceConferenceDefinitions = [];
    private readonly List<BalanceOfPowerDefinition> _bopDefinitions = [];
    private readonly List<ScriptedDiplomaticActionDefinition> _scriptedDiploDefinitions = [];
    private readonly List<ScriptedGuiDefinition> _scriptedGuiDefinitions = [];
    private readonly List<GuiDefinition> _guiDefinitions = [];
    private readonly List<MusicDefinition> _musicDefinitions = [];
    private readonly List<SoundDefinition> _soundDefinitions = [];
    private readonly List<StrategicRegionDefinition> _strategicRegionDefinitions = [];
    private readonly List<SupplyAreaDefinition> _supplyAreaDefinitions = [];
    private readonly List<LocalisationReplaceDefinition> _localisationReplaceDefinitions = [];
    private readonly List<RawAsset> _rawAssets = [];
    private readonly List<ProvinceDefinition> _provinceDefinitions = [];
    private readonly List<AdjacencyDefinition> _adjacencyDefinitions = [];
    private DefaultMapDefinition? _defaultMapDefinition;
    private readonly List<ScriptedLocalisation> _scriptedLocalisations = [];
    private Hoi4DataService? _dataService;

    /// <summary>Display name for this mod.</summary>
    public string Name { get; set; }

    /// <summary>Root output directory for generated files.</summary>
    public string OutputPath { get; set; }

    /// <summary>Mod version string.</summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>Supported game version string (e.g., "1.14.*").</summary>
    public string SupportedGameVersion { get; set; } = "1.14.*";

    /// <summary>Tags for the mod (e.g., "Alternative History", "Gameplay").</summary>
    public List<string> Tags { get; set; } = [];

    /// <summary>Path to the Hearts of Iron IV installation directory for data extraction.</summary>
    public string? GamePath { get; set; }

    public ModProject(string name, string outputPath)
    {
        Name = name;
        OutputPath = outputPath;
    }

    /// <summary>Add a focus tree to this mod.</summary>
    public ModProject AddFocusTree(FocusTree tree)
    {
        _focusTrees.Add(tree);
        return this;
    }

    /// <summary>Add a focus tree definition to be compiled and emitted.</summary>
    public ModProject AddFocusTrees(params FocusTreeDefinition[] trees)
    {
        _focusTreeDefinitions.AddRange(trees);
        return this;
    }

    /// <summary>
    /// Add focus definitions for localisation generation.
    /// Call this with the FocusDefinition instances from your FocusTreeDefinition.
    /// </summary>
    public ModProject AddFocusDefinitions(params FocusDefinition[] focuses)
    {
        _focusDefinitions.AddRange(focuses);
        return this;
    }

    /// <summary>
    /// Add compiled scripted localisations for generation.
    /// </summary>
    public ModProject AddScriptedLocalisations(IEnumerable<ScriptedLocalisation> locs)
    {
        _scriptedLocalisations.AddRange(locs);
        return this;
    }

    /// <summary>
    /// Add events to be compiled and emitted.
    /// </summary>
    public ModProject AddEvents(params EventDefinition[] events)
    {
        _eventDefinitions.AddRange(events);
        return this;
    }

    /// <summary>
    /// Add decision category definitions for emission and localisation.
    /// </summary>
    public ModProject AddDecisionCategories(params DecisionCategoryDefinition[] categories)
    {
        _decisionCategoryDefinitions.AddRange(categories);
        return this;
    }

    /// <summary>
    /// Add decision definitions for emission and localisation.
    /// </summary>
    public ModProject AddDecisions(params DecisionDefinition[] decisions)
    {
        _decisionDefinitions.AddRange(decisions);
        return this;
    }

    /// <summary>
    /// Add scripted trigger definitions.
    /// </summary>
    public ModProject AddScriptedTriggers(params ScriptedTriggerDefinition[] triggers)
    {
        _scriptedTriggerDefinitions.AddRange(triggers);
        return this;
    }

    /// <summary>
    /// Add scripted effect definitions.
    /// </summary>
    public ModProject AddScriptedEffects(params ScriptedEffectDefinition[] effects)
    {
        _scriptedEffectDefinitions.AddRange(effects);
        return this;
    }

    /// <summary>
    /// Add sprite definitions for .gfx generation.
    /// </summary>
    public ModProject AddSprites(params SpriteDefinition[] sprites)
    {
        _spriteDefinitions.AddRange(sprites);
        return this;
    }

    /// <summary>
    /// Add idea definitions for National Spirits.
    /// </summary>
    public ModProject AddIdeas(params IdeaDefinition[] ideas)
    {
        _ideaDefinitions.AddRange(ideas);
        return this;
    }

    /// <summary>
    /// Add character definitions (v1.11+).
    /// </summary>
    public ModProject AddCharacters(params CharacterDefinition[] characters)
    {
        _characterDefinitions.AddRange(characters);
        return this;
    }

    /// <summary>
    /// Add technology definitions.
    /// </summary>
    public ModProject AddTechnologies(params TechnologyDefinition[] technologies)
    {
        _technologyDefinitions.AddRange(technologies);
        return this;
    }

    /// <summary>Add on_action definitions.</summary>
    public ModProject AddOnActions(params OnActionDefinition[] onActions)
    {
        _onActionDefinitions.AddRange(onActions);
        return this;
    }

    /// <summary>Add opinion modifier definitions.</summary>
    public ModProject AddOpinionModifiers(params OpinionModifierDefinition[] modifiers)
    {
        _opinionModifierDefinitions.AddRange(modifiers);
        return this;
    }

    /// <summary>Add dynamic modifier definitions.</summary>
    public ModProject AddDynamicModifiers(params DynamicModifierDefinition[] modifiers)
    {
        _dynamicModifierDefinitions.AddRange(modifiers);
        return this;
    }

    /// <summary>Add country definitions (tags + country files).</summary>
    public ModProject AddCountries(params CountryDefinition[] countries)
    {
        _countryDefinitions.AddRange(countries);
        return this;
    }

    /// <summary>Add country history definitions.</summary>
    public ModProject AddCountryHistories(params CountryHistoryDefinition[] histories)
    {
        _countryHistoryDefinitions.AddRange(histories);
        return this;
    }

    /// <summary>Add state definitions.</summary>
    public ModProject AddStates(params StateDefinition[] states)
    {
        _stateDefinitions.AddRange(states);
        return this;
    }

    /// <summary>Add order of battle definitions.</summary>
    public ModProject AddOobs(params OobDefinition[] oobs)
    {
        _oobDefinitions.AddRange(oobs);
        return this;
    }

    public ModProject AddNavalOobs(params NavalOobDefinition[] oobs)
    {
        _navalOobDefinitions.AddRange(oobs);
        return this;
    }

    public ModProject AddAirOobs(params AirOobDefinition[] oobs)
    {
        _airOobDefinitions.AddRange(oobs);
        return this;
    }

    /// <summary>Add sub-unit (battalion/company) definitions.</summary>
    public ModProject AddSubUnits(params SubUnitDefinition[] subUnits)
    {
        _subUnitDefinitions.AddRange(subUnits);
        return this;
    }

    /// <summary>Add equipment definitions.</summary>
    public ModProject AddEquipment(params EquipmentDefinition[] equipment)
    {
        _equipmentDefinitions.AddRange(equipment);
        return this;
    }

    /// <summary>Add continuous focus palette definitions.</summary>
    public ModProject AddContinuousFocuses(params ContinuousFocusDefinition[] palettes)
    {
        _continuousFocusDefinitions.AddRange(palettes);
        return this;
    }

    /// <summary>Add bookmark definitions.</summary>
    public ModProject AddBookmarks(params BookmarkDefinition[] bookmarks)
    {
        _bookmarkDefinitions.AddRange(bookmarks);
        return this;
    }

    /// <summary>Add AI strategy definitions.</summary>
    public ModProject AddAiStrategies(params AiStrategyDefinition[] strategies)
    {
        _aiStrategyDefinitions.AddRange(strategies);
        return this;
    }

    /// <summary>Add country leader trait definitions.</summary>
    public ModProject AddCountryLeaderTraits(params CountryLeaderTraitDefinition[] traits)
    {
        _countryLeaderTraitDefinitions.AddRange(traits);
        return this;
    }

    /// <summary>Add unit leader trait definitions.</summary>
    public ModProject AddUnitLeaderTraits(params UnitLeaderTraitDefinition[] traits)
    {
        _unitLeaderTraitDefinitions.AddRange(traits);
        return this;
    }

    /// <summary>Add occupation law definitions.</summary>
    public ModProject AddOccupationLaws(params OccupationLawDefinition[] laws)
    {
        _occupationLawDefinitions.AddRange(laws);
        return this;
    }

    /// <summary>Add wargoal definitions.</summary>
    public ModProject AddWargoals(params WargoalDefinition[] wargoals)
    {
        _wargoalDefinitions.AddRange(wargoals);
        return this;
    }

    /// <summary>Add operation definitions.</summary>
    public ModProject AddOperations(params OperationDefinition[] operations)
    {
        _operationDefinitions.AddRange(operations);
        return this;
    }

    /// <summary>Add ability definitions.</summary>
    public ModProject AddAbilities(params AbilityDefinition[] abilities)
    {
        _abilityDefinitions.AddRange(abilities);
        return this;
    }

    public ModProject AddBuildings(params BuildingDefinition[] buildings) { _buildingDefinitions.AddRange(buildings); return this; }
    public ModProject AddStaticModifiers(params StaticModifierDefinition[] mods) { _staticModifierDefinitions.AddRange(mods); return this; }
    public ModProject AddGameRules(params GameRuleDefinition[] rules) { _gameRuleDefinitions.AddRange(rules); return this; }
    public ModProject AddTerrains(params TerrainDefinition[] terrains) { _terrainDefinitions.AddRange(terrains); return this; }
    public ModProject AddStateCategories(params StateCategoryDefinition[] cats) { _stateCategoryDefinitions.AddRange(cats); return this; }
    public ModProject AddResources(params ResourceDefinition[] resources) { _resourceDefinitions.AddRange(resources); return this; }
    public ModProject AddTechSharing(params TechSharingDefinition[] groups) { _techSharingDefinitions.AddRange(groups); return this; }
    public ModProject AddIdeologies(params IdeologyDefinition[] ideologies) { _ideologyDefinitions.AddRange(ideologies); return this; }
    public ModProject AddAutonomyStates(params AutonomyStateDefinition[] states) { _autonomyStateDefinitions.AddRange(states); return this; }
    public ModProject AddDifficulty(params DifficultySettingDefinition[] settings) { _difficultyDefinitions.AddRange(settings); return this; }
    public ModProject AddNames(params NameDefinition[] names) { _nameDefinitions.AddRange(names); return this; }
    public ModProject AddMios(params MioDefinition[] mios) { _mioDefinitions.AddRange(mios); return this; }
    public ModProject AddIntelligenceAgencies(params IntelligenceAgencyDefinition[] agencies) { _intelAgencyDefinitions.AddRange(agencies); return this; }
    public ModProject AddPeaceConference(params PeaceConferenceDefinition[] actions) { _peaceConferenceDefinitions.AddRange(actions); return this; }
    public ModProject AddBalanceOfPower(params BalanceOfPowerDefinition[] bops) { _bopDefinitions.AddRange(bops); return this; }
    public ModProject AddScriptedDiplomaticActions(params ScriptedDiplomaticActionDefinition[] actions) { _scriptedDiploDefinitions.AddRange(actions); return this; }
    public ModProject AddScriptedGuis(params ScriptedGuiDefinition[] guis) { _scriptedGuiDefinitions.AddRange(guis); return this; }
    public ModProject AddGuis(params GuiDefinition[] guis) { _guiDefinitions.AddRange(guis); return this; }
    public ModProject AddMusic(params MusicDefinition[] music) { _musicDefinitions.AddRange(music); return this; }
    public ModProject AddSounds(params SoundDefinition[] sounds) { _soundDefinitions.AddRange(sounds); return this; }
    public ModProject AddStrategicRegions(params StrategicRegionDefinition[] regions) { _strategicRegionDefinitions.AddRange(regions); return this; }
    public ModProject AddSupplyAreas(params SupplyAreaDefinition[] areas) { _supplyAreaDefinitions.AddRange(areas); return this; }
    public ModProject AddLocalisationReplace(params LocalisationReplaceDefinition[] defs) { _localisationReplaceDefinitions.AddRange(defs); return this; }

    /// <summary>
    /// Add raw/static assets to be copied into the mod output.
    /// The relativePath is relative to the mod root (e.g., "map/provinces.bmp", "gfx/leaders/my_leader.dds").
    /// </summary>
    public ModProject AddRawAssets(params RawAsset[] assets) { _rawAssets.AddRange(assets); return this; }

    /// <summary>Add a raw asset from a source file path and a target relative path in the mod output.</summary>
    public ModProject AddRawAsset(string sourcePath, string targetRelativePath)
    {
        _rawAssets.Add(new RawAsset(sourcePath, targetRelativePath));
        return this;
    }

    /// <summary>Add all files from a source directory, preserving folder structure relative to the directory root.</summary>
    public ModProject AddRawAssetDirectory(string sourceDir, string targetRelativeDir = "")
    {
        if (!Directory.Exists(sourceDir)) return this;
        foreach (var file in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(sourceDir, file);
            var target = string.IsNullOrEmpty(targetRelativeDir) ? relative : Path.Combine(targetRelativeDir, relative);
            _rawAssets.Add(new RawAsset(file, target));
        }
        return this;
    }

    public ModProject AddProvinceDefinitions(params ProvinceDefinition[] provinces) { _provinceDefinitions.AddRange(provinces); return this; }
    public ModProject AddAdjacencies(params AdjacencyDefinition[] adjacencies) { _adjacencyDefinitions.AddRange(adjacencies); return this; }
    public ModProject SetDefaultMap(DefaultMapDefinition def) { _defaultMapDefinition = def; return this; }

    /// <summary>
    /// Validate all content and emit files to the output directory.
    /// </summary>
    public EmitResult Emit()
    {
        var result = new EmitResult();

        // Validate all content
        if (!string.IsNullOrEmpty(GamePath))
        {
            _dataService = new Hoi4DataService(GamePath);
        }

        ModCompiler.ClearScriptedLocalisations();

        // Compile Focus Tree Definitions
        var allFocusTrees = new List<FocusTree>(_focusTrees);
        foreach (var def in _focusTreeDefinitions)
        {
            var compiled = ModCompiler.Compile(def);
            result.Validation = result.Validation.Merge(compiled.Validation);
            allFocusTrees.Add(compiled.Model);
            // We'll collect locs at the end via GetScriptedLocalisations()
        }

        var treeValidator = new FocusTreeValidator(_dataService);
        foreach (var tree in allFocusTrees)
        {
            var validation = treeValidator.Validate(tree);
            result.Validation = result.Validation.Merge(validation);
        }

        if (!result.Validation.IsValid)
            return result;

        // Ensure output directories exist
        var focusDir = Path.Combine(OutputPath, "common", "national_focus");
        Directory.CreateDirectory(focusDir);

        // Emit focus trees
        foreach (var tree in allFocusTrees)
        {
            var filePath = Path.Combine(focusDir, $"{tree.Id}.txt");
            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions
            {
                AddGenerationHeader = true,
                DisposeWriter = false
            });

            clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
            clausewitzWriter.WriteComment($"Do not edit manually. Changes will be overwritten.");
            clausewitzWriter.WriteBlankLine();

            FocusTreeEmitter.Emit(tree, clausewitzWriter);

            result.EmittedFiles.Add(filePath);
        }


        // Collect all scripted localisations during compilation
        var allScriptedLocs = new List<ScriptedLocalisation>(_scriptedLocalisations);

        // Compile Ideas (with dynamic loc support)
        var compiledIdeas = new List<Idea>();
        foreach (var def in _ideaDefinitions)
        {
            var idea = ModCompiler.CompileIdea(def);
            compiledIdeas.Add(idea);
        }

        // Compile Decisions (with dynamic loc support)
        var compiledDecisions = new List<Decision>();
        foreach (var def in _decisionDefinitions)
        {
            var decision = ModCompiler.CompileDecision(def);
            compiledDecisions.Add(decision);
        }

        // Emit events
        if (_eventDefinitions.Count > 0)
        {
            var eventsDir = Path.Combine(OutputPath, "events");
            Directory.CreateDirectory(eventsDir);

            // Group events by namespace: "prefix.1" -> "prefix"
            var eventsByNamespace = new Dictionary<string, List<Event>>();
            foreach (var def in _eventDefinitions)
            {
                var ev = Builders.Compiler.EventCompiler.Compile(def);
                var namespaceName = ev.Id.Split('.')[0];

                if (!eventsByNamespace.ContainsKey(namespaceName))
                    eventsByNamespace[namespaceName] = [];
                eventsByNamespace[namespaceName].Add(ev);
            }

            foreach (var (ns, evList) in eventsByNamespace)
            {
                var eventPath = Path.Combine(eventsDir, $"{ns}.txt");
                using var fileWriter = new StreamWriter(eventPath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions
                {
                    AddGenerationHeader = true,
                    DisposeWriter = false
                });

                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteComment($"Do not edit manually. Changes will be overwritten.");
                clausewitzWriter.WriteBlankLine();

                EventEmitter.Emit(ns, evList, clausewitzWriter);

                result.EmittedFiles.Add(eventPath);
            }
        }

        // Emit decisions
        var decisionsDir = Path.Combine(OutputPath, "common", "decisions");
        if (_decisionCategoryDefinitions.Count > 0 || compiledDecisions.Count > 0)
        {
            var categoriesDir = Path.Combine(decisionsDir, "categories");
            Directory.CreateDirectory(categoriesDir);

            // Emit Categories
            if (_decisionCategoryDefinitions.Count > 0)
            {
                var categoryFile = Path.Combine(categoriesDir, $"{Name.ToLowerInvariant().Replace(' ', '_')}_categories.txt");
                using var fileWriter = new StreamWriter(categoryFile, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions
                {
                    AddGenerationHeader = true,
                    DisposeWriter = false
                });

                var compiledCategories = _decisionCategoryDefinitions
                    .Select(def => new DecisionCategory {
                        Id = def.Id,
                        NameIdentifier = def.Id,
                        Icon = def.Icon?.Value,
                        Visible = def.CompileVisible(),
                        Available = def.CompileAvailable()
                    })
                    .ToList();

                DecisionEmitter.EmitCategories(compiledCategories, clausewitzWriter);
                result.EmittedFiles.Add(categoryFile);
            }
        }

        // Emit Decisions grouped by Category ID
        var decisionsByCategory = compiledDecisions.GroupBy(d => _decisionDefinitions.First(def => def.Id == d.Id).Category);
        foreach (var group in decisionsByCategory)
        {
            var catId = group.Key;
            var decisionFile = Path.Combine(decisionsDir, $"{catId}.txt");
            using var fileWriter = new StreamWriter(decisionFile, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions
            {
                AddGenerationHeader = true,
                DisposeWriter = false
            });

            DecisionEmitter.EmitDecisions(catId, group.ToList(), clausewitzWriter);
            result.EmittedFiles.Add(decisionFile);
        }

        // Collect all scripted localisations from ModCompiler
        allScriptedLocs.AddRange(ModCompiler.GetScriptedLocalisations());

        // Emit scripted localisation
        if (allScriptedLocs.Count > 0)
        {
            var scriptedLocDir = Path.Combine(OutputPath, "common", "scripted_localisation");
            Directory.CreateDirectory(scriptedLocDir);
            
            var scriptedLocPath = Path.Combine(scriptedLocDir, $"00_{Name.ToLowerInvariant().Replace(' ', '_')}_focus_texts.txt");
            using var fileWriter = new StreamWriter(scriptedLocPath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions
            {
                AddGenerationHeader = true,
                DisposeWriter = false
            });
            
            clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name}");
            clausewitzWriter.WriteBlankLine();
            ScriptedLocalisationEmitter.Emit(allScriptedLocs, clausewitzWriter);
            
            result.EmittedFiles.Add(scriptedLocPath);
        }

        // Emit scripted triggers
        if (_scriptedTriggerDefinitions.Count > 0)
        {
            var triggersDir = Path.Combine(OutputPath, "common", "scripted_triggers");
            Directory.CreateDirectory(triggersDir);
            var filePath = Path.Combine(triggersDir, $"{Sanitize(Name)}_triggers.txt");
            
            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });

            var compiled = _scriptedTriggerDefinitions.Select(Builders.Compiler.ScriptedCompiler.CompileTrigger).ToList();
            ScriptedEmitter.EmitTriggers(compiled, clausewitzWriter);
            result.EmittedFiles.Add(filePath);
        }

        // Emit scripted effects
        if (_scriptedEffectDefinitions.Count > 0)
        {
            var effectsDir = Path.Combine(OutputPath, "common", "scripted_effects");
            Directory.CreateDirectory(effectsDir);
            var filePath = Path.Combine(effectsDir, $"{Sanitize(Name)}_effects.txt");
            
            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });

            var compiled = _scriptedEffectDefinitions.Select(Builders.Compiler.ScriptedCompiler.CompileEffect).ToList();
            ScriptedEmitter.EmitEffects(compiled, clausewitzWriter);
            result.EmittedFiles.Add(filePath);
        }

        // Emit Graphics (.gfx) + auto-copy source files
        if (_spriteDefinitions.Count > 0)
        {
            var interfaceDir = Path.Combine(OutputPath, "interface");
            Directory.CreateDirectory(interfaceDir);
            var filePath = Path.Combine(interfaceDir, $"{Sanitize(Name)}.gfx");

            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });

            // Auto-copy source files for sprites that have SourceFile set
            foreach (var spriteDef in _spriteDefinitions)
            {
                if (spriteDef.SourceFile != null && File.Exists(spriteDef.SourceFile))
                {
                    var targetPath = Path.Combine(OutputPath, spriteDef.TextureFile.Replace('/', Path.DirectorySeparatorChar));
                    var targetDir = Path.GetDirectoryName(targetPath);
                    if (targetDir != null) Directory.CreateDirectory(targetDir);
                    File.Copy(spriteDef.SourceFile, targetPath, overwrite: true);
                    result.EmittedFiles.Add(targetPath);
                }
            }

            var sprites = _spriteDefinitions.Select(Builders.Compiler.GfxCompiler.Compile).ToList();
            var gfxFile = new GfxFile { Id = Name, Sprites = sprites };
            GfxEmitter.Emit(gfxFile, clausewitzWriter);
            result.EmittedFiles.Add(filePath);
        }

        // Emit Ideas
        if (compiledIdeas.Count > 0)
        {
            var ideasDir = Path.Combine(OutputPath, "common", "ideas");
            Directory.CreateDirectory(ideasDir);
            var filePath = Path.Combine(ideasDir, $"{Sanitize(Name)}_ideas.txt");
            
            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });

            IdeaEmitter.Emit(compiledIdeas, clausewitzWriter);
            result.EmittedFiles.Add(filePath);
        }
        // Emit Characters
        if (_characterDefinitions.Count > 0)
        {
            var charValidator = new CharacterValidator(_dataService);
            var charDir = Path.Combine(OutputPath, "common", "characters");
            Directory.CreateDirectory(charDir);
            var filePath = Path.Combine(charDir, $"{Sanitize(Name)}_characters.txt");
            
            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });

            var compiled = new List<Character>();
            foreach (var def in _characterDefinitions)
            {
                var character = Builders.Compiler.ModCompiler.CompileCharacter(def);
                result.Validation = result.Validation.Merge(charValidator.Validate(character));
                compiled.Add(character);
            }
            
            CharacterEmitter.Emit(clausewitzWriter, compiled);
            result.EmittedFiles.Add(filePath);
        }

        // Emit Technologies
        if (_technologyDefinitions.Count > 0)
        {
            var techDir = Path.Combine(OutputPath, "common", "technologies");
            Directory.CreateDirectory(techDir);
            var filePath = Path.Combine(techDir, $"{Sanitize(Name)}_technologies.txt");
            
            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });

            var compiled = _technologyDefinitions.Select(t => t.Build()).ToList();
            TechnologyEmitter.Emit(compiled, clausewitzWriter);
            result.EmittedFiles.Add(filePath);
        }

        // Emit On Actions
        if (_onActionDefinitions.Count > 0)
        {
            var onActionsDir = Path.Combine(OutputPath, "common", "on_actions");
            Directory.CreateDirectory(onActionsDir);
            var filePath = Path.Combine(onActionsDir, $"{Sanitize(Name)}_on_actions.txt");

            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });

            var allActions = _onActionDefinitions.SelectMany(d => d.Build()).ToList();
            OnActionEmitter.Emit(allActions, clausewitzWriter);
            result.EmittedFiles.Add(filePath);
        }

        // Emit Opinion Modifiers
        if (_opinionModifierDefinitions.Count > 0)
        {
            var opDir = Path.Combine(OutputPath, "common", "opinion_modifiers");
            Directory.CreateDirectory(opDir);
            var filePath = Path.Combine(opDir, $"{Sanitize(Name)}_opinion_modifiers.txt");

            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });

            var allModifiers = _opinionModifierDefinitions.SelectMany(d => d.Build()).ToList();
            OpinionModifierEmitter.Emit(allModifiers, clausewitzWriter);
            result.EmittedFiles.Add(filePath);
        }

        // Emit Dynamic Modifiers
        if (_dynamicModifierDefinitions.Count > 0)
        {
            var dynDir = Path.Combine(OutputPath, "common", "dynamic_modifiers");
            Directory.CreateDirectory(dynDir);
            var filePath = Path.Combine(dynDir, $"{Sanitize(Name)}_dynamic_modifiers.txt");

            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });

            var compiled = _dynamicModifierDefinitions.Select(d => d.Build()).ToList();
            DynamicModifierEmitter.Emit(compiled, clausewitzWriter);
            result.EmittedFiles.Add(filePath);
        }

        // Emit Country Tags + Country Files
        if (_countryDefinitions.Count > 0)
        {
            // country_tags file
            var tagsDir = Path.Combine(OutputPath, "common", "country_tags");
            Directory.CreateDirectory(tagsDir);
            var tagsFile = Path.Combine(tagsDir, $"{Sanitize(Name)}_countries.txt");

            using (var fileWriter = new StreamWriter(tagsFile, false, System.Text.Encoding.UTF8))
            using (var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false }))
            {
                CountryEmitter.EmitTags(_countryDefinitions, clausewitzWriter);
            }
            result.EmittedFiles.Add(tagsFile);

            // individual country files
            var countriesDir = Path.Combine(OutputPath, "common", "countries");
            Directory.CreateDirectory(countriesDir);
            foreach (var country in _countryDefinitions)
            {
                var countryFile = Path.Combine(countriesDir, $"{country.Tag} - {country.Tag}.txt");
                using var fileWriter = new StreamWriter(countryFile, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                CountryEmitter.EmitCountryFile(country, clausewitzWriter);
                result.EmittedFiles.Add(countryFile);
            }
        }

        // Emit Country Histories
        if (_countryHistoryDefinitions.Count > 0)
        {
            var historyDir = Path.Combine(OutputPath, "history", "countries");
            Directory.CreateDirectory(historyDir);
            foreach (var def in _countryHistoryDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(historyDir, $"{def.Tag} - {def.Tag}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteBlankLine();
                CountryHistoryEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit States
        if (_stateDefinitions.Count > 0)
        {
            var statesDir = Path.Combine(OutputPath, "history", "states");
            Directory.CreateDirectory(statesDir);
            foreach (var def in _stateDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(statesDir, $"{def.Id}-{def.StateName}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                StateEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit OOBs (Order of Battle)
        if (_oobDefinitions.Count > 0)
        {
            var unitsDir = Path.Combine(OutputPath, "history", "units");
            Directory.CreateDirectory(unitsDir);
            foreach (var def in _oobDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(unitsDir, $"{built.FileName}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteBlankLine();
                OobEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Naval OOBs
        if (_navalOobDefinitions.Count > 0)
        {
            var unitsDir = Path.Combine(OutputPath, "history", "units");
            Directory.CreateDirectory(unitsDir);
            foreach (var def in _navalOobDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(unitsDir, $"{built.FileName}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteBlankLine();
                OobEmitter.EmitNaval(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Air OOBs
        if (_airOobDefinitions.Count > 0)
        {
            var unitsDir = Path.Combine(OutputPath, "history", "units");
            Directory.CreateDirectory(unitsDir);
            foreach (var def in _airOobDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(unitsDir, $"{built.FileName}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteBlankLine();
                OobEmitter.EmitAir(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Sub-Units
        if (_subUnitDefinitions.Count > 0)
        {
            var unitsDir = Path.Combine(OutputPath, "common", "units");
            Directory.CreateDirectory(unitsDir);
            foreach (var def in _subUnitDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(unitsDir, $"{built.FileName}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteBlankLine();
                SubUnitEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Equipment
        if (_equipmentDefinitions.Count > 0)
        {
            var equipDir = Path.Combine(OutputPath, "common", "units", "equipment");
            Directory.CreateDirectory(equipDir);
            foreach (var def in _equipmentDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(equipDir, $"{built.FileName}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteBlankLine();
                EquipmentEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Continuous Focuses
        if (_continuousFocusDefinitions.Count > 0)
        {
            var cfDir = Path.Combine(OutputPath, "common", "continuous_focus");
            Directory.CreateDirectory(cfDir);
            foreach (var def in _continuousFocusDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(cfDir, $"{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteBlankLine();
                ContinuousFocusEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Bookmarks
        if (_bookmarkDefinitions.Count > 0)
        {
            var bookmarkDir = Path.Combine(OutputPath, "common", "bookmarks");
            Directory.CreateDirectory(bookmarkDir);
            foreach (var def in _bookmarkDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(bookmarkDir, $"{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteBlankLine();
                BookmarkEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit AI Strategies
        if (_aiStrategyDefinitions.Count > 0)
        {
            var aiDir = Path.Combine(OutputPath, "common", "ai_strategy");
            Directory.CreateDirectory(aiDir);
            foreach (var def in _aiStrategyDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(aiDir, $"{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                clausewitzWriter.WriteComment($"Generated by ObjectiveIron — {Name} v{Version}");
                clausewitzWriter.WriteBlankLine();
                AiStrategyEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Country Leader Traits
        if (_countryLeaderTraitDefinitions.Count > 0)
        {
            var dir = Path.Combine(OutputPath, "common", "country_leader", "traits");
            Directory.CreateDirectory(dir);
            foreach (var def in _countryLeaderTraitDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(dir, $"{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                LeaderTraitEmitter.Emit(built, clausewitzWriter, "leader_traits");
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Unit Leader Traits
        if (_unitLeaderTraitDefinitions.Count > 0)
        {
            var dir = Path.Combine(OutputPath, "common", "unit_leader");
            Directory.CreateDirectory(dir);
            foreach (var def in _unitLeaderTraitDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(dir, $"{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                LeaderTraitEmitter.Emit(built, clausewitzWriter, "leader_traits");
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Occupation Laws
        if (_occupationLawDefinitions.Count > 0)
        {
            var dir = Path.Combine(OutputPath, "common", "occupation_laws");
            Directory.CreateDirectory(dir);
            foreach (var def in _occupationLawDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(dir, $"{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                OccupationLawEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Wargoals
        if (_wargoalDefinitions.Count > 0)
        {
            var dir = Path.Combine(OutputPath, "common", "wargoals");
            Directory.CreateDirectory(dir);
            foreach (var def in _wargoalDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(dir, $"{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                WargoalEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Operations
        if (_operationDefinitions.Count > 0)
        {
            var dir = Path.Combine(OutputPath, "common", "operations");
            Directory.CreateDirectory(dir);
            foreach (var def in _operationDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(dir, $"{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                OperationEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Abilities
        if (_abilityDefinitions.Count > 0)
        {
            var dir = Path.Combine(OutputPath, "common", "abilities");
            Directory.CreateDirectory(dir);
            foreach (var def in _abilityDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(dir, $"{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                AbilityEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Phase F features
        EmitGeneric(_buildingDefinitions, "common/buildings", d => d.Id, (d, w) => BuildingEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_staticModifierDefinitions, "common/static_modifiers", d => d.Id, (d, w) => StaticModifierEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_gameRuleDefinitions, "common/game_rules", d => d.Id, (d, w) => GameRuleEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_terrainDefinitions, "common/terrain", d => d.Id, (d, w) => TerrainEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_stateCategoryDefinitions, "common/state_category", d => d.Id, (d, w) => StateCategoryEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_resourceDefinitions, "common/resources", d => d.Id, (d, w) => ResourceDefEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_techSharingDefinitions, "common/technology_sharing", d => d.Id, (d, w) => TechSharingEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_ideologyDefinitions, "common/ideologies", d => d.Id, (d, w) => IdeologyEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_autonomyStateDefinitions, "common/autonomous_states", d => d.Id, (d, w) => AutonomyStateEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_difficultyDefinitions, "common/difficulty_settings", d => d.Id, (d, w) => DifficultyEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_nameDefinitions, "common/names", d => d.Id, (d, w) => NameEmitter.Emit(d.Build(), w), result);

        // Phase G features
        EmitGeneric(_mioDefinitions, "common/military_industrial_organization/organizations", d => d.Id, (d, w) => MioEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_intelAgencyDefinitions, "common/intelligence_agencies", d => d.Id, (d, w) => IntelAgencyEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_peaceConferenceDefinitions, "common/peace_conference", d => d.Id, (d, w) => PeaceConferenceEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_bopDefinitions, "common/bop", d => d.Id, (d, w) => BopEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_scriptedDiploDefinitions, "common/scripted_diplomatic_actions", d => d.Id, (d, w) => ScriptedDiploEmitter.Emit(d.Build(), w), result);
        EmitGeneric(_scriptedGuiDefinitions, "common/scripted_guis", d => d.Id, (d, w) => ScriptedGuiEmitter.Emit(d.Build(), w), result);

        // Phase H: GUI files
        if (_guiDefinitions.Count > 0)
        {
            var guiDir = Path.Combine(OutputPath, "interface");
            Directory.CreateDirectory(guiDir);
            foreach (var def in _guiDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(guiDir, $"{def.Id}.gui");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                GuiEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Phase H: Music
        if (_musicDefinitions.Count > 0)
        {
            var musicDir = Path.Combine(OutputPath, "music");
            Directory.CreateDirectory(musicDir);
            foreach (var def in _musicDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(musicDir, $"{def.Id}_songs.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                MusicEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Phase H: Sound
        if (_soundDefinitions.Count > 0)
        {
            var soundDir = Path.Combine(OutputPath, "sound");
            Directory.CreateDirectory(soundDir);
            foreach (var def in _soundDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(soundDir, $"{def.Id}.asset");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                SoundEmitter.Emit(built, fileWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Phase H: Strategic Regions
        if (_strategicRegionDefinitions.Count > 0)
        {
            var regionDir = Path.Combine(OutputPath, "map", "strategicregions");
            Directory.CreateDirectory(regionDir);
            foreach (var def in _strategicRegionDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(regionDir, $"{built.RegionId}-{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                StrategicRegionEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Phase H: Supply Areas
        if (_supplyAreaDefinitions.Count > 0)
        {
            var supplyDir = Path.Combine(OutputPath, "map", "supplyareas");
            Directory.CreateDirectory(supplyDir);
            foreach (var def in _supplyAreaDefinitions)
            {
                var built = def.Build();
                var filePath = Path.Combine(supplyDir, $"{built.AreaId}-{def.Id}.txt");
                using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
                SupplyAreaEmitter.Emit(built, clausewitzWriter);
                result.EmittedFiles.Add(filePath);
            }
        }

        // Emit Localisation
        var emittedLocs = LocalisationEmitter.Emit(
            Name,
            _focusDefinitions,
            _eventDefinitions,
            _decisionCategoryDefinitions,
            _decisionDefinitions,
            _ideaDefinitions,
            _characterDefinitions,
            _technologyDefinitions,
            _countryDefinitions,
            _stateDefinitions,
            OutputPath);
        result.EmittedFiles.AddRange(emittedLocs);

        // Emit Localisation Replace
        if (_localisationReplaceDefinitions.Count > 0)
        {
            var emittedReplace = LocalisationEmitter.EmitReplace(_localisationReplaceDefinitions, OutputPath);
            result.EmittedFiles.AddRange(emittedReplace);
        }

        // Emit Map files
        if (_provinceDefinitions.Count > 0)
        {
            var path = MapEmitter.EmitDefinitionCsv(_provinceDefinitions, OutputPath);
            result.EmittedFiles.Add(path);
        }

        if (_adjacencyDefinitions.Count > 0)
        {
            var path = MapEmitter.EmitAdjacenciesCsv(_adjacencyDefinitions, OutputPath);
            result.EmittedFiles.Add(path);
        }

        if (_defaultMapDefinition != null)
        {
            var path = MapEmitter.EmitDefaultMap(_defaultMapDefinition.Build(), OutputPath);
            result.EmittedFiles.Add(path);
        }

        // Copy raw assets
        foreach (var asset in _rawAssets)
        {
            var targetPath = Path.Combine(OutputPath, asset.TargetRelativePath.Replace('/', Path.DirectorySeparatorChar));
            var targetDir = Path.GetDirectoryName(targetPath);
            if (targetDir != null) Directory.CreateDirectory(targetDir);

            if (File.Exists(asset.SourcePath))
            {
                File.Copy(asset.SourcePath, targetPath, overwrite: true);
                result.EmittedFiles.Add(targetPath);
            }
        }

        // Emit mod descriptor
        EmitDescriptor(result);

        result.Success = true;
        return result;
    }

    private string Sanitize(string name) => name.ToLowerInvariant().Replace(' ', '_').Replace('-', '_');

    private void EmitDescriptor(EmitResult result)
    {
        var descriptorPath = Path.Combine(OutputPath, "descriptor.mod");
        using var writer = new StreamWriter(descriptorPath, false, System.Text.Encoding.UTF8);

        writer.WriteLine($"name = \"{Name}\"");
        writer.WriteLine($"version = \"{Version}\"");
        writer.WriteLine($"supported_version = \"{SupportedGameVersion}\"");

        if (Tags.Count > 0)
        {
            writer.WriteLine("tags = {");
            foreach (var tag in Tags)
                writer.WriteLine($"\t\"{tag}\"");
            writer.WriteLine("}");
        }

        result.EmittedFiles.Add(descriptorPath);
    }

    private void EmitGeneric<T>(List<T> definitions, string subPath, Func<T, string> getId,
        Action<T, ClausewitzWriter> emitAction, EmitResult result)
    {
        if (definitions.Count == 0) return;
        var dir = Path.Combine(OutputPath, subPath.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(dir);
        foreach (var def in definitions)
        {
            var filePath = Path.Combine(dir, $"{getId(def)}.txt");
            using var fileWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            using var clausewitzWriter = new ClausewitzWriter(fileWriter, new WriterOptions { AddGenerationHeader = true, DisposeWriter = false });
            emitAction(def, clausewitzWriter);
            result.EmittedFiles.Add(filePath);
        }
    }
}

/// <summary>Result of an emit operation.</summary>
public sealed class EmitResult
{
    public bool Success { get; set; }
    public ValidationResult Validation { get; set; } = ValidationResult.Success();
    public List<string> EmittedFiles { get; } = [];

    public override string ToString()
    {
        if (!Success)
            return $"Emit failed: {Validation}";
        return $"Emit succeeded. {EmittedFiles.Count} file(s) generated.";
    }
}
