# ObjectiveIron

**Hearts of Iron IV のMODをC#で書く。**

HoI4のMODスクリプトをC#のFluent APIで定義し、Paradox Clausewitzエンジンのネイティブフォーマットにトランスパイルするツールです。

## なぜ？

| 課題 | ObjectiveIron の解決策 |
|------|----------------------|
| タイポがランタイムまで検出されない | C#の型安全性 + コンパイル時チェック |
| IDE補完がない | Fluent API による IntelliSense 完全対応 |
| コピペ地獄 | C#のクラス継承・メソッドによるコード再利用 |
| 構造的な間違いが分からない | 循環依存検出 / 参照整合性バリデーション |

## クイックスタート

```bash
# サンプルMODを生成
dotnet run --project src/ObjectiveIron.Cli -- sample -o ./sample_output

# Exampleプロジェクトを実行
dotnet run --project examples/ExampleMod -- -o ./example_output

# ビルド & テスト
dotnet build
dotnet test
```

## 使い方

`ModProject` にDefinitionクラスを登録して `Emit()` するだけで完全なMOD構造が出力されます。

```csharp
var project = new ModProject("My Mod", "./output")
{
    Version = "1.0.0",
    SupportedGameVersion = "1.14.*",
    Tags = ["Alternative History"]
};

// 国家
project.AddCountries(new MyCountry());
project.AddCountryHistories(new MyCountryHistory());

// 国家方針ツリー
var tree = new MyFocusTree();
project.AddFocusTrees(tree);
project.AddFocusDefinitions(tree.Focus1, tree.Focus2);

// イベント・ディシジョン・アイデア等
project.AddEvents(new MyEvent());
project.AddIdeas(new MyNationalSpirit());

// 静的アセット (画像・音声等)
project.AddRawAssetDirectory("./assets");

// 出力
var result = project.Emit();
```

詳細な使用例は [`examples/ExampleMod/`](examples/ExampleMod/) を参照してください。

## サポート対象

### 主要コンテンツ
- **Focus Trees** (国家方針ツリー) — 動的タイトル / 排他 / AI重み
- **Events** (イベント) — country / news / state イベント
- **Decisions** (ディシジョン) — カテゴリ / タイマー / ターゲット
- **Ideas** (国家精神) — modifier / on_add / on_remove / trait
- **Characters** (キャラクター) — 指導者 / 将軍 / 提督 / 顧問
- **Technologies** (技術) — カテゴリ / フォルダ / ツリー構造

### 国家・ヒストリー
- **Country Definitions** — タグ / 色 / graphical_culture / flag_colors
- **Country History** — 政体 / 人気度 / 技術 / OOB / 効果 / 日付エントリ
- **States** — プロヴィンス / 資源 / 建造物 / VP / controller / impassable
- **OOB** (陸軍) — 師団テンプレート / 配置 / 経験値
- **Naval OOB** — 艦隊 / 任務部隊 / 個艦定義
- **Air OOB** — 航空団 / 機種 / 配置

### 軍事・装備
- **Sub-Units** — 大隊/中隊タイプ定義
- **Equipment** — 装備定義 / アーキタイプ / バリアント
- **Division Templates** — 連隊配置 / 支援中隊

### 政治・外交
- **Ideologies** — カスタムイデオロギー
- **Occupation Laws** — 占領法
- **Wargoals** — 戦争目標
- **Operations** — 諜報作戦
- **Abilities** — 指揮官能力
- **AI Strategies** — AI戦略計画

### インフラ
- **Buildings** — カスタム建造物タイプ
- **State Categories** — 州カテゴリ
- **Static Modifiers** / **Game Rules** / **Terrain**
- **Autonomy States** / **Difficulty Settings**
- **Tech Sharing** / **Resources** / **Names**

### DLC機能
- **MIO** (軍産複合体) / **Intelligence Agency**
- **Peace Conference** / **Balance of Power**
- **Scripted Diplomatic Actions** / **Scripted GUI**

