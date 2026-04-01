# Unity Engine Reference - Version 6.3 LTS

## Overview
Unity 6.3 LTS (Long Term Support) is the current stable version used for the Beacon project. This version provides enhanced performance, improved graphics capabilities, and long-term stability for game development.

## Key Features Used in This Project

### Entity Component System (ECS) Integration
- Project uses Leopotam.EcsLite for ECS architecture alongside traditional MonoBehaviour components
- ECS is in early stages - existing MonoBehaviour components take priority
- When expanding ECS: components in `Ecs/PlayerComponents.cs`, systems in separate files

### ScriptableObject Architecture
All game entity data is stored in ScriptableObjects:
- CharacterData - player character statistics and abilities
- WeaponData - weapon attributes and behaviors  
- EnemyData - enemy stats and prefab references
- WaveDataSO - wave configuration and enemy spawn patterns
- BiomeData - map biome properties
- LeshyData - specialized enemy data for Leshy boss

### Dependency Injection (Zenject)
- All dependencies registered in ProjectInstaller.cs
- Uses FromComponentInNewPrefab, FromComponentInHierarchy, AsSingle() patterns
- No manual singleton creation
- Interfaces for all services (ISoundManager, etc.)

### Addressable Assets System
- Async asset loading for better memory management
- Scene and prefab addressing for efficient bundling
- Remote content delivery support

### Unity Specific Optimizations
- Object pooling through EnemiesGenerator for enemies
- GeneratorWeapon for projectile pooling
- UI text object reuse rather than instantiation
- Component caching in Awake() to avoid GetComponent in loops
- Physics2D optimization notes in technical debt

## Supported Platforms
- Primary: WebGL (Yandex Games)
- Desktop: Windows, macOS, Linux (through Unity builds)
- Mobile: iOS, Android (planned)

## Package Dependencies
- Leopotam.EcsLite - ECS framework
- Zenject - Dependency injection
- Addressables - Asset management system
- Various Unity Registry packages for 2D rendering, input, etc.

## Known Limitations
- ECS integration incomplete - use MonoBehaviour for new gameplay features
- WebGL build size optimization needed
- Some editor-only tools may not work in builds