# Орда врагов — Поведение как в Vampire Survivors

## A) Геймдизайн/Логика поведения

### Текущая проблема
Все враги движутся по прямой к игроку и накладываются друг на друга.

### Паттерны движения

#### Минимальный вариант (только спавн)

1. **Спавн по дуге/окружности**
   - Вместо случайной точки в радиусе — выбор угла на окружности
   - Распределение врагов по секторам (фронтам)
   - Минимальное расстояние между точками спавна

2. **Variance в скорости**
   - Каждый враг имеет ±10-20% к базовой скорости
   - Быстрые догоняют, медленные отстают — естественное рассеивание

#### Расширенный вариант (спавн + движение)

1. **Simple Separation**
   - Враги отталкиваются от ближайших соседей (2-3)
   - Упрощённый алгоритм: только по distance, без velocity
   - Радиус отталкивания = 1.5-2x размер спрайта

2. **Формирование кольца давления**
   - Враги стремятся к целевому радиусу вокруг игрока
   - Часть врагов атакует "в лоб", часть "обтекает"

3. **Направленные потоки**
   - WaveSystem выбирает "направление атаки" для группы
   - Враги из одной волны появляются с одного направления

---

## B) Реализация в проекте "Сечь"

### Вариант 1: Минимальный (только спавн)

#### Изменения в EnemiesGenerator

```csharp
// Новые параметры в инспекторе
[Header("Орда - Спавн")]
[SerializeField] private bool _spawnOnArc = true;
[SerializeField] private float _arcSpreadAngle = 120f; // дуга в градусах
[SerializeField] private float _minSpawnAngleDistance = 15f; // мин угол между врагами

private float _lastSpawnAngle = 0f;

private void CalculateSpawnPosition(Transform playerTransform)
{
    if (!_spawnOnArc)
    {
        // Старый метод - случайная точка
        ...
        return;
    }

    // Новый метод - спавн по дуге
    float angle = _lastSpawnAngle + Random.Range(_minSpawnAngleDistance, _arcSpreadAngle);
    angle = angle % 360f;
    _lastSpawnAngle = angle;

    float distance = Random.Range(_minSpawnDistance, _maxSpawnDistance);
    float rad = angle * Mathf.Deg2Rad;
    
    _spawnPosition = playerTransform.position + new Vector3(
        Mathf.Cos(rad) * distance,
        Mathf.Sin(rad) * distance,
        0
    );
}
```

### Вариант 2: Расширенный (спавн + separation)

#### Изменения в EnemyMovement

```csharp
public class EnemyMovement : MonoBehaviour
{
    [Header("Орда - Поведение")]
    [SerializeField] private float _separationRadius = 1.5f;
    [SerializeField] private float _separationForce = 2f;
    [SerializeField] private float _speedVariance = 0.2f;
    
    private float _actualSpeed;
    private Vector3 _separationOffset;

    public void Init(float speed)
    {
        _speed = speed;
        // Вариация скорости ±20%
        _actualSpeed = speed * (1f + Random.Range(-_speedVariance, _speedVariance));
        _separationOffset = Vector3.zero;
    }

    private void MoveToTarget()
    {
        if (_target == null) return;

        // Основное движение к игроку
        Vector3 direction = (_target.position - transform.position).normalized;
        
        // Separation - отталкивание от соседей
        Vector3 separation = CalculateSeparation();
        
        // Комбинируем направления
        Vector3 finalDirection = (direction + separation * _separationForce).normalized;

        transform.position += finalDirection * _actualSpeed * Time.deltaTime;
        
        AnimationRunPlayed?.Invoke();
        Flip(_target);
    }

    private Vector3 CalculateSeparation()
    {
        Vector3 separation = Vector3.zero;
        int neighbors = 0;
        
        // Проверяем только ближайших врагов из списка
        for (int i = 0; i < EnemiesGenerator.AllEnemies.Count; i++)
        {
            Enemy other = EnemiesGenerator.AllEnemies[i];
            if (other == null || other.gameObject == gameObject) continue;
            
            float dist = Vector3.Distance(transform.position, other.transform.position);
            
            if (dist < _separationRadius && dist > 0)
            {
                // Вектор от соседа к нам
                Vector3 away = transform.position - other.transform.position;
                separation += away / dist; // Чем ближе, тем сильнее отталкивание
                neighbors++;
            }
            
            // Ограничиваем проверку для оптимизации
            if (neighbors >= 3) break;
        }
        
        return neighbors > 0 ? separation / neighbors : Vector3.zero;
    }
}
```

---

## Рекомендуемые параметры

### Для EnemyData (баланс)

| Параметр | Рекомендация | Пояснение |
|----------|--------------|-----------|
| Speed Variance | ±15-20% | Естественное рассеивание |
| Separation Radius | 1.0-1.5 | ~2x размер спрайта |
| Separation Force | 1.5-2.5 | Не слишком сильное |

### Для WaveSystem

| Параметр | Рекомендация |
|----------|--------------|
| Arc Spread | 90-180° |
| Min Angle Distance | 10-20° |

---

## Приоритет реализации

### Этап 1: Минимальный (1-2 часа)
- [x] Спавн по дуге в EnemiesGenerator
- [x] Вариация скорости в EnemyMovement

### Этап 2: Расширенный (2-4 часа)
- [ ] Simple separation в EnemyMovement
- [ ] Кольцо давления вокруг игрока
- [ ] Направленные волны

### Этап 3: Оптимизация (если нужно)
- [ ] Grid-based lookup для соседей
- [ ] Проверка separation через n-1 врагов
