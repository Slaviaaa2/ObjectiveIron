using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Emitter;

namespace ExampleMod;

/// <summary>
/// Example: 架空国家「Exampleia」のMODを生成する完全なサンプル.
/// dotnet run -- -o ./output で出力を生成できます.
/// </summary>
class Program
{
    static int Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var outputPath = args.Length >= 2 && args[0] == "-o" ? args[1] : "./output";

        // ─── MODプロジェクト作成 ──────────────────────────────
        var project = new ModProject("Exampleia Mod", outputPath)
        {
            Version = "1.0.0",
            SupportedGameVersion = "1.14.*",
            Tags = ["Alternative History"]
        };

        // ─── 国家定義 ────────────────────────────────────────
        project.AddCountries(new ExampleiaCountry());
        project.AddCountryHistories(new ExampleiaHistory());

        // ─── 国家方針ツリー ──────────────────────────────────
        var tree = new ExampleiaFocusTree();
        project.AddFocusTrees(tree);
        project.AddFocusDefinitions(
            tree.IndustrialEffort,
            tree.MilitaryBuildup,
            tree.Diplomacy
        );

        // ─── 国家精神・アイデア ──────────────────────────────
        project.AddIdeas(new NationalUnitySpirit());

        // ─── イベント ────────────────────────────────────────
        project.AddEvents(new IndustrialBoomEvent());

        // ─── State定義 ───────────────────────────────────────
        project.AddStates(new ExampleiaMainland());

        // ─── 陸軍OOB ────────────────────────────────────────
        project.AddOobs(new Exampleia1936Oob());

        // ─── ローカライゼーション上書き ──────────────────────
        project.AddLocalisationReplace(new ExampleiaLocReplace());

        // ─── 静的アセット ────────────────────────────────────
        // assets/ フォルダの中身をまるごとコピー
        project.AddRawAssetDirectory("./assets");

        // ─── 出力 ────────────────────────────────────────────
        var result = project.Emit();

        if (result.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Build successful! {result.EmittedFiles.Count} files generated.");
            Console.ResetColor();
            foreach (var file in result.EmittedFiles)
                Console.WriteLine($"  {file}");
            return 0;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine($"Build failed: {result}");
        Console.ResetColor();
        return 1;
    }
}
