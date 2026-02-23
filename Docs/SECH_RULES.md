# Правила разработки — Сечь

---

## 1. Архитектурные решения

### 1.1 Dependency Injection (Zenject)

- Все зависимости регистрировать в `ProjectInstaller.cs`
- Использовать `FromComponentInNewPrefab`, `FromComponentInHierarchy`, `AsSingle()`
- НЕ создавать синглоны вручную через `new Singleton()`
- Интерфейсы для всех сервисов: `ISoundManager`, etc.

### 1.2 ECS (Leopotam.EcsLite)

- ECS пока в зачатке — НЕ ломать существующие компоненты
- При расширении ECS: компоненты в `Ecs/PlayerComponents.cs`, системы в отдельных файлах
- Обычный код (MonoBehaviour) имеет приоритет

### 1.3 ScriptableObject

- ВСЕ данные игровых сущностей — через ScriptableObject:
  - `CharacterData` — персонажи
  - `WeaponData` — оружие
  - `EnemyData` — враги
  - `WaveDataSO` — волны
  - `BiomeData` — биомы карты
- НЕ хардкодить数值 в коде

---

## 2. Структура папок

```
Assets/Scripts/
├── Core/           # GameManager, GameDataManager, SoundManager, DeviceDetector
├── Player/         # Компоненты игрока
├── Enemy/          # Враги, волны, генерация
├── Weapon/         # Оружие
├── UI/             # Интерфейс
├── Chunks/         # Генерация мира
├── Ecs/            # ECS
├── Zenject/        # DI
├── Star/, Coin/, Pills/, Loots/  # Лут
├── Upgraid/        # Улучшения
└── ProgressBar/, HealthBar/       # UI компоненты
```

---

## 3. Требования к неймингу

| Тип | Префикс/Суффикс | Пример |
|-----|------------------|--------|
| Класс | UpperCamelCase | `PlayerMovement` |
| Метод | UpperCamelCase | `TakeDamage()` |
| Приватное поле | `_camelCase` | `_health` |
| Публичное поле | camelCase | `currentHealth` |
| Константа | UPPER_SNAKE | `WAVE_DURATION` |
| Интерфейс | I + UpperCamelCase | `ISoundManager` |
| Scriptable/SObject | DataO + Name | `CharacterData` |
| UI компонент | Тип + Name | `ScoreText`, `RestartButton` |

---

## 4. Оптимизация

### 4.1 Объекты

- Враги — ТОЛЬКО через пул: `EnemiesGenerator.GetEnemyFromPool()`
- Снаряды — через `GeneratorWeapon`
- UI текст — переиспользовать, не создавать новые объекты

### 4.2 Паттерны

- Использовать `ObjectPool` для часто создаваемых объектов
- Не создавать объекты в `Update()` / `FixedUpdate()`
- Кешировать компоненты в `Awake()`, не использовать `GetComponent` в цикле

### 4.3 Лимиты

- Максимум 500 активных врагов (см. `EnemiesGenerator._maxPoolSize`)
- waveDuration = 60 секунд по умолчанию

---

## 5. Что НЕЛЬЗЯ ломать

### 5.1 Сохранения

- `GameDataManager` — синглтон, сохраняет в Yandex Games
- Поля: `bestScore`, `bestLevel`, `coins`, `totalKill`, `selectedCharacter`, `unlockedCharacters`
- Любое изменение структуры сохранений — только с миграцией

### 5.2 Волновая система

- `WaveSystem` — основа геймплея
- `EnemiesGenerator` — пул врагов
- НЕ удалять событие `OneKill` — от него зависит счёт

### 5.3 Жизненный цикл игрока

- `Player.singleton` — статический синглтон
- `PlayerLevelManager` — прогресс XP/уровней
- `PlayerHealth` — здоровье, смерть

### 5.4 UI

- `UIManager` — центральный UI менеджер
- `LocalizationManager` — все тексты идут через него
- НЕ хардкодить строки в UI

---

## 6. Работа с кодом

### 6.1 Коммиты

- Атомарные коммиты: одна фича = один коммит
- Сообщения: `[Feature] Add new weapon type`, `[Fix] Enemy spawn distance`

### 6.2 Тестирование

- После изменения `WaveSystem` — проверить спавн
- После изменения `GameDataManager` — проверить сохранение/загрузку

### 6.3 CI/CD

- Проверка компиляции перед пушем
- Теги: `v0.1.0`, `v0.2.0` и т.д.

---

## 7. UI Архитектура

### 7.1 Текущее состояние

- **Нет чёткого MVC/MVVM** — всё в MonoBehaviour
- **Event-Driven** — подписка на события геймплея
- **Singleton** — UIManager, LocalizationManager

### 7.2 Структура UI

```
UI/
├── UIManager              # Главный HUD (всё в одном!)
├── UIGameOverManager     # Экран смерти
├── UIWeaponManager       # Панель Level Up
├── PauseMenuManager      # Меню паузы
├── UIStats               # Статистика (хорошо)
├── LocalizationManager   # Локализация (хорошо)
├── ButtonManager         # Ховер-эффекты
└── CharacterCardUI      # Data container (хорошо)
```

### 7.3 Правила

- **НЕ создавать новые UI в Update()** — использовать пул
- **Все тексты через LocalizationManager** — не хардкодить
- **Подписка/отписка в OnEnable/OnDisable**
- **UI не должен напрямую управлять геймплеем** — только через GameManager

---

## 8. Technical Debt и Roadmap

Подробный план исправлений и список фич — см. `SECH_ROADMAP.md`

Кратко:
- **Немедленно:** Аллокации в TilemapChunkManager, Physics2D в VampireSkill
- **Скоро:** Рефакторинг GameManager, UIManager, CharacterShop
- **Фичи:** Новое оружие/враги — низкий риск, ачивки — средний

---

## 9. Контакты

- Основной разработчик: Илья
- Платформа: Yandex Games (WebGL)
- Unity Version: 2022+
