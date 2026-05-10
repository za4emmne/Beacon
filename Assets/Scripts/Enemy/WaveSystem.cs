using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSystem : MonoBehaviour
{
    [Header("Конфигурация волн")]
    public WaveDataSO[] waveConfigs;
    public float waveDuration = 60f;
    public float healthMultiplierPerWave = 1.1f;
    public float damageMultiplierPerWave = 1.05f;

    [Header("Боссы")]
    public EnemyData[] bossEnemies;
    [Range(0f, 1f)] public float bossSpawnChance = 0.15f;

    [Header("Зависимости")]
    [SerializeField] private EnemiesGenerator _enemySpawner;
    [SerializeField] private Transform _playerTransform;

    [Header("События")]
    public UnityEngine.Events.UnityEvent<int> OnWaveStart;
    public UnityEngine.Events.UnityEvent<int> OnWaveComplete;

    private int _currentWaveIndex;
    private bool _isWaveActive;
    private Coroutine _currentWaveCoroutine;

    private float[] _cachedHealthMultipliers = new float[100];
    private float[] _cachedDamageMultipliers = new float[100];

    private void Awake()
    {
        InitDebug.Log($"[INIT][WaveSystem] Awake() - this={GetInstanceID()}, scene={SceneManager.GetActiveScene().name}");
        PrecomputeMultipliers();
    }

    private void OnEnable()
    {
        InitDebug.Log("[INIT][WaveSystem] OnEnable()");
        
        _isWaveActive = false;
        _currentWaveIndex = 0;
        
        InitDebug.Log($"[INIT][WaveSystem] Reset - waveIndex={_currentWaveIndex}, active={_isWaveActive}");
    }

    private void OnDisable()
    {
        InitDebug.Log("[INIT][WaveSystem] OnDisable()");
        
        _isWaveActive = false;
        
        if (_currentWaveCoroutine != null)
        {
            StopCoroutine(_currentWaveCoroutine);
            _currentWaveCoroutine = null;
            InitDebug.Log("[INIT][WaveSystem] Stopped wave coroutine");
        }
    }

    private void OnDestroy()
    {
        InitDebug.Log($"[INIT][WaveSystem] OnDestroy() - this={GetInstanceID()}");
    }

    public void Initialized(Transform transform)
    {
        _playerTransform = transform;
        InitDebug.Log($"[INIT][WaveSystem] Initialized with player: {transform?.GetInstanceID()}");
    }

    public void StartWave()
    {
        InitDebug.Log("[INIT][WaveSystem] StartWave() called");
        
        if (!ValidateDependencies())
        {
            InitDebug.LogError("[INIT][WaveSystem] Dependencies validation FAILED!");
            return;
        }
        
        StartCoroutine(WaveTimer());
    }

    private bool ValidateDependencies()
    {
        bool valid = true;
        
        if (_enemySpawner == null)
        {
            InitDebug.LogError("[INIT][WaveSystem] _enemySpawner is NULL!");
            valid = false;
            enabled = false;
        }

        if (_playerTransform == null)
        {
            InitDebug.LogError("[INIT][WaveSystem] _playerTransform is NULL!");
            valid = false;
            enabled = false;
        }

        if (waveConfigs == null || waveConfigs.Length == 0)
        {
            InitDebug.LogError("[INIT][WaveSystem] waveConfigs is NULL or EMPTY!");
            valid = false;
        }
        
        if (valid)
            InitDebug.Log("[INIT][WaveSystem] Dependencies validated OK");
        
        return valid;
    }

    private void PrecomputeMultipliers()
    {
        for (int i = 0; i < _cachedHealthMultipliers.Length; i++)
        {
            _cachedHealthMultipliers[i] = Mathf.Pow(healthMultiplierPerWave, i);
            _cachedDamageMultipliers[i] = Mathf.Pow(damageMultiplierPerWave, i);
        }
    }

    private IEnumerator WaveTimer()
    {
        InitDebug.Log("[WAVE][WaveSystem] WaveTimer STARTED");
        
        while (enabled)
        {
            _currentWaveIndex++;
            _isWaveActive = true;

            WaveDataSO currentWave = GetCurrentWaveConfig();

            InitDebug.Log($"=== WAVE {_currentWaveIndex} STARTED: {currentWave?.waveName} ===");
            OnWaveStart?.Invoke(_currentWaveIndex);

            bool shouldSpawnBoss = Random.value < bossSpawnChance;

            if (shouldSpawnBoss && bossEnemies != null && bossEnemies.Length > 0)
            {
                InitDebug.Log($"[WAVE][WaveSystem] Spawning BOSS (chance: {bossSpawnChance})");
                _currentWaveCoroutine = StartCoroutine(SpawnBossWave());
            }
            else
            {
                InitDebug.Log($"[WAVE][WaveSystem] Spawning regular wave");
                _currentWaveCoroutine = StartCoroutine(SpawnRegularWave(currentWave));
            }

            yield return new WaitForSeconds(waveDuration);

            if (_currentWaveCoroutine != null)
            {
                StopCoroutine(_currentWaveCoroutine);
                _currentWaveCoroutine = null;
            }

            _isWaveActive = false;
            OnWaveComplete?.Invoke(_currentWaveIndex);
            
            InitDebug.Log($"=== WAVE {_currentWaveIndex} COMPLETED ===");
        }
    }

    private IEnumerator SpawnRegularWave(WaveDataSO wave)
    {
        if (wave == null || wave.possibleEnemies == null)
        {
            InitDebug.LogError("[WAVE][WaveSystem] Wave config is NULL!");
            yield break;
        }
        
        float spawnInterval = waveDuration / wave.totalEnemies;
        var spawnWait = new WaitForSeconds(spawnInterval);

        InitDebug.Log($"[WAVE][WaveSystem] Spawning {wave.totalEnemies} enemies, interval={spawnInterval:F2}s");

        for (int i = 0; i < wave.totalEnemies; i++)
        {
            SpawnRandomEnemy(wave);
            yield return spawnWait;
        }

        InitDebug.Log($"[WAVE][WaveSystem] Regular wave SPAWN COMPLETE");
    }

    private IEnumerator SpawnBossWave()
    {
        if (bossEnemies == null || bossEnemies.Length == 0)
        {
            InitDebug.LogWarning("[WAVE][WaveSystem] No bosses configured!");
            yield break;
        }
        
        int randomBossIndex = Random.Range(0, bossEnemies.Length);
        EnemyData bossData = bossEnemies[randomBossIndex];
        
        InitDebug.Log($"[WAVE][WaveSystem] Spawning BOSS: {bossData?.name} (index={randomBossIndex})");
        
        SpawnBoss(bossData);

        WaveDataSO currentWave = GetCurrentWaveConfig();
        int additionalEnemies = currentWave != null ? currentWave.totalEnemies / 3 : 10;

        if (additionalEnemies > 0)
        {
            float spawnInterval = waveDuration / additionalEnemies;
            var spawnWait = new WaitForSeconds(spawnInterval);

            for (int i = 0; i < additionalEnemies; i++)
            {
                SpawnRandomEnemy(currentWave);
                yield return spawnWait;
            }
        }
        
        InitDebug.Log("[WAVE][WaveSystem] Boss wave SPAWN COMPLETE");
    }

    private void SpawnRandomEnemy(WaveDataSO wave)
    {
        if (wave == null || wave.possibleEnemies == null || wave.possibleEnemies.Length == 0)
        {
            InitDebug.LogError("[WAVE][WaveSystem] No enemies to spawn!");
            return;
        }
        
        int randomIndex = Random.Range(0, wave.possibleEnemies.Length);
        EnemyData randomEnemy = wave.possibleEnemies[randomIndex];

        float healthMultiplier = GetHealthMultiplier(_currentWaveIndex - 1);
        float damageMultiplier = GetDamageMultiplier(_currentWaveIndex - 1);

        _enemySpawner?.SpawnEnemyWithModifiers(randomEnemy, _playerTransform);
    }

    private void SpawnBoss(EnemyData bossData)
    {
        if (bossData == null)
        {
            InitDebug.LogError("[WAVE][WaveSystem] Boss data is NULL!");
            return;
        }
        
        float healthMultiplier = GetHealthMultiplier(_currentWaveIndex - 1) * 3f;
        float damageMultiplier = GetDamageMultiplier(_currentWaveIndex - 1) * 2f;

        _enemySpawner?.SpawnEnemyWithModifiers(bossData, _playerTransform);
        
        InitDebug.Log($"[WAVE][WaveSystem] BOSS spawned with multipliers: health={healthMultiplier:F2}, damage={damageMultiplier:F2}");
    }

    private float GetHealthMultiplier(int waveIndex)
    {
        return waveIndex < _cachedHealthMultipliers.Length
            ? _cachedHealthMultipliers[waveIndex]
            : Mathf.Pow(healthMultiplierPerWave, waveIndex);
    }

    private float GetDamageMultiplier(int waveIndex)
    {
        return waveIndex < _cachedDamageMultipliers.Length
            ? _cachedDamageMultipliers[waveIndex]
            : Mathf.Pow(damageMultiplierPerWave, waveIndex);
    }

    private WaveDataSO GetCurrentWaveConfig()
    {
        if (waveConfigs == null || waveConfigs.Length == 0)
            return null;
            
        int waveIndex = Mathf.Min(_currentWaveIndex - 1, waveConfigs.Length - 1);
        return waveConfigs[waveIndex];
    }

    public float GetWaveTimeRemaining()
    {
        return _isWaveActive ? waveDuration : 0f;
    }

    public int CurrentWave => _currentWaveIndex;
    public bool IsWaveActive => _isWaveActive;

    public int GetTotalEnemiesOnScene()
    {
        return _enemySpawner != null ? _enemySpawner.GetTotalActiveEnemies() : 0;
    }
}