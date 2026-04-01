# Opencode Game Studios -- Game Studio Agent Architecture

Indie game development managed through 48 coordinated opencode subagents.
Each agent owns a specific domain, enforcing separation of concerns and quality.

## Technology Stack

- **Engine**: Unity 6.3 LTS
- **Language**: C#
- **Version Control**: Git with trunk-based development
- **Build System**: Unity's built-in build system
- **Asset Pipeline**: Unity's asset pipeline with Addressable Assets System

> **Note**: Engine-specialist agents exist for Godot, Unity, and Unreal with
> dedicated sub-specialists. Use the set matching your engine.

## Project Structure

This Unity 6.3 LTS project follows the standard Unity structure with custom organization:

```
/ (Project Root)
├── Assets/                     # All game assets, scripts, scenes, prefabs
│   ├── Animation/              # Animation clips
│   ├── Materials/              # Materials
│   ├── Music/                  # Audio files
│   ├── Prefabs/                # Prefab objects
│   │   └── Enemy/              # Enemy prefabs (BatSlavic, Leshy, etc.)
│   ├── Resources/              # Resources folder
│   ├── Scenes/                 # Unity scenes
│   ├── Scripts/                # C# game code
│   │   ├── Core/               # GameManager, GameDataManager, etc.
│   │   ├── Enemy/              # Enemy scripts and data
│   │   │   ├── Enemy.cs        # Base enemy class
│   │   │   ├── Leshy.cs        # Leshy enemy implementation
│   │   │   ├── LeshyData.cs    # Leshy data (ScriptableObject)
│   │   │   └── WaveSystem.cs   # Wave management
│   │   ├── Player/             # Player-related scripts
│   │   ├── Weapon/             # Weapon systems
│   │   └── UI/                 # User interface scripts
│   ├── Sprites/                # 2D sprites
│   │   └── Enemies/            # Enemy sprites (Bat, Leshy, etc.)
│   └── Settings/               # Game settings
├── Docs/                       # Design documents and references
├── ProjectSettings/            # Unity project settings
├── Packages/                   # Unity packages
└── UserSettings/               # User-specific settings
```

## Engine Version Reference

@docs/engine-reference/unity/VERSION.md

## Technical Preferences

@.claude/docs/technical-preferences.md

## Coordination Rules

@.claude/docs/coordination-rules.md

## Collaboration Protocol

**User-driven collaboration, not autonomous execution.**
Every task follows: **Question -> Options -> Decision -> Draft -> Approval**

- Agents MUST ask "May I write this to [filepath]?" before using Write/Edit tools
- Agents MUST show drafts or summaries before requesting approval
- Multi-file changes require explicit approval for the full changeset
- No commits without user instruction

See `docs/COLLABORATIVE-DESIGN-PRINCIPLE.md` for full protocol and examples.

> **First session?** If the project has no engine configured and no game concept,
> run `/start` to begin the guided onboarding flow.

## Coding Standards

@.claude/docs/coding-standards.md

## Context Management

@.claude/docs/context-management.md