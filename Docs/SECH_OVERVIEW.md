# Сечь — Проект Unity

## Жанр

**Survival Roguelite** с видом сверху (top-down). Аналоги: Vampire Survivors, Brotato.

---

## Технологический стек

- **Unity 2022+** с URP (Universal Render Pipeline)
- **Zenject** — Dependency Injection
- **Leopotam.EcsLite** — ECS (частично, в зачатке)
- **Yandex Games SDK (YG2)** — платформа, сохранения, реклама
- **Cinemachine** — камера
- **DOTween** — анимации UI

---

## Архитектура

### Паттерны

- **DI (Zenject)**: `ProjectInstaller.cs`
- **ECS (Leopotam)**: `EcsStartup.cs`, компоненты в `PlayerComponents.cs`
- **ScriptableObject**: `CharacterData`, `WeaponData`, `WaveDataSO`, `EnemyData`, `BiomeData`
- **Singleton**: `DeviceDetector`, `GameManager`, `GameDataManager`, `UIManager`, `LocalizationManager`

### Определение устройства

- **DeviceDetector** (`Core/DeviceDetector.cs`) — синглтон для определения платформы:
  - `IsMobile` — мобильное устройство (Android/iOS/WebGL Mobile)
  - `IsEditor` — запуск в редакторе
  - `Platform` — RuntimePlatform
  - `IsTouchDevice()` — поддержка тачскрина
  - `IsWebGL()` — запущено в браузере

### Основные папки

```
Assets/Scripts/
├── Core/           # GameManager, GameDataManager, SoundManager, DeviceDetector
├── Player/         # Игрок: движение, здоровье, оружие, уровни
├── Enemy/          # Враги, волны, генерация
├── Weapon/         # Система оружия
├── UI/             # Интерфейс
├── Chunks/         # Генерация карты (Tilemap, биомы)
├── Ecs/            # ECS-компоненты
└── Zenject/       # DI-конфигурация
```

---

## Ключевые системы

| Система | Описание |
|---------|----------|
| **Бой** | Автоматические атаки, оружие с уровнями, урон по площади |
| **Враги** | Волновая система, пул объектов, боссы |
| **Лут** | Монеты, звёзды опыта, зелья |
| **Прогресс** | Система XP/уровней, магазин персонажей |
| **UI** | HUD, меню, пауза, локализация |
| **Сохранения** | Yandex Games Cloud (монеты, прогресс, персонажи) |
| **Карта** | Бесконечные чанки, биомы, декорации |

---

## Поток данных игрока

### 1. Ввод → Движение

```
FixedJoystick / Input.GetAxisRaw()
        ↓
PlayerMovement.FixedUpdate()
        ↓
_rigidbody2D.linearVelocity = _input * _speed
```

### 2. Атака

```
WeaponController.FireLoop()
        ↓
generator.SpawnProjectilesBurst()
        ↓
EnemyAttacked.OnCollisionEnter2D()
        ↓
EnemyHealth.TakeDamage() → Died
        ↓
EnemiesGenerator.ReturnEnemyToPool()
        ↓
OneKill?.Invoke() → счётчик убийств
```

### 3. Опыт и уровни

```
Enemy умирает → SpawnExperienceStar()
        ↓
Star.OnTriggerEnter2D()
        ↓
PlayerLevelManager.AddProgress()
        ↓
_level++ → LevelUp?.Invoke()
```

### 4. Сохранение

```
GameDataManager → YG2.SaveProgress()
        ↓
YG2.saves.bestScore, bestLevel, coins, totalKill, selectedCharacter
```

---

## Цепочка спавна врагов

```
GameManager.InitializeGame()
        ↓
WaveSystem.StartWave() → WaveSystem.WaveTimer() (цикл)
        ↓
WaveSystem.SpawnRandomEnemy() / SpawnBoss()
        ↓
EnemiesGenerator.SpawnEnemyWithModifiers()
        ↓
Enemy.Initialize() → EnemyMovement, EnemyHealth, EnemyAttacked
        ↓
При смерти: EnemyAnimation.DieWithDissolve()
        ↓
Enemy.OnRelease() → EnemiesGenerator.ReturnEnemyToPool()
        ↓
OneKill?.Invoke() + SpawnExperienceStar()
```

---

## Архитектура UI-слоя

### Текущая реализация: Смешанная (monolitUI + events)

Чёткого MVC/MVVM/MVP нет. Всё смешано в MonoBehaviour-классы.

### UI-компоненты

| Класс | Ответственность |
|-------|----------------|
| **UIManager** | Главный HUD: счёт, уровень, монеты, кнопки |
| **UIGameOverManager** | Экран смерти |
| **UIWeaponManager** | Панель выбора оружия при Level Up |
| **PauseMenuManager** | Меню паузы |
| **UIStats** | Отображение статистики |
| **LocalizationManager** | Локализация (Singleton) |
| **ButtonManager** | Ховер-эффекты кнопок |
| **CharacterCardUI** | Карточка персонажа (data container) |
| **UIMenuManager** | Главное меню, адаптация под устройство |

### Адаптивный UI (ПК vs Мобильный)

Определение устройства через `DeviceDetector`:
- **ПК**: полное меню с кнопками (Stats, Shop, Achive)
- **Мобильный**: одна кнопка магазина + стрелочки для переключения вкладок

См. `UIMenuManager.cs`:
- `_pcMenuPanel` — панель кнопок для ПК
- `_mobileBackgroundImage` — фоновая картинка для мобильных
- `_mobileShopPanel`, `_mobileShopTabButton`, `_mobilePrevTabButton`, `_mobileNextTabButton` — мобильный магазин с каруселью

### Связь с геймплеем

```
                    ┌─────────────────────┐
                    │    GameManager     │
                    │   (синглтон)       │
                    └─────────┬───────────┘
                              │
          ┌───────────────────┼───────────────────┐
          ▼                   ▼                   ▼
   ┌──────────────┐   ┌──────────────┐   ┌──────────────┐
   │ UIManager    │   │ WaveSystem   │   │GameDataManager│
   │ (подписка)   │   │              │   │ (сохранения)  │
   └──────┬───────┘   └──────┬───────┘   └──────────────┘
          │
          │    События:
          │  - OnAddCoin
          │  - PlayerRaist
          │  - OneKill
          │  - LevelUp
          ▼
   ┌──────────────────────────────────────────────────┐
   │              UI (Text, Button, Image)              │
   └──────────────────────────────────────────────────┘
```

### Используемые паттерны

- **Singleton**: `UIManager.Instance`, `LocalizationManager.Instance`
- **Event-Driven**: подписка на события `OnAddCoin`, `LevelUp`, `OneKill`
- **Observer**: подписка `+=` / отписка `-=` в OnEnable/OnDisable

### Проблемы

- **UIManager** делает слишком много: HUD + события + анимации + сцены
- Прямая связь UI ↔ GameManager
- НетSeparation of Concerns

См. также `SECH_RULES.md` раздел 7.

---

## Roadmap и Technical Debt

Полный план фич, технический долг и приоритеты — см. `SECH_ROADMAP.md`

Кратко:
- **TilemapChunkManager** — аллокации в Update (критично)
- **EnemyMovement** — 500+ Update вызовов
- **GameManager** — God Object

---

## Контакты

- Основной разработчик: Илья
- Платформа: Yandex Games (WebGL)
- Unity Version: 2022+
