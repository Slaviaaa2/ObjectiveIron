# Definitions Reference

ObjectiveIron で利用可能な全 Definition クラスの一覧です。

## 基本パターン

すべてのDefinitionは以下のパターンに従います：

1. 抽象クラスを継承
2. `abstract` プロパティをオーバーライド（ID、名前等）
3. `virtual` メソッドをオーバーライドして内容を定義（Scope系）
4. `ModProject.Add*()` で登録
5. `Emit()` で出力

## 一覧

### コンテンツ系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `FocusTreeDefinition` | `common/national_focus/` | Id, Country, Structure() |
| `FocusDefinition` | (ツリーに含まれる) | Id, Title, Icon, Position, Cost |
| `EventDefinition` | `events/` | Id, Type, Title, Description, Options() |
| `DecisionCategoryDefinition` | `common/decisions/categories/` | Id, Name, Icon |
| `DecisionDefinition` | `common/decisions/` | Id, Name, Cost, DaysRemove |
| `IdeaDefinition` | `common/ideas/` | Id, Category, Name, Modifier() |
| `CharacterDefinition` | `common/characters/` | Id, Name, Country, Roles |
| `TechnologyDefinition` | `common/technologies/` | Id, Name, Category, ResearchCost |

### 国家・ヒストリー系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `CountryDefinition` | `common/country_tags/` + `common/countries/` | Tag, Color, GraphicalCulture |
| `CountryHistoryDefinition` | `history/countries/` | Tag, Capital, Oob, Politics(), Technologies() |
| `StateDefinition` | `history/states/` | Id, Manpower, Provinces, History() |
| `OobDefinition` | `history/units/` | FileName, Templates(), Units() |
| `NavalOobDefinition` | `history/units/` | FileName, Fleets() |
| `AirOobDefinition` | `history/units/` | FileName, AirWings() |

### 軍事・装備系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `SubUnitDefinition` | `common/units/` | FileName, Define() |
| `EquipmentDefinition` | `common/units/equipment/` | FileName, Define() |

### 政治・外交系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `IdeologyDefinition` | `common/ideologies/` | FileName, Define() |
| `OccupationLawDefinition` | `common/occupation_laws/` | FileName, Define() |
| `WargoalDefinition` | `common/wargoals/` | FileName, Define() |
| `OperationDefinition` | `common/operations/` | FileName, Define() |
| `AbilityDefinition` | `common/abilities/` | FileName, Define() |
| `AiStrategyDefinition` | `common/ai_strategy/` | FileName, Define() |

### インフラ系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `BuildingDefinition` | `common/buildings/` | FileName, Define() |
| `StateCategoryDefinition` | `common/state_category/` | FileName, Define() |
| `StaticModifierDefinition` | `common/static_modifiers/` | FileName, Define() |
| `GameRuleDefinition` | `common/game_rules/` | FileName, Define() |
| `TerrainDefinition` | `common/terrain/` | FileName, Define() |
| `ResourceDefinition` | `common/resources/` | FileName, Define() |
| `TechSharingDefinition` | `common/technology_sharing/` | FileName, Define() |
| `AutonomyStateDefinition` | `common/autonomous_states/` | FileName, Define() |
| `DifficultySettingDefinition` | `common/difficulty_settings/` | FileName, Define() |
| `NameDefinition` | `common/names/` | FileName, Define() |

### DLC系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `MioDefinition` | `common/military_industrial_organization/` | FileName, Define() |
| `IntelligenceAgencyDefinition` | `common/intelligence_agencies/` | FileName, Define() |
| `PeaceConferenceDefinition` | `common/peace_conference/` | FileName, Define() |
| `BalanceOfPowerDefinition` | `common/balance_of_power/` | FileName, Define() |
| `ScriptedDiplomaticActionDefinition` | `common/scripted_diplomatic_actions/` | FileName, Define() |
| `ScriptedGuiDefinition` | `common/scripted_guis/` | FileName, Define() |

### UI・メディア系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `SpriteDefinition` | `interface/*.gfx` | Name, TextureFile (or SourceFile) |
| `GuiDefinition` | `interface/*.gui` | Id, Define() |
| `MusicDefinition` | `music/` | FileName, Define() |
| `SoundDefinition` | `sound/*.asset` | FileName, Define() |

### マップ系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `StrategicRegionDefinition` | `map/strategicregions/` | Id, Define() |
| `SupplyAreaDefinition` | `map/supplyareas/` | Id, Define() |
| `DefaultMapDefinition` | `map/default.map` | MaxProvinces, SeaStarts, Continents() |
| `ProvinceDefinition` | `map/definition.csv` | Id, R, G, B, Type, Terrain |
| `AdjacencyDefinition` | `map/adjacencies.csv` | From, To, Type, Through |

### ローカライゼーション系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `LocalisationReplaceDefinition` | `localisation/replace/` | Id, Define() |

### スクリプト系

| Definition | 出力先 | 主要プロパティ |
|---|---|---|
| `ScriptedTriggerDefinition` | `common/scripted_triggers/` | Id, Define() |
| `ScriptedEffectDefinition` | `common/scripted_effects/` | Id, Define() |
| `OnActionDefinition` | `common/on_actions/` | FileName, Define() |
| `OpinionModifierDefinition` | `common/opinion_modifiers/` | FileName, Define() |
| `DynamicModifierDefinition` | `common/dynamic_modifiers/` | Id, Define() |
| `BookmarkDefinition` | `common/bookmarks/` | Id, Define() |
| `ContinuousFocusDefinition` | `common/continuous_focus/` | Id, Define() |
| `CountryLeaderTraitDefinition` | `common/country_leader/` | FileName, Define() |
| `UnitLeaderTraitDefinition` | `common/unit_leader/` | FileName, Define() |

### その他

| API | 説明 |
|---|---|
| `RawAsset` | 静的ファイルコピー（画像、音声等） |
| `AddRawAssetDirectory()` | ディレクトリ丸ごとコピー |
| `AddRawAsset(source, target)` | 個別ファイルコピー |

## Scope API

| Scope | 用途 | メソッド数 |
|---|---|---|
| `EffectScope` | ゲーム効果 (add_political_power等) | 90+ |
| `TriggerScope` | 条件判定 (has_war, tag等) | 80+ |
| `ModifierScope` | 数値修正 (stability_factor等) | 70+ |
| `AiScope` | AI重み付け | 6 |

すべてのScopeに `Custom(key, value)` と `CustomBlock(key, configure)` エスケープハッチがあります。
