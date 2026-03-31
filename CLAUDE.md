# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Project Is

ObjectiveIron is a C# transpiler for Hearts of Iron IV (HoI4) mods. It converts C# Fluent API definitions into Paradox Clausewitz engine native format (`.txt` files), providing type safety, IDE support, and validation for HoI4 modding.

## Commands

```bash
dotnet build                                          # Build entire solution
dotnet test                                           # Run all tests
dotnet test --project tests/ObjectiveIron.Core.Tests/ # Run a specific test project
dotnet run --project src/ObjectiveIron.Cli -- sample -o ./sample_output  # Generate sample output
dotnet run --project src/ObjectiveIron.Cli -- --help  # CLI help
```

## Architecture

The pipeline has four stages:

```
User C# Definition (FocusTreeDefinition subclass)
         ↓
   ModCompiler.Compile()        [Builders/Compiler/]
         ↓
Core Domain Models               [Core/Models/]
         ↓
Validator.Validate()             [Core/Validation/]
         ↓
Emitter.Emit() → .txt files      [Emitter/Emitters/]
```

### Projects and Their Roles

| Project | Role |
|---------|------|
| `ObjectiveIron.Core` | Domain models (`FocusTree`, `Focus`, `Event`, etc.) and validators. Zero external dependencies. |
| `ObjectiveIron.Builders` | Fluent API that users subclass. `FocusTreeDefinition` (abstract), scope builders (`EffectScope`, `TriggerScope`), type enums (`CountryTag`, `Ideology`, `Icons`), and compiler classes that translate definitions → Core models. |
| `ObjectiveIron.Emitter` | `ModProject` orchestrates all emitters. `ClausewitzWriter` handles low-level text formatting. Individual emitters per domain (focus tree, events, decisions, localisation, etc.). |
| `ObjectiveIron.Cli` | Entry point + sample definitions in `Sample/`. Shows intended usage patterns. |

### Key Design Patterns

**User-facing API:** Users subclass `FocusTreeDefinition`, override `Id`, `Country`, and `Structure(FocusGraph graph)`. Focuses are defined as properties (class instances), then connected via `graph.Root()`, `graph.Add().After()`, `.ExclusiveWith()`.

**Scope builders:** `EffectScope` and `TriggerScope` use a fluent/method-chaining pattern to build `Block` trees (the recursive primitive representing Clausewitz blocks). These are passed into `FocusDefinition` via `OnComplete`, `Trigger`, etc.

**`Block` / `Property` primitives** (`Core/Models/Primitives/`): The core AST. A `Block` has a key, child blocks, and properties. A `Property` is a key=value leaf. `ClausewitzWriter` walks these trees to produce output.

**`ModProject`** (`Emitter/ModProject.cs`): The single orchestrator for output. Callers `Add*` definitions to it, then call `Emit()` which invokes each emitter and writes files to the output directory. Returns `EmitResult`.

### Project Dependencies

```
Core ← Builders ← Emitter ← Cli
```

Core has no project references. Each layer only depends on layers to its left.

## Testing

Tests use xUnit with `[Fact]` attributes. The three test projects mirror the three non-CLI projects. Emitter tests typically assert on the string content of emitted output.
