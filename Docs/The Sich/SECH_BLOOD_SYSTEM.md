# Система крови (Blood System)

Система эффектов крови в игре "Сечь" состоит из четырёх основных компонентов:
1. **BloodParticles** — разлетающиеся частицы крови при ударе и смерти
2. **BloodDecal** — пятна крови на земле
3. **BloodSplatterManager** — менеджер пула пятен и процедурной генерации спрайтов
4. **BloodTrailEmitter** — следы крови, оставляемые при беге ранеными персонажами

---

## Настройка в Unity

### 1. BloodSplatterManager

Добавьте пустой GameObject в сцену и добавьте к нему компонент `BloodSplatterManager`.

#### Параметры:

| Параметр | Описание | По умолчанию |
|----------|----------|--------------|
| **Max Splatters** | Максимальное количество активных пятен крови на сцене | 50 |
| **Splatter Lifetime** | Время жизни пятна (сек) | 15 |
| **Fade Start Time** | Время начала затухания (сек) | 12 |
| **Splatter Size Min** | Минимальный размер пятна | 0.3 |
| **Splatter Size Max** | Максимальный размер пятна | 0.8 |
| **Pool Size** | Размер пула объектов | 60 |
| **Blood Sprites** | Массив спрайтов крови (если null — генерируются процедурно) | null |

#### Настройки следов (Trail Settings):

| Параметр | Описание | По умолчанию |
|----------|----------|--------------|
| **Trail Lifetime** | Время жизни следа крови | 8 |
| **Trail Fade Start** | Время начала затухания следа | 5 |
| **Trail Size Min** | Минимальный размер следа | 0.15 |
| **Trail Size Max** | Максимальный размер следа | 0.3 |
| **Trail Color** | Цвет следов | (0.6, 0.1, 0.1, 0.8) |

#### Спрайты:
- Если не назначены спрайты — создаются автоматически процедурно при запуске
- Для использования своих спрайтов: создайте Sprite[] с 6-8 спрайтами крови и назначьте в инспекторе

---

### 2. BloodParticles

Компонент создаётся автоматически при вызове `BloodParticles.CreateBloodBurst()`. Настраивается в коде.

#### Параметры (сериализуемые поля):

**Hit Effect Settings (при попадании):**
| Параметр | Описание | По умолчанию |
|----------|----------|--------------|
| **Hit Particle Count** | Количество частиц при попадании | 30 |
| **Hit Particle Lifetime** | Время жизни частиц (сек) | 0.4 |
| **Hit Particle Size** | Размер частиц | 0.08 |
| **Hit Color** | Цвет частиц | Red |
| **Hit Speed Min** | Минимальная скорость разлёта | 2 |
| **Hit Speed Max** | Максимальная скорость разлёта | 4 |

**Death Effect Settings (при смерти):**
| Параметр | Описание | По умолчанию |
|----------|----------|--------------|
| **Death Particle Count** | Количество частиц при смерти | 50 |
| **Death Particle Lifetime** | Время жизни частиц (сек) | 0.6 |
| **Death Particle Size** | Размер частиц | 0.12 |
| **Death Color** | Цвет частиц | (0.8, 0, 0) |
| **Death Speed Min** | Минимальная скорость разлёта | 3 |
| **Death Speed Max** | Максимальная скорость разлёта | 6 |

---

### 3. BloodTrailEmitter

Компонент для создания кровавых следов при беге. Добавьте на игрока и/или врагов.

#### Параметры:

| Параметр | Описание | По умолчанию |
|----------|----------|--------------|
| **Min Speed For Trail** | Минимальная скорость для появления следов | 2 |
| **Drops Per Second** | Количество капель в секунду | 8 |
| **Trail Duration** | Продолжительность кровотечения после удара | 3 |
| **Intensity** | Интенсивность (множитель) | 1 |
| **Only On Ground Hit** | Оставлять следы только на земле | false |

#### Trail Appearance:

| Параметр | Описание | По умолчанию |
|----------|----------|--------------|
| **Size Multiplier** | Размер относительно обычных пятен | 0.4 |
| **Position Randomness** | Случайное смещение позиции | 0.1 |
| **Rotation Randomness** | Случайный поворот | 30° |

#### Ground Check:

| Параметр | Описание | По умолчанию |
|----------|----------|--------------|
| **Ground Layer** | Слой земли | -1 (все слои) |
| **Ground Check Distance** | Дистанция проверки земли | 0.5 |

#### Optimization:

| Параметр | Описание | По умолчанию |
|----------|----------|--------------|
| **Low Blood Mode** | Упрощённый режим (меньше следов) | false |
| **Max Active Trails** | Максимум активных следов на существо | 50 |

---

## Интеграция

### EnemyHealth

Система крови интегрирована в `EnemyHealth.cs`:

```csharp
[Header("Blood Settings")]
[SerializeField] private bool _spawnBloodOnHit = true;
[SerializeField] private bool _spawnBloodOnDeath = true;
[SerializeField] private bool _enableTrailOnHit = true;
[SerializeField] private float _trailDuration = 2f;
[SerializeField, Range(0.1f, 2f)] private float _trailIntensity = 0.8f;
```

При получении урона:
- Вызывается `BloodSplatterManager.Instance.SpawnSplatter()` (мгновенный сплэш)
- Вызывается `BloodTrailEmitter.StartBleeding()` (следы при беге)

### PlayerHealth

Добавлена интеграция следов крови:

```csharp
[Header("Blood Trail Settings")]
[SerializeField] private bool _enableBloodTrail = true;
[SerializeField] private float _trailDuration = 3f;
[SerializeField, Range(0.1f, 2f)] private float _trailIntensity = 1f;
```

---

## API

```csharp
// Создать пятно крови при попадании
BloodSplatterManager.Instance.SpawnSplatter(position, direction);

// Создать пятно крови при смерти
BloodSplatterManager.Instance.SpawnSplatterAtDeath(position);

// Создать след крови (меньшие, частые пятна)
BloodSplatterManager.Instance.SpawnTrailSplatter(position, direction, size);

// Очистить все пятна
BloodSplatterManager.Instance.ClearAllSplatters();

// Начать кровотечение (для следов)
bloodTrailEmitter.StartBleeding(duration, intensity);

// Остановить кровотечение
bloodTrailEmitter.StopBleeding();

// Проверить состояние
bool isBleeding = bloodTrailEmitter.IsBleeding;
float timeRemaining = bloodTrailEmitter.BleedTimeRemaining;
```

---

## Визуальные настройки для референса

### Размер пятен

| Тип | Размер | Коэффициент |
|-----|--------|-------------|
| **Сплэш при ударе** | 0.3 - 0.8 | 1.0x |
| **Сплэш при смерти** | 0.36 - 1.2 | 1.5x |
| **След при беге** | 0.12 - 0.24 | 0.3-0.4x |

### Цвет пятен

| Тип | RGB | Alpha |
|-----|-----|-------|
| **Сплэш при ударе** | (112, 16, 16) | 1.0 |
| **Сплэш при смерти** | (102-153, 13-38, 13-38) | 1.0 |
| **След при беге** | (153, 26, 26) | 0.7-0.8 |

### Время жизни

| Тип | Lifetime | Fade Start |
|-----|----------|------------|
| **Обычные пятна** | 15 сек | 12 сек |
| **Следы** | 8 сек | 5 сек |

### Разброс для линейного следа

- **Position Randomness**: 0.1 — чуть смещает пятно в сторону от линии
- **Rotation Randomness**: 30° — пятна повёрнуты хаотично, но общий след читается
- **Drops Per Second**: 8-12 — частота капель при беге

---

## Оптимизация

1. **Пул объектов**: BloodSplatterManager использует пул (60 объектов)
2. **Max Splatters**: Лимит 50 активных пятен на сцене
3. **Low Blood Mode**: Упрощённый режим с удвоенным интервалом
4. **Lifetime**: Следы живут меньше (8 сек vs 15 сек)
5. **Ground Check**: Опциональная проверка земли

---

## История изменений

- **v1.0** — Базовая система с пулом и процедурными спрайтами
- **v1.1** — Добавлена граница скорости частиц, увеличено количество, уменьшен размер
- **v1.2** — Улучшена хаотичность краёв пятен
- **v1.3** — Добавлена система кровавых следов (BloodTrailEmitter)
