using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesGenerator : MonoBehaviour
{
    [Header("Пул врагов")]
    [SerializeField] private int _initialPoolSize = 50;
    [SerializeField] private int _maxPoolSize = 500;

    [Header("Дистанция спавна")]
    [SerializeField] private float _minSpawnDistance = 10f;
    [SerializeField] private float _maxSpawnDistance = 15f;

    [Header("Орда - Спавн")]
    [SerializeField] private bool _spawnOnArc = true;
    [SerializeField] private float _arcSpreadAngle = 120f;
    [SerializeField] private float _minSpawnAngleDistance = 15f;
    [SerializeField] private float _spawnAngleOffset = 0f;

    [Header("Генераторы")]
    [SerializeField] private StarsSpawner _starGenerator;

    public static List<Enemy> AllEnemies = new List<Enemy>();

    private Dictionary<int, Queue<Enemy>> _enemyPools = new Dictionary<int, Queue<Enemy>>();
    private Dictionary<int, EnemyData> _enemyDataMap = new Dictionary<int, EnemyData>();

    private Transform _transform;
    private Transform _playerTransform;

    private Vector3 _spawnPosition = Vector3.zero;
    private Vector2 _randomDirection = Vector2.zero;

    private int _activeEnemiesCount = 0;

    public event Action OneKill;

    private void Awake()
    {
        InitDebug.Log($"[INIT][EnemiesGenerator] Awake() - this={GetInstanceID()}, scene={SceneManager.GetActiveScene().name}");
        
        _transform = transform;
        
        InitDebug.Log($"[INIT][EnemiesGenerator] AllEnemies count: {AllEnemies.Count}");
    }

    private void OnEnable()
    {
        InitDebug.Log("[INIT][EnemiesGenerator] OnEnable()");
        ClearAllPools();
        AllEnemies.Clear();
        InitDebug.Log("[INIT][EnemiesGenerator] Pools cleared");
    }

    private void OnDisable()
    {
        InitDebug.Log("[INIT][EnemiesGenerator] OnDisable()");
        
        ClearAllPools();
        AllEnemies.Clear();
        
        InitDebug.Log($"[INIT][EnemiesGenerator] Cleared - activeEnemies={_activeEnemiesCount}, AllEnemies={AllEnemies.Count}");
    }

    private void OnDestroy()
    {
        InitDebug.Log($"[INIT][EnemiesGenerator] OnDestroy() - this={GetInstanceID()}");
        ClearAllPools();
        AllEnemies.Clear();
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        InitDebug.Log($"[INIT][EnemiesGenerator] SetPlayerTransform: {playerTransform?.GetInstanceID()}");
    }

    public Transform GetPlayerTransform() => _playerTransform;

    public void SpawnEnemyWithModifiers(EnemyData enemyData, Transform playerTransform)
    {
        Enemy enemy = GetEnemyFromPool(enemyData);
        if (enemy == null)
        {
            InitDebug.LogWarning($"[SPAWN][EnemiesGenerator] Failed to get enemy from pool: {enemyData?.name}");
            return;
        }

        if (enemy is Leshy leshy && enemyData is LeshyData leshyData)
        {
            leshy.gameObject.SetActive(true);
            leshy.Initialize(leshyData, this, _playerTransform != null ? _playerTransform : playerTransform);
            InitDebug.Log($"[SPAWN][EnemiesGenerator] Spawned Leshy: {enemy.GetInstanceID()}");
        }
        else
        {
            enemy.Initialize(enemyData, this);
            enemy.gameObject.SetActive(true);
            InitDebug.Log($"[SPAWN][EnemiesGenerator] Spawned {enemyData?.name}: {enemy.GetInstanceID()}");
        }

        CalculateSpawnPosition(playerTransform);
        enemy.transform.position = _spawnPosition;

        _activeEnemiesCount++;
    }

    public void AddEnemyOnList(Enemy enemy)
    {
        AllEnemies.Add(enemy);
        InitDebug.Log($"[EVENT][EnemiesGenerator] Added to AllEnemies: {enemy.GetInstanceID()}, total={AllEnemies.Count}");
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        AllEnemies.Remove(enemy);
        InitDebug.Log($"[EVENT][EnemiesGenerator] Removed from AllEnemies: {enemy?.GetInstanceID()}, total={AllEnemies.Count}");
    }

    public Enemy GetEnemyFromPool(EnemyData enemyData)
    {
        if (enemyData == null)
        {
            InitDebug.LogError("[SPAWN][EnemiesGenerator] enemyData is NULL!");
            return null;
        }
        
        int enemyId = enemyData.GetInstanceID();

        if (!_enemyPools.ContainsKey(enemyId))
        {
            _enemyPools[enemyId] = new Queue<Enemy>();
            _enemyDataMap[enemyId] = enemyData;
            PrewarmPool(enemyData, _initialPoolSize);
            InitDebug.Log($"[POOL][EnemiesGenerator] Created new pool for {enemyData.name}, id={enemyId}");
        }

        Queue<Enemy> pool = _enemyPools[enemyId];

        if (pool.Count == 0)
        {
            if (GetTotalPooledObjects() >= _maxPoolSize)
            {
                InitDebug.LogWarning($"[POOL][EnemiesGenerator] Max pool size reached! active={_activeEnemiesCount}");
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
            InitDebug.LogError($"[SPAWN][EnemiesGenerator] Prefab is null for {enemyData.name}");
            return null;
        }

        GameObject enemyObj = Instantiate(enemyData.Prefab, _transform);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        if (enemy == null)
        {
            EnemyBoss boss = enemyObj.GetComponent<EnemyBoss>();
            if (boss == null)
            {
                InitDebug.LogError($"[SPAWN][EnemiesGenerator] No Enemy or EnemyBoss component on prefab {enemyData.Prefab.name}");
                Destroy(enemyObj);
                return null;
            }
        }

        InitDebug.Log($"[POOL][EnemiesGenerator] Created new enemy: {enemyObj.GetInstanceID()}");
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
        
        InitDebug.Log($"[POOL][EnemiesGenerator] Prewarmed {count} enemies for {enemyData.name}");
    }

    public void ReturnEnemyToPool(Enemy enemy)
    {
        if (enemy == null) return;

        OneKill?.Invoke();
        
        _activeEnemiesCount = Mathf.Max(0, _activeEnemiesCount - 1);
        int procentChance = UnityEngine.Random.Range(0, 100);

        if (procentChance < enemy.Data.dropChance * 100)
            SpawnExperienceStar(enemy.transform.position);

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

        if (_spawnOnArc)
        {
            float angle = _spawnAngleOffset + UnityEngine.Random.Range(_minSpawnAngleDistance, _arcSpreadAngle);
            _spawnAngleOffset = angle % 360f;

            float distance = UnityEngine.Random.Range(_minSpawnDistance, _maxSpawnDistance);
            float rad = angle * Mathf.Deg2Rad;

            _spawnPosition = playerTransform.position + new Vector3(
                Mathf.Cos(rad) * distance,
                Mathf.Sin(rad) * distance,
                0
            );
        }
        else
        {
            _randomDirection.x = UnityEngine.Random.Range(-1f, 1f);
            _randomDirection.y = UnityEngine.Random.Range(-1f, 1f);
            _randomDirection.Normalize();

            float spawnDistance = UnityEngine.Random.Range(_minSpawnDistance, _maxSpawnDistance);

            _spawnPosition.x = playerTransform.position.x + _randomDirection.x * spawnDistance;
            _spawnPosition.y = playerTransform.position.y + _randomDirection.y * spawnDistance;
            _spawnPosition.z = playerTransform.position.z;
        }
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
        
        InitDebug.Log("[POOL][EnemiesGenerator] All pools cleared");
    }

    public int GetTotalActiveEnemies() => _activeEnemiesCount;

    public int GetPoolCount(EnemyData enemyData)
    {
        int enemyId = enemyData.GetInstanceID();
        return _enemyPools.ContainsKey(enemyId) ? _enemyPools[enemyId].Count : 0;
    }
}