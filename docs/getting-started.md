# Getting Started

ObjectiveIron を使って HoI4 MOD を作成する手順です。

## 1. プロジェクト作成

```bash
mkdir MyMod && cd MyMod
dotnet new console
dotnet add reference /path/to/ObjectiveIron.Emitter.csproj
dotnet add reference /path/to/ObjectiveIron.Builders.csproj
```

または `examples/ExampleMod/` をコピーして改変するのが最も簡単です。

## 2. 国家を定義

```csharp
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models.Primitives;

public class MyCountry : CountryDefinition
{
    public override string Tag => "MYC";
    public override (int R, int G, int B) Color => (100, 150, 200);
    public override LocalizedText? Name => new() { En = "My Country", Ja = "マイカントリー" };
}
```

## 3. 国家ヒストリーを定義

```csharp
using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;

public class MyCountryHistory : CountryHistoryDefinition
{
    public override string Tag => "MYC";
    public override int? Capital => 1;
    public override string? Oob => "MYC_1936";
    public override int? ResearchSlots => 3;
    public override double? Stability => 0.5;

    protected override void Politics(PoliticsScope p)
    {
        p.RulingParty("democratic").ElectionsAllowed(true);
    }

    protected override void Popularities(PopularityScope p)
    {
        p.Democratic(60).Neutrality(30).Fascism(5).Communism(5);
    }

    protected override void Technologies(TechSetScope t)
    {
        t.Add("infantry_weapons", "tech_support");
    }
}
```

## 4. 国家方針ツリーを定義

フォーカスはクラスとして定義し、ツリーで接続します。

```csharp
using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Builders.Types;
using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

public class IndustryFocus : FocusDefinition
{
    public override string Id => "MYC_industry";
    public override LocalizedText Title => new() { En = "Industrialize" };
    public override GfxSprite Icon => Icons.GenericProduction;
    public override FocusPosition Position => new(0, 0);
    public override int Cost => 10;

    protected override void CompletionReward(EffectScope e)
    {
        e.AddBuildingConstruction(BuildingType.IndustrialComplex, 2, instantBuild: true);
    }
}

public class MyFocusTree : FocusTreeDefinition
{
    public override string Id => "myc_focus";
    public override CountryTag Country => new("MYC");

    public IndustryFocus Industry { get; } = new();

    protected override void Structure(FocusGraph graph)
    {
        graph.Root(Industry);
    }
}
```

## 5. MODプロジェクトとして出力

```csharp
using ObjectiveIron.Emitter;

var project = new ModProject("My Mod", "./output")
{
    Version = "1.0.0",
    SupportedGameVersion = "1.14.*"
};

project.AddCountries(new MyCountry());
project.AddCountryHistories(new MyCountryHistory());

var tree = new MyFocusTree();
project.AddFocusTrees(tree);
project.AddFocusDefinitions(tree.Industry);

project.Emit();
```

## 6. 実行

```bash
dotnet run -- -o "C:/Users/you/Documents/Paradox Interactive/Hearts of Iron IV/mod/MyMod"
```

生成されたファイル群がそのまま HoI4 の MOD として動作します。

## 静的アセット（画像・音声等）

バイナリファイルは `assets/` フォルダに配置し、`AddRawAssetDirectory()` でまるごとコピーできます。

```
MyMod/
├── assets/
│   ├── gfx/interface/goals/my_icon.dds
│   └── map/provinces.bmp
├── Program.cs
└── MyMod.csproj
```

```csharp
project.AddRawAssetDirectory("./assets");
```

スプライト定義では `SourceFile` を指定すると自動コピーされます：

```csharp
public class MyIcon : SpriteDefinition
{
    public override string Name => "GFX_my_icon";
    public override string? SourceFile => "./assets/my_icon.dds";
    // gfx/interface/goals/my_icon.dds に自動コピー
}
```

## バニラの上書き

既存のTAG・State IDを使えばバニラを上書きできます。

```csharp
// ドイツを上書き
public class GermanyOverride : CountryHistoryDefinition
{
    public override string Tag => "GER";
    // ... 全フィールドを再定義
}

// State 1 を上書き
public class StateOverride : StateDefinition
{
    public override int Id => 1;
    // ... 全フィールドを再定義
}
```

ローカライゼーションの上書きは `LocalisationReplaceDefinition` で：

```csharp
public class MyLocReplace : LocalisationReplaceDefinition
{
    public override string Id => "my_overrides";

    protected override void Define(LocalisationReplaceScope s)
    {
        s.Replace("POLITICS_FASCISM", new LocalizedText
        {
            En = "Ultranationalism", Ja = "超国家主義"
        });
    }
}
```

## Custom エスケープハッチ

すべてのScope（Effect / Trigger / Modifier）で、専用メソッドが無いバニラコマンドも `Custom` で使えます：

```csharp
e.Custom("some_exotic_effect", "value");
e.CustomBlock("complex_block", inner => {
    inner.Custom("key1", 42);
    inner.Custom("key2", "text");
});
```