### UI・メディア
- **GUI** (.gui) — ウィンドウ / ボタン / アイコン / テキスト
- **Sprites** (.gfx) — 自動アセットコピー対応
- **Music** / **Sound** (.asset)

### マップ
- **Strategic Regions** / **Supply Areas**
- **default.map** / **definition.csv** / **adjacencies.csv**
- **Raw Assets** — provinces.bmp等のバイナリファイルコピー

### ローカライゼーション
- **多言語対応** — En / Ja / De / Fr / Es / Pt / Ru / Pl / Zh / Ko
- **Localisation Replace** — バニラ文字列の上書き (`localisation/replace/`)

### スクリプト
- **Scripted Triggers** / **Scripted Effects** / **Scripted Localisation**
- **On Actions** / **Opinion Modifiers** / **Dynamic Modifiers**
- **Bookmarks** / **Continuous Focus**
- **Country/Unit Leader Traits**

## Scope API

Effect / Trigger / Modifier はFluent APIで型安全に記述できます。
すべてのScopeに `Custom()` エスケープハッチがあり、未実装のバニラコマンドも即座に使用可能です。

```csharp
// Effect例
e.AddPoliticalPower(100);
e.SetCountryFlag("my_flag");
e.SetVariable("my_var", 42);
e.CreateFaction("My Alliance");
e.EveryOwnedState(s => s.AddBuildingConstruction(BuildingType.Infrastructure, 1));

// Trigger例
t.HasCompletedFocus("my_focus");
t.HasCountryFlag("my_flag");
t.Or(o => { o.HasWar(); o.HasStability(Operator.LessThan, 0.5); });

// Modifier例
m.StabilityFactor(0.1);
m.ArmyAttackFactor(0.15);
m.ResearchSpeedFactor(0.05);

// 未実装コマンドもCustomで即使用可能
e.Custom("some_new_effect", "value");
e.CustomBlock("complex_effect", inner => { inner.Custom("key", 1); });
```

## GFX自動アセット管理

`SourceFile` を指定するとEmit時にファイルが自動コピーされます。

```csharp
public class MyIcon : SpriteDefinition
{
    public override string Name => "GFX_my_icon";
    public override string? SourceFile => "./assets/my_icon.dds";
    // → gfx/interface/goals/my_icon.dds に自動コピー + .gfxに登録

    // リーダーポートレート等の場合:
    // public override string TextureFolder => "gfx/leaders/EXA/";
}
```

## プロジェクト構成

```
ObjectiveIron/
├── src/
│   ├── ObjectiveIron.Core       # ドメインモデル + バリデーション
│   ├── ObjectiveIron.Builders   # Fluent API (Definition / Scope)
│   ├── ObjectiveIron.Emitter    # Clausewitz形式出力エンジン
│   └── ObjectiveIron.Cli        # CLIツール + サンプル
├── tests/                       # xUnit テスト
├── examples/
│   └── ExampleMod/              # 完全なMODプロジェクト例
└── docs/                        # ドキュメント
```

### パイプライン

```
C# Definition (ユーザーコード)
       ↓ Build()
  Core Domain Models
       ↓ Validate()
  Validation Results
       ↓ Emit()
  .txt / .yml / .gfx / .gui / .csv ファイル群
```

## 必要環境

- .NET 8.0 SDK
- HoI4 (MODの動作確認用)

## Disclaimer

This is an unofficial, community-developed tool and is not affiliated with, endorsed by, or associated with Paradox Interactive AB. Hearts of Iron IV, Clausewitz Engine, and related trademarks are the property of Paradox Interactive AB.

This tool does not contain any game assets, data files, or proprietary content from Hearts of Iron IV. It generates mod files based on user-defined C# code. Users are responsible for ensuring their mods comply with [Paradox Interactive's mod policy](https://www.paradoxinteractive.com/modding-policy).

## ライセンス

[MIT License](LICENSE)
