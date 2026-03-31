# ObjectiveIron - 全HoI4機能実装計画

## 概要
HoI4の全モッダブル機能をC#で定義できるようにする。Phase A~Hの8フェーズ + 横断的Scope拡張。

---

## Phase A: Phase 4 統合（既存コードをModProjectに接続）
**既に定義・エミッター存在。ModProjectに未接続。**

| 機能 | Definition | Emitter | ModProject |
|------|-----------|---------|-----------|
| On Actions | OnActionDefinition.cs | OnActionEmitter.cs | 未接続 |
| Opinion Modifiers | OpinionModifierDefinition.cs | OpinionModifierEmitter.cs | 未接続 |
| Dynamic Modifiers | DynamicModifierDefinition.cs | DynamicModifierEmitter.cs | 未接続 |
| Country Tags | CountryDefinition.cs | CountryEmitter.cs | 未接続 |

作業内容:
- ModProject.cs: 4つのAdd/Emitブロック追加
- OnActionEmitter: 不要な外側ラッパー `on_actions = { }` 削除（HoI4形式ではトップレベル）
- OpinionModifierEmitter: 同上 `opinion_modifiers = { }` 削除
- LocalisationEmitter: CountryDefinition対応追加
- サンプル4つ新規作成

---

## Phase B: History ファイル
**プレイ可能なMODに必須。**

### B1. CountryHistoryDefinition
出力: `history/countries/{TAG} - {TAG}.txt`
```csharp
public class GermanyHistory : CountryHistoryDefinition
{
    public override string Tag => "GER";
    public override int Capital => 64;
    public override string Oob => "GER_1936";
    protected override void Politics(PoliticsScope p) {
        p.RulingParty("fascism");
        p.ElectionsAllowed(false);
    }
    protected override void Popularities(PopularityScope p) {
        p.Set("fascism", 95);
        p.Set("neutrality", 3);
    }
    protected override void StartingTech(TechSetScope t) {
        t.Add("infantry_weapons", "infantry_weapons1", "tech_recon");
    }
    protected override void StartingIdeas(IdeaSetScope i) => i.Add("GER_autarky");
    protected override void StartingEffects(EffectScope e) {
        e.RecruitCharacter("GER_adolf_hitler");
        e.SetFuelRatio(0.8);
    }
}
```

### B2. StateDefinition
出力: `history/states/{Id}-{Name}.txt`
```csharp
public class Corsica : StateDefinition
{
    public override int Id => 1;
    public override string Name => "STATE_1";
    public override int Manpower => 322900;
    public override string StateCategory => "town";
    public override int[] Provinces => [3838, 9851, 11804];
    protected override void History(StateHistoryScope h) {
        h.Owner("FRA");
        h.AddCoreOf("FRA");
        h.VictoryPoints(3838, 1);
        h.Buildings(b => { b.Infrastructure(2); b.IndustrialComplex(1); b.AirBase(1); });
    }
}
```

### B3. OobDefinition (Order of Battle)
出力: `history/units/{Tag}_{Year}.txt`
```csharp
public class GerOob : OobDefinition
{
    public override string Tag => "GER";
    public override int Year => 1936;
    protected override void Templates(TemplateScope t) {
        t.DivisionTemplate("Infanterie-Division", d => {
            d.Regiment("infantry", 0, 0);
            d.Regiment("infantry", 0, 1);
            d.Regiment("infantry", 0, 2);
            d.Support("engineer", 0, 0);
        });
    }
    protected override void Units(UnitPlacementScope u) {
        u.Division("Infanterie-Division", provinceId: 11467, experienceFactor: 0.3);
    }
}
```

---

## Phase C: ユニット・装備
**ゲームプレイの根幹。多くのMODで変更される。**

### C1. SubUnitDefinition
出力: `common/units/{id}.txt`
```csharp
public class Infantry : SubUnitDefinition
{
    public override string Id => "infantry";
    public override string Abbreviation => "INF";
    public override string MapIconCategory => "infantry";
    public override string Group => "infantry";
    public override double CombatWidth => 2;
    public override double MaxStrength => 25;
    public override int Manpower => 1000;
    protected override void Types(ListScope t) => t.Add("infantry");
    protected override void Categories(ListScope c) =>
        c.Add("category_front_line", "category_light_infantry", "category_army");
    protected override void Need(EquipmentNeedScope n) => n.Require("infantry_equipment", 100);
}
```

### C2. EquipmentDefinition
出力: `common/units/equipment/{id}.txt`
```csharp
public class InfantryEquipment : EquipmentDefinition
{
    public override string Id => "infantry_equipment";
    public override bool IsArchetype => true;
    public override string EquipmentType => "infantry";
    public override double Defense => 20;
    public override double Breakthrough => 3;
    public override double SoftAttack => 3;
    public override double BuildCostIc => 0.43;
}
```

---

## Phase D: 継続フォーカス・ブックマーク・AI戦略

### D1. ContinuousFocusPaletteDefinition
出力: `common/continuous_focus/{id}.txt`

### D2. BookmarkDefinition
出力: `common/bookmarks/{id}.txt`
ローカライズ: ブックマーク名・説明・国家説明

### D3. AiStrategyDefinition
出力: `common/ai_strategy/{id}.txt`

---

## Phase E: リーダー特性・占領法・戦争目標・諜報作戦

### E1. CountryLeaderTraitDefinition
出力: `common/country_leader/{id}.txt`

### E2. UnitLeaderTraitDefinition
出力: `common/unit_leader/{id}.txt`

### E3. OccupationLawDefinition
出力: `common/occupation_laws/{id}.txt`

### E4. WargoalDefinition
出力: `common/wargoals/{id}.txt`

### E5. OperationDefinition（諜報作戦）
出力: `common/operations/{id}.txt`

### E6. AbilityDefinition（リーダーアビリティ）
出力: `common/abilities/{id}.txt`

---

## Phase F: 建物・修正子・ゲームルール・地形・その他データ

### F1. BuildingDefinition
出力: `common/buildings/{id}.txt`

### F2. StaticModifierDefinition
出力: `common/modifiers/{id}.txt`

### F3. GameRuleDefinition
出力: `common/game_rules/{id}.txt`

### F4. TerrainDefinition
出力: `common/terrain/{id}.txt`

### F5. StateCategoryDefinition
出力: `common/state_category/{id}.txt`

### F6. NameGroupDefinition（国名リスト）
出力: `common/names/{id}.txt`

### F7. ResourceDefinition
出力: `common/resources/{id}.txt`

### F8. TechSharingGroupDefinition
出力: `common/technology_sharing/{id}.txt`

### F9. IdeologyDefinition
出力: `common/ideologies/{id}.txt`

### F10. AutonomyStateDefinition
出力: `common/autonomous_states/{id}.txt`

### F11. DifficultySettingDefinition
出力: `common/difficulty_settings/{id}.txt`

---

## Phase G: MIO・諜報機関・講和会議・BoP・外交アクション

### G1. MioDefinition（軍需企業）
出力: `common/military_industrial_organization/organizations/{id}.txt`
特性ツリー構造を持つ複雑な機能

### G2. IntelligenceAgencyDefinition
出力: `common/intelligence_agencies/{id}.txt`

### G3. IntelligenceAgencyUpgradeDefinition
出力: `common/intelligence_agency_upgrades/{id}.txt`

### G4. PeaceConferenceDefinition
出力: `common/peace_conference/{id}.txt`

### G5. BopDefinition（Balance of Power）
出力: `common/bop/{id}.txt`

### G6. ScriptedDiplomaticActionDefinition
出力: `common/scripted_diplomatic_actions/{id}.txt`

### G7. ScriptedGuiDefinition
出力: `common/scripted_guis/{id}.txt`

---

## Phase H: Interface・Music・Sound・GFX

### H1. GuiDefinition（.guiファイル）
出力: `interface/{id}.gui`
```csharp
public class MyCustomGui : GuiDefinition
{
    public override string Id => "my_custom_window";
    protected override void Define(GuiScope gui) {
        gui.ContainerWindow("main_window", w => {
            w.Position(100, 200);
            w.Size(400, 300);
            w.Button("ok_button", b => {
                b.Sprite("GFX_button_ok");
                b.Position(150, 250);
                b.ClickSound("ui_menu_close");
            });
            w.Icon("background", i => {
                i.Sprite("GFX_my_background");
                i.Position(0, 0);
            });
            w.Text("title_text", t => {
                t.Position(100, 10);
                t.Text("MY_WINDOW_TITLE");
                t.Font("hoi_24header");
            });
        });
    }
}
```

### H2. GfxSheetDefinition（.gfxファイル拡張）
既存のSpriteDefinitionを拡張:
- `frameAnimatedSpriteType` — アニメーションスプライト
- `progressbartype` — プログレスバー
- `textSpriteType` — テキストスプライト
- `corneredTileSpriteType` — 角タイルスプライト
- `maskedShieldType` — 紋章シールド

### H3. MusicDefinition
出力: `music/{mod_name}.txt`
```csharp
public class MyMusic : MusicDefinition
{
    public override string StationName => "my_mod_music";
    protected override void Songs(MusicScope m) {
        m.Song("my_theme", s => {
            s.Chance(c => c.Factor(1.0));
        });
        m.Song("peace_song", s => {
            s.Chance(c => {
                c.Factor(1.0);
                c.Modifier(1.5, t => t.Custom("has_war", "no"));
            });
        });
    }
}
```

