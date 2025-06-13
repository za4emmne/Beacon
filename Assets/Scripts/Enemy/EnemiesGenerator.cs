using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    [Header("Настройки пула")]
    [SerializeField] private int _initialPoolSize = 50;
    [SerializeField] private int _maxPoolSize = 500; // Увеличили лимит, т.к. враги накапливаются

    [Header("Спавн параметры")]
    [SerializeField] private float _minSpawnDistance = 10f;
    [SerializeField] private float _maxSpawnDistance = 15f;

    [Header("Зависимости")]
    [SerializeField] private StarGenerator _starGenerator;

    // Оптимизированные пулы
    private Dictionary<int, Queue<Enemy>> _enemyPools = new Dictionary<int, Queue<Enemy>>();
    private Dictionary<int, EnemyData> _enemyDataMap = new Dictionary<int, EnemyData>();

    // Кэшированные компоненты
    private Transform _transform;

    // Переиспользуемые переменные для избежания аллокаций
    private Vector3 _spawnPosition = Vector3.zero;
    private Vector2 _randomDirection = Vector2.zero;

    // Счетчик активных врагов для мониторинга
    private int _activeEnemiesCount = 0;

    public event Action OneKill;

    private void Awake()
    {
        _transform = transform;
    }

    public void SpawnEnemyWithModifiers(EnemyData enemyData, Transform playerTransform)
    {
        Enemy enemy = GetEnemyFromPool(enemyData);
        if (enemy == null) return;

        // Применяем модификаторы
        enemy.Initialize(enemyData, this);
        enemy.gameObject.SetActive(true);

        // Устанавливаем позицию
        CalculateSpawnPosition(playerTransform);
        enemy.transform.position = _spawnPosition;

        _activeEnemiesCount++;
    }

    private Enemy GetEnemyFromPool(EnemyData enemyData)
    {
        int enemyId = enemyData.GetInstanceID();

        // Создаем пул если его нет
        if (!_enemyPools.ContainsKey(enemyId))
        {
            _enemyPools[enemyId] = new Queue<Enemy>();
            _enemyDataMap[enemyId] = enemyData;
            PrewarmPool(enemyData, _initialPoolSize);
        }

        Queue<Enemy> pool = _enemyPools[enemyId];

        // Создаем новый объект если пул пуст
        if (pool.Count == 0)
        {
            if (GetTotalPooledObjects() >= _maxPoolSize)
            {
                Debug.LogWarning($"Достигнут максимальный размер пула! Активных врагов: {_activeEnemiesCount}");
                return null;
            }
            return CreateNewEnemy(enemyData);
        }

        return pool.Dequeue();
    }

    private Enemy CreateNewEnemy(EnemyData enemyData)
    {
        if (enemyData.Prefab == null)
        {
            Debug.LogError($"У {enemyData.name} не назначен префаб!");
            return null;
        }

        GameObject enemyObj = Instantiate(enemyData.Prefab, _transform);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError($"У префаба {enemyData.Prefab.name} нет компонента Enemy!");
            Destroy(enemyObj);
            return null;
        }

        return enemy;
    }

    private void PrewarmPool(EnemyData enemyData, int count)
    {
        int enemyId = enemyData.GetInstanceID();
        Queue<Enemy> pool = _enemyPools[enemyId];

        for (int i = 0; i < count; i++)
        {
            Enemy enemy = CreateNewEnemy(enemyData);
            if (enemy != null)
            {
                enemy.gameObject.SetActive(false);
                pool.Enqueue(enemy);
            }
        }
    }

    public void ReturnEnemyToPool(Enemy enemy)
    {
        if (enemy == null) return;

        OneKill?.Invoke();
        _activeEnemiesCount = Mathf.Max(0, _activeEnemiesCount - 1);

        // Спавним звезду опыта
        SpawnExperienceStar(enemy.transform.position);

        // Возвращаем в пул
        EnemyData enemyData = enemy.Data;
        if (enemyData != null)
        {
            int enemyId = enemyData.GetInstanceID();
            if (_enemyPools.ContainsKey(enemyId))
            {
                enemy.gameObject.SetActive(false);
                _enemyPools[enemyId].Enqueue(enemy);
                return;
            }
        }

        // Если не удалось вернуть в пул, уничтожаем
        Destroy(enemy.gameObject);
    }

    private void SpawnExperienceStar(Vector3 position)
    {
        if (_starGenerator != null)
        {
            var star = _starGenerator.GetObject();
            if (star != null)
            {
                star.transform.position = position;
            }
        }
    }

    private void CalculateSpawnPosition(Transform playerTransform)
    {
        if (playerTransform == null)
        {
            _spawnPosition = _transform.position;
            return;
        }

        // Генерируем случайное направление
        _randomDirection.x = UnityEngine.Random.Range(-1f, 1f);
        _randomDirection.y = UnityEngine.Random.Range(-1f, 1f);
        _randomDirection.Normalize();

        float spawnDistance = UnityEngine.Random.Range(_minSpawnDistance, _maxSpawnDistance);

        _spawnPosition.x = playerTransform.position.x + _randomDirection.x * spawnDistance;
        _spawnPosition.y = playerTransform.position.y + _randomDirection.y * spawnDistance;
        _spawnPosition.z = playerTransform.position.z;
    }

    private int GetTotalPooledObjects()
    {
        int total = 0;
        foreach (var pool in _enemyPools.Values)
        {
            total += pool.Count;
        }
        return total;
    }

    public void ClearAllPools()
    {
        foreach (var pool in _enemyPools.Values)
        {
            while (pool.Count > 0)
            {
                Enemy enemy = pool.Dequeue();
                if (enemy != null && enemy.gameObject != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
        }

        _enemyPools.Clear();
        _enemyDataMap.Clear();
        _activeEnemiesCount = 0;
    }

    private void OnDestroy()
    {
        ClearAllPools();
    }

    // Публичные методы для мониторинга
    public int GetTotalActiveEnemies() => _activeEnemiesCount;

    public int GetPoolCount(EnemyData enemyData)
    {
        int enemyId = enemyData.GetInstanceID();
        return _enemyPools.ContainsKey(enemyId) ? _enemyPools[enemyId].Count : 0;
    }
}
