using ObjectiveIron.Builders.Compiler;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Cli.Sample;
using ObjectiveIron.Emitter;

namespace ObjectiveIron.Cli;

class Program
{
    static int Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (args.Length == 0 || args[0] == "--help" || args[0] == "-h")
        {
            PrintHelp();
            return 0;
        }

        return args[0].ToLowerInvariant() switch
        {
            "sample" => RunSample(args),
            "build" => RunBuild(args),
            "version" => PrintVersion(),
            _ => Error($"Unknown command: {args[0]}")
        };
    }

    static int RunSample(string[] args)
    {
        var outputPath = GetArgValue(args, "-o", "--output") ?? "./sample_output";

        Console.WriteLine("🔨 ObjectiveIron — Generating sample mod...");
        Console.WriteLine();

        var tree = new ExampleFocusTree();

        var project = new ModProject("ObjectiveIron Sample", outputPath)
        {
            Version = "1.1.0",
            SupportedGameVersion = "1.14.*",
            Tags = ["Alternative History", "Gameplay"],
            GamePath = @"C:\Program Files (x86)\Steam\steamapps\common\Hearts of Iron IV"
        };
        
        project.AddFocusTrees(tree);
        project.AddFocusDefinitions(
            tree.IndustrialEffort,
            tree.MilitaryExpansion,
            tree.ResearchPush,
            tree.PoliticalManeuvering,
            tree.WarPreparation,
            tree.UltimateDecision
        );
        
        project.AddEvents(new SampleEvent());
        project.AddDecisionCategories(new PoliticalDecisions(), new MyCategory());
        project.AddDecisions(new CommissionIndustrialReport(), new DynamicDecision());
        project.AddScriptedTriggers(new HighStabilityTrigger());
        project.AddScriptedEffects(new StandardIndustrialBoost(), new GiveBonusEffect());
        project.AddSprites(new CustomFocusIcon(), new CustomEventPicture(), new MikhailGromanPortrait(),
            new AutoResolvedSprite(), new AutoResolvedLeaderPortrait());
        project.AddIdeas(new EconomicRecoverySpirit(), new MilitarizedSocietySpirit(), new DynamicIdea());
        project.AddCharacters(new MikhailGroman(), new VasilyLutov(), new MyGeneral());
        project.AddTechnologies(new AtomicResearch(), new NuclearReactorTech());
        project.AddOnActions(new SampleOnActions());
        project.AddOpinionModifiers(new SampleOpinionModifiers());
        project.AddDynamicModifiers(new WarExhaustionModifier(), new IndustrialBoomModifier());
        project.AddCountries(new SampleCountryExampleia());
        project.AddCountryHistories(new ExampleiaHistory());
        project.AddStates(new ExampleiaCapitalState(), new ExampleiaRuralState());
        project.AddOobs(new Exampleia1936Oob());
        project.AddNavalOobs(new ExampleiaNavalOob());
        project.AddAirOobs(new ExampleiaAirOob());
        project.AddSubUnits(new SampleEliteInfantry());
        project.AddEquipment(new SampleInfantryEquipment());
        project.AddContinuousFocuses(new ExampleiaContinuousFocus());
        project.AddBookmarks(new SampleBookmark());
        project.AddAiStrategies(new ExampleiaAiStrategy());
        project.AddCountryLeaderTraits(new SampleCountryLeaderTraits());
        project.AddUnitLeaderTraits(new SampleUnitLeaderTraits());
        project.AddOccupationLaws(new SampleOccupationLaws());
        project.AddWargoals(new SampleWargoals());
        project.AddOperations(new SampleOperations());
        project.AddAbilities(new SampleAbilities());
        project.AddBuildings(new SampleBuildings());
        project.AddGameRules(new SampleGameRules());
        project.AddStateCategories(new SampleStateCategories());
        project.AddIdeologies(new SampleIdeologies());
        project.AddMios(new SampleMio());
        project.AddScriptedGuis(new SampleScriptedGui());
        project.AddGuis(new SampleGui());
        project.AddMusic(new SampleMusic());
        project.AddSounds(new SampleSound());
        project.AddLocalisationReplace(new SampleLocalisationReplace());

        // Map definitions
        project.SetDefaultMap(new SampleDefaultMap());
        project.AddProvinceDefinitions(
            new ProvinceDefinition(1, 130, 60, 50, ProvinceType.Land, false, "plains", "europe"),
            new ProvinceDefinition(2, 140, 70, 60, ProvinceType.Land, true, "forest", "europe"),
            new ProvinceDefinition(5000, 0, 0, 200, ProvinceType.Sea, false, "ocean", "europe")
        );
        project.AddAdjacencies(
            new AdjacencyDefinition(1, 2, AdjacencyType.Land, -1),
            new AdjacencyDefinition(2, 5000, AdjacencyType.Sea, -1)
        );

        // Raw assets: copy the entire "assets/" directory if it exists
        // project.AddRawAssetDirectory("./assets", "");
        // Or individual files:
        // project.AddRawAsset("./my_map/provinces.bmp", "map/provinces.bmp");

        var result = project.Emit();

        if (result.Success)
        {
            if (result.Validation.HasWarnings)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠️  Warnings: {result.Validation}");
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ {result}");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Generated files:");
            foreach (var file in result.EmittedFiles)
                Console.WriteLine($"  📄 {file}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {result}");
            Console.ResetColor();
            return 1;
        }

        return 0;
    }

    static int RunBuild(string[] args)
    {
        Console.WriteLine("🔨 ObjectiveIron — Build command");
        Console.WriteLine("   Build from user-defined mod projects is not yet implemented.");
        Console.WriteLine("   Use 'objective-iron sample -o ./output' to generate a sample mod.");
        return 0;
    }

    static int PrintVersion()
    {
        Console.WriteLine("ObjectiveIron v0.2.0");
        return 0;
    }

    static void PrintHelp()
    {
        Console.WriteLine("ObjectiveIron — HoI4 C# Transpiler");
        Console.WriteLine();
        Console.WriteLine("Usage: objective-iron <command> [options]");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  sample      Generate a sample mod to verify the pipeline");
        Console.WriteLine("  build       Build a mod from C# definitions (WIP)");
        Console.WriteLine("  version     Show version information");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  -o, --output <path>    Output directory (default: ./sample_output)");
        Console.WriteLine("  -h, --help             Show this help message");
    }

    static string? GetArgValue(string[] args, params string[] keys)
    {
        for (int i = 0; i < args.Length - 1; i++)
            if (keys.Contains(args[i])) return args[i + 1];
        return null;
    }

    static int Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine($"Error: {message}");
        Console.ResetColor();
        return 1;
    }
}