### H4. SoundDefinition
出力: `sound/{mod_name}.asset`
```csharp
public class MySounds : SoundDefinition
{
    public override string Id => "my_mod_sounds";
    protected override void Define(SoundScope s) {
        s.Sound("my_click_sound", "gui/my_click.wav");
        s.Sound("my_event_sound", "gui/my_event.wav", alwaysLoad: false);
    }
}
```

### H5. MapDefinition（マップ関連）
出力: `map/` 以下の各種ファイル
- strategicregions, supply_nodes, railways 等
- 実際にはバイナリ/画像ファイルが多いため、テキストベースのもののみ対応

---

## 横断的: Scope拡張

### TriggerScope 追加メソッド（約40個）
**国家レベル:**
HasWarWith, HasWarTogether, IsAlly, IsPuppetOf, StrengthRatio, SurrenderProgress,
HasArmyManpower, HasEquipment, NumDivisions, HasArmySize, HasWarGoalAgainst,
FocusProgress, HasDlc, HasGlobalFlag, HasCountryFlag, HasStateFlag,
IsHistoricalFocusOn, Threat, NumOfResearchSlots, HasAvailableIdeaPower,
IsInFactionWith, HasArmyExperience, HasNavyExperience, HasAirExperience

**州レベル:**
IsControlledBy, IsOwnedBy, IsCoastal, Infrastructure, StatePopulation,
HasStateCategory, IsDemilitarizedZone, IsOwnedAndControlledBy

**変数:**
CheckVariable, HasVariable, SetTemp

### EffectScope 追加メソッド（約50個）
**国家レベル:**
SetCountryFlag, ClearCountryFlag, SetGlobalFlag, ClearGlobalFlag,
LoadOob, AddCommandPower, SetCapital, AddStateCore, RemoveStateCore,
SetVariable, AddToVariable, ClampVariable, SetRulingParty,
StartCivilWar, KillCountryLeader, AddThreat, AddNamedThreat,
AddToTechSharingGroup, SetCosmetic, DropCosmeticTag,
AddTimedIdea, ModifyTechSharingBonus, AddDynamicModifier, RemoveDynamicModifier,
AddToFaction, RemoveFromFaction, SetFuelRatio, AddFuel,
CreateEquipmentVariant, AddEquipmentProduction

**州レベル:**
SetStateFlag, ClearStateFlag, AddClaimBy, RemoveClaimBy,
AddCoreOf, RemoveCoreOf, SetDemilitarizedZone, SetCompliance,
SetResistance, AddResistanceTarget, DamageBuilding, AddBuilding

**スコープ切替:**
EveryState, RandomState, EveryCountry, EveryEnemy,
EveryOwnedState, RandomOwnedState, EveryNeighborState,
Root, From, Prev, Owner, Controller, Capital

### ModifierScope 追加メソッド（約30個）
Conscription, ConscriptionFactor, MaxCommandPower, CommandPowerGainMult,
ArmyOrgFactor, LandNightAttack, PlanningSpeedFactor, NavalBaseCapacity,
ProductionFactor, LocalBuildingSlots, MaxInfrastructure, LocalResourcesFactor,
JustifyWarGoalTime, GenerateWargoalTension, GuaranteeTime, VolunteersTension,
OperativeSlots, CivilianIntelligenceToOthers, DivisionOrganizationFactor,
ArmyAttackFactor, ArmyDefenceFactor, NavyOrgFactor, AirAccidentChance,
SubUnitDesignCostFactor, LicenseProductionSpeed, TradeOpinionFactor,
OffensiveWarStabilityFactor, DefensiveWarStabilityFactor,
OutOfSupplyFactor, AttritionForController, LocalManpower

---

## 実装順序

```
Phase A (既存統合)     ← まずここから
    ↓
Phase B (History)      ← プレイ可能MODに必須
    ↓
Phase C (Units/Equip)  ← ゲームプレイの根幹
    ↓
Phase D (ContFocus/BM/AI)
    ↓
Phase E (Traits/Laws/Wargoals/Ops)
    ↓
Phase F (データ定義群)
    ↓
Phase G (MIO/Intel/Peace/BoP/Diplo)
    ↓
Phase H (Interface/Music/Sound/GFX)

Scope拡張: 各フェーズと並行して必要に応じて追加
```

## 設計方針
- 全ての新Scopeクラスに `Custom(string key, object value)` エスケープハッチを設ける
- LocalisationEmitterのパラメータ爆発対策: `LocalisationContext` recordに集約
- ModProject.Emit()の肥大化対策: privateヘルパーメソッドに分割
- 各機能のDefinitionは既存パターン踏襲（abstract class + virtual override）
