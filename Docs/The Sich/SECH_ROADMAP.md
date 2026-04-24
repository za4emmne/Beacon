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

### ✅ ИСПРАВЛЕНО:TilemapChunkManager.Update — аллокации

**Проблемы (были):**
- `new Vector2Int()` в цикле (строка 95) - ИСПРАВЛЕНО
- `new List<Vector2Int>()` каждый кадр (строка 104) - ИСПРАВЛЕНО (переиспользуемые списки)
- `GetComponentInChildren` для каждого чанка - ИСПРАВЛЕНО (кеширование)

**Решение применено:**
- ✅ Пересчитывать чанки только при движении игрока (кеширование _lastPlayerChunk)
- ✅ Переиспользовать List<Vector2Int> (_chunksToCreate, _chunksToRemove)

---

### ✅ ИСПРАВЛЕНО: EnemyMovement.Update — 500+ вызовов

**Проблемы (были):**
- Update вызывается для КАЖДОГО врага (до 500!)
- 500 × 60 FPS = 30,000+ вызовов в секунду

**Решение применено:**
- ✅ Добавлен интервал обновления (_updateInterval = 0.1f)
- ✅ Update вызывается раз в 100мс вместо каждого кадра

---

## Часть 3: Исправить В БЛИЖАЙШЕМ ВРЕМЕНИ

### ✅ ИСПРАВЛЕНО: PlayerMovement — аллокации в FixedUpdate

**Проблемы (были):**
- `new Vector2()` дважды за вызов
- `new Vector3()` в Update каждый кадр

**Решение применено:**
- Переиспользовать Vector2 / Vector3 - см. PlayerMovement.cs

---

### ✅ ИСПРАВЛЕНО: Timer.Update — string.Format каждый кадр

**Проблемы (были):**
- Форматирование строки каждый кадр
- LocalizationManager вызывается постоянно

**Решение применено:**
- ✅ Обновлять текст только при изменении минут/секунд (_lastMinutes, _lastSeconds)
- ✅ Оптимизирован Update()

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

1. ~~**Немедленно:** Исправить аллокации в TilemapChunkManager~~ - ✅ ИСПРАВЛЕНО
2. ~~**Потом:** Убрать Physics2D из VampireSkill.Update~~ - Требует проверки наличия файла
3. **Параллельно:** Добавить первое новое оружие/врага (тренировка)

### Порядок рефакторинга

1. TilemapChunkManager ✅ → EnemiesGenerator → GameManager → UIManager → CharacterShop

### Когда делать

- ~~TilemapChunkManager — до добавления новых фич карты~~ - ✅ ИСПРАВЛЕНО
- EnemyMovement — до увеличения лимита врагов - ✅ ИСПРАВЛЕНО
- GameManager — перед добавлением новых систем (ачивки, комбо)

---

## Текущее состояние проекта

### ✅ Основные скрипты (исправлены)

| Скрипт | Строк | Статус |
|--------|-------|--------|
| TilemapChunkManager | 317 | ✅ Исправлено (аллокации) |
| EnemyMovement | ~145 | ✅ Исправлено (интервал) |
| Timer | ~70 | ✅ Исправлено (форматирование) |
| PlayerMovement | ~175 | ✅ Исправлено (аллокации) |
| VampireSkill | ~80 | ⚠️ Файл не найден |
| UIMenuManager | 348 | 🟡 Требует рефакторинга |

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

### Дополнительно исправлено в Апреле 2026:

| Проблема | Файл | Статус |
|----------|------|--------|
| Леший не создавал летучих мышей | Leshy.cs, EnemiesGenerator.cs | ✅ Исправлено |
| Перезапуск сцены | UIManager.cs, GameManager.cs | ✅ Исправлено |
| Очистка синглтонов при перезагрузке | Все менеджеры | ✅ Исправлено |

---

*Обновлено: Апрель 2026*
