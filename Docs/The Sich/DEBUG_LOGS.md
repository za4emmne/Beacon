# Debug Логи - Система Инициализации

## Файл: InitDebug.cs
```
// Глобальный флаг включения/выключения логов
public static class InitDebug
{
    public static bool Enabled = true; // Изменить на false для отключения
}
```

---

## Префиксы логов

| Префикс | Значение |
|---------|----------|
| `[INIT]` | Инициализация (Awake, Start, OnEnable) |
| `[RESTART]` | Перезапуск сцены |
| `[EVENT]` | Подписка/отписка от событий |
| `[SPAWN]` | Спавн объектов (игрок, враги) |
| `[POOL]` | Работа с пулами объектов |
| `[WAVE]` | Волновая система |
| `[UI]` | UI события |

---

## Порядок инициализации при ПЕРВОМ запуске сцены Game:

### Этап 1: Загрузка сцены
1. Unity загружает сцену Game
2. Создаются все объекты со статическими Awake()

### Этап 2: Awake (в порядке зависимостей)
1. `GameDataManager.Awake()` - если Instance == null, становится синглтоном, DontDestroyOnLoad, LoadData()
2. `GameManager.Awake()` - становится Instance, сбрасывает _initialized=false
3. `UIManager.Awake()` - становится Instance
4. `EnemiesGenerator.Awake()` - инициализирует пустые пулы
5. `WaveSystem.Awake()` - пре-вычисляет множители
6. `Player.Awake()` - становится singleton

### Этап 3: OnEnable
1. `GameManager.OnEnable()` - подписывается на _enemyManager.OneKill
2. `UIManager.OnEnable()` - подписывается на события
3. `EnemiesGenerator.OnEnable()` - очищает пулы и AllEnemies
4. `WaveSystem.OnEnable()` - сбрасывает счётчики волн

### Этап 4: Start (в порядке зависимостей)
1. `GameManager.Start()` - запускает InitializeRoutine()
2. `GameDataManager.Start()` - если нули - LoadData()
3. `UIManager.Start()` - регистрирует кнопки
4. `Player.Start()` - инициализирует здоровье

### Этап 5: InitializeRoutine (GameManager корутина)
1. Ждёт GameDataManager.Instance != null
2. Вызывает CreatePlayer()
   - Спавнит Player префаб
   - Player.Awake() устанавливает singleton
3. Запускает InitializeGame()
   - Настраивает Follower на Player
   - Инициализирует WaveSystem и EnemiesGenerator
   - Запускает волны через _waveSystem.StartWave()

---

## Порядок инициализации при РЕСТАРТЕ сцены Game:

### Этап 1: Нажатие кнопки "Повторить"
1. `UIManager.RestartScene()` вызывается
2. Time.timeScale = 1f
3. SceneManager.LoadScene("Game")

### Этап 2: Выгрузка старой сцены
1. Вызываются OnDisable() на всех объектах
2. Вызываются OnDestroy() на всех объектах
3. Сбрасывается состояние не-DontDestroyOnLoad объектов

### Этап 3: Новая сцена (аналогично первому запуску)
1. GameDataManager НЕ пересоздаётся (это DontDestroyOnLoad!)
   - Но Awake() не вызывается повторно если Instance уже существует
2. Все остальные объекты создаются заново

---

## ПОТЕНЦИАЛЬНЫЕ ПРОБЛЕМНЫЕ МЕСТА:

### 1. GameDataManager - DontDestroyOnLoad
- **Проблема**: При перезагрузке сцены GameDataManager не пересоздаётся, но может сохранять старое состояние между запусками
- **Решение**: Вызов ResetRunData() перед перезагрузкой (добавлено в GameDataManager)

### 2. Статические списки - EnemiesGenerator.AllEnemies
- **Проблема**: Статический List не очищается автоматически
- **Решение**: Очистка в OnEnable() и OnDisable()

### 3. Подписки на события - UIManager.OnDisable
- **Проблема**: В оригинальном коде была ошибка: += вместо -= для OnShowTooltip
- **Решение**: Исправлено в новой версии

### 4. Player.singleton - может быть null
- **Проблема**: При инициализации WaveSystem до создания Player
- **Решение**: Добавлены проверки в InitializeGame()

### 5. Корутины - WaveSystem
- **Проблема**: При перезагрузке старая корутина может继续 работать
- **Решение**: Остановка в OnDisable()

---

## Как использовать логи:

1. Включить логи: `InitDebug.Enabled = true;` в InitDebug.cs
2. Запустить игру с чистой консолью
3. Запустить первый раз - записать порядок логов
4. Умереть и нажать "Повторить"
5. Сравнить порядок логов первого и второго запуска
6. Найти место где порядок отличается

---

## Ключевые идентификаторы для поиска проблем:
- `[INIT][GameManager] Instance=` - проверка дубликатов
- `[INIT][Player] singleton=` - проверка создания игрока
- `[SPAWN][EnemiesGenerator] Spawned` - проверка появления врагов
- `[EVENT]` - проверка подписок
- `Duplicate` - поиск дубликатов синглтонов
- `NULL` - поиск null ссылок