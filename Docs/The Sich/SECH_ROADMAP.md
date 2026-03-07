# Сечь — Roadmap и Technical Debt

---

## Часть 1: Фичи для добавления

### 🟢 Простые (низкий риск, 1-2 файла)

| Фича | Что делать | Файлы | Риск |
|------|-----------|--------|------|
| **Новый враг** | Создать `EnemyData.asset`, добавить в `WaveDataSO.possibleEnemies` | 1-2 | Минимальный |
| **Новое оружие** | Создать `WeaponData.asset`, унаследовать `WeaponController` | 2 | Минимальный |
| **Новый босс** | Создать `EnemyData`, добавить в `WaveSystem.bossEnemies` | 1-2 | Минимальный |
| **Локализация** | Создать JSON в `Resources/Localization/ru.json` и т.д. | 1 | Нулевой |
| **Звуки оружия** | Добавить поле `AudioClip` в `WeaponData`, воспроизвести в `GeneratorWeapon` | 2 | Низкий |

### 🟡 Средние (3-5 файлов)

| Фича | Что делать | Файлы | Риск |
|------|-----------|--------|------|
| **Система ачивок** | Расширить `SavesYG`, заполнить `AchiveShop`, триггеры в `GameManager` | 3-4 | Средний |
| **Новый лут** | Создать `Crystal : MonoBehaviour`, `SpawnerCrystal`, интегрировать в `TilemapChunkManager` | 3-4 | Средний |
| **Таблица лидеров** | Использовать Yandex Games Leaderboard API | 2-3 | Средний |

### 🔴 Сложные (высокий риск)

| Фича | Что делать | Файлы | Риск |
|------|-----------|--------|------|
| **Комбо-система** | Добавить таймер в `GameManager`, множитель урона, UI | 3+ | Высокий |
| **Ежедневные награды** | Добавить даты в `SavesYG`, проверять при старте | 3+ | Средний |

---

## Часть 2: Исправить НЕМЕДЛЕННО (критично)

### 🔴 TilemapChunkManager.Update — аллокации

**Проблемы:**
- `new Vector2Int()` в цикле (строка 95)
- `new List<Vector2Int>()` каждый кадр (строка 104)
- `GetComponentInChildren` для каждого чанка (строка 135)

**Решение:**
- Кешировать playerChunk, пересчитывать только при движении игрока
- Переиспользовать List<Vector2Int>
- Кешировать компонент Tilemap

---

### 🔴 VampireSkill.Update — Physics2D каждый кадр

**Проблемы:**
- `Physics2D.OverlapCircleAll()` вызывается каждый кадр (строка 61)
- Создаёт новый массив Collider2D[]

**Решение:**
- Вызывать только при активации UI
- Или использовать таймер (раз в 0.5 сек)

---

### 🔴 EnemyMovement.Update — 500+ вызовов

**Проблемы:**
- Update вызывается для КАЖДОГО врага (до 500!)
- 500 × 60 FPS = 30,000+ вызовов в секунду

**Решение:**
- Batch обновление через EnemiesGenerator
- Или переписать на ECS

---

## Часть 3: Исправить В БЛИЖАЙШЕМ ВРЕМЕНИ

### 🟠 PlayerMovement — аллокации в FixedUpdate

**Проблемы:**
- `new Vector2()` дважды за вызов (строки 56, 60)
- `new Vector3()` в Update каждый кадр (строки 91, 96)

**Решение:**
- Переиспользовать Vector2 / Vector3
- Использовать константы

---

### 🟠 Timer.Update — string.Format каждый кадр

**Проблемы:**
- Форматирование строки каждый кадр
- LocalizationManager вызывается постоянно

**Решение:**
- Обновлять текст только при изменении минут/секунд

---

### 🟠 GameManager — слишком много ответственностей

**Проблемы:**
- Создание игрока
- Инициализация всех систем
- Монеты, реклама, сохранение

**Рефакторинг:**
```
GameManager
├── PlayerCreator   → создание персонажа
├── CoinManager     → монеты
└── RewardAdsHandler → реклама
```

---

### 🟠 TilemapChunkManager — God Object

**Проблемы:**
- 5+ ответственностей в одном классе

**Рефакторинг:**
```
TilemapChunkManager
├── ChunkPoolManager   → пул чанков
├── BiomeSelector      → выбор биома
├── TileGenerator     → генерация тайлов
└── DecorationSpawner → спавн декораций
```

---

### 🟠 CharacterShop — God Object

**Проблемы:**
- UI + логика покупки + анимация + сохранение

**Рефакторинг:**
```
CharacterShop
├── ShopUIController    → только UI
├── CharacterPurchaser  → логика покупки
└── CarouselAnimator   → DOTween анимация
```

---

### 🟠 UIManager — God Object

**Проблемы:**
- 155 строк, делает всё: HUD + события + анимации + сцены

**Рефакторинг:**
```
UIManager
├── HUDController       → счёт, уровень, монеты
├── LevelUpController   → выбор оружия
└── MenuController     → сцены, пауза
```

---

## Часть 4: Рекомендации

### С чего начать

1. **Немедленно:** Исправить аллокации в TilemapChunkManager (самый критичный)
2. **Потом:** Убрать Physics2D из VampireSkill.Update
3. **Параллельно:** Добавить первое новое оружие/врага (тренировка)

### Порядок рефакторинга

1. TilemapChunkManager → EnemiesGenerator → GameManager → UIManager → CharacterShop

### Когда делать

- TilemapChunkManager — до добавления новых фич карты
- EnemyMovement — до увеличения лимита врагов
- GameManager — перед добавлением новых систем (ачивки, комбо)

---

## Текущее состояние проекта

### Основные скрипты (нужные исправлений)

| Скрипт | Строк | Проблемы |
|--------|-------|-----------|
| TilemapChunkManager | 317 | Аллокации в Update |
| EnemyMovement | ~150 | Вызывается 500+ раз |
| PlayerMovement | ~175 | Аллокации |
| Timer | ~50 | string.Format каждый кадр |
| VampireSkill | ~80 | Physics2D каждый кадр |
| UIMenuManager | 348 | God Object |

### Скрипты UI

| Скрипт | Назначение |
|--------|-----------|
| UIMenuManager | Главное меню |
| ShopNavigation | Навигация по магазину (нужно выделить) |
| StatsPanelController | Статистика (нужно выделить) |
| PlatformConfigurator | Адаптация ПК/мобильная (нужно выделить) |
| LocationShopUI | Магазин локаций |
| LocationCardUI | Карточка локации |

### Системы

| Система | Файлы |
|---------|-------|
| Оружие | Weapon.cs, WeaponController.cs, GeneratorWeapon.cs, ManagerWeapon.cs |
| Враги | Enemy.cs, EnemyMovement.cs, EnemiesGenerator.cs, WaveSystem.cs |
| Игрок | Player.cs, PlayerMovement.cs, PlayerHealth.cs, PlayerWeapons.cs |
| Читы | SavesYG.cs, GameDataManager.cs |
| Локации | TilemapChunkManager.cs, BiomeData.cs |

---

*Обновлено: Март 2026*
