using System.Collections;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [Header("Настройки волн")]
    public WaveDataSO[] waveConfigs;
    public float waveDuration = 60f; // Строго 60 секунд на волну
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

    // Кэшированные значения для оптимизации
    private float[] _cachedHealthMultipliers = new float[100];
    private float[] _cachedDamageMultipliers = new float[100];

    private void Awake()
    {
        PrecomputeMultipliers();
        ValidateDependencies();
    }

    private void PrecomputeMultipliers()
    {
        for (int i = 0; i < _cachedHealthMultipliers.Length; i++)
        {
            _cachedHealthMultipliers[i] = Mathf.Pow(healthMultiplierPerWave, i);
            _cachedDamageMultipliers[i] = Mathf.Pow(damageMultiplierPerWave, i);
        }
    }

    private void ValidateDependencies()
    {
        if (_enemySpawner == null)
        {
            Debug.LogError("EnemiesGenerator не назначен в WaveSystem!");
            enabled = false;
            return;
        }

        if (_playerTransform == null)
        {
            Debug.LogError("Player Transform не назначен в WaveSystem!");
            enabled = false;
            return;
        }

        if (waveConfigs == null || waveConfigs.Length == 0)
        {
            Debug.LogError("Конфигурации волн не назначены!");
            enabled = false;
        }
    }

    private void Start()
    {
        StartCoroutine(WaveTimer());
    }

    private IEnumerator WaveTimer()
    {
        while (enabled)
        {
            _currentWaveIndex++;
            _isWaveActive = true;

            WaveDataSO currentWave = GetCurrentWaveConfig();

            Debug.Log($"?? Волна {_currentWaveIndex} началась! ({currentWave.waveName})");
            OnWaveStart?.Invoke(_currentWaveIndex);

            // Запускаем спавн врагов параллельно с таймером
            bool shouldSpawnBoss = Random.value < bossSpawnChance;

            if (shouldSpawnBoss && bossEnemies.Length > 0)
            {
                _currentWaveCoroutine = StartCoroutine(SpawnBossWave());
            }
            else
            {
                _currentWaveCoroutine = StartCoroutine(SpawnRegularWave(currentWave));
            }

            // Ждем СТРОГО 60 секунд независимо от врагов
            yield return new WaitForSeconds(waveDuration);

            // Останавливаем спавн если он еще идет
            if (_currentWaveCoroutine != null)
            {
                StopCoroutine(_currentWaveCoroutine);
                _currentWaveCoroutine = null;
            }

            _isWaveActive = false;
            OnWaveComplete?.Invoke(_currentWaveIndex);
            Debug.Log($"? Волна {_currentWaveIndex} завершена! (Время истекло)");

            // Сразу переходим к следующей волне без паузы
        }
    }

    private IEnumerator SpawnRegularWave(WaveDataSO wave)
    {
        float spawnInterval = waveDuration / wave.totalEnemies;
        var spawnWait = new WaitForSeconds(spawnInterval);

        // Спавним врагов равномерно в течение 60 секунд
        for (int i = 0; i < wave.totalEnemies; i++)
        {
            SpawnRandomEnemy(wave);
            yield return spawnWait;
        }

        // Корутина завершается, но волна продолжается до истечения таймера
    }

    private IEnumerator SpawnBossWave()
    {
        // Спавним босса в начале волны
        int randomBossIndex = Random.Range(0, bossEnemies.Length);
        SpawnBoss(bossEnemies[randomBossIndex]);

        // Можем добавить дополнительных врагов к боссу
        WaveDataSO currentWave = GetCurrentWaveConfig();
        int additionalEnemies = currentWave.totalEnemies / 3; // 1/3 от обычного количества

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
    }

    private void SpawnRandomEnemy(WaveDataSO wave)
    {
        int randomIndex = Random.Range(0, wave.possibleEnemies.Length);
        EnemyData randomEnemy = wave.possibleEnemies[randomIndex];

        float healthMultiplier = GetHealthMultiplier(_currentWaveIndex - 1);
        float damageMultiplier = GetDamageMultiplier(_currentWaveIndex - 1);

        _enemySpawner.SpawnEnemyWithModifiers(randomEnemy, _playerTransform);
    }

    private void SpawnBoss(EnemyData bossData)
    {
        float healthMultiplier = GetHealthMultiplier(_currentWaveIndex - 1) * 3f;
        float damageMultiplier = GetDamageMultiplier(_currentWaveIndex - 1) * 2f;

        _enemySpawner.SpawnEnemyWithModifiers(bossData, _playerTransform);
        Debug.Log("?? БОСС появился!");
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
        int waveIndex = Mathf.Min(_currentWaveIndex - 1, waveConfigs.Length - 1);
        return waveConfigs[waveIndex];
    }

    // Метод для получения оставшегося времени волны (для UI)
    public float GetWaveTimeRemaining()
    {
        // Можно добавить отдельный таймер если нужно показывать оставшееся время
        return _isWaveActive ? waveDuration : 0f;
    }

    // Публичные свойства для UI
    public int CurrentWave => _currentWaveIndex;
    public bool IsWaveActive => _isWaveActive;

    // Метод для получения общего количества врагов на сцене
    public int GetTotalEnemiesOnScene()
    {
        return _enemySpawner.GetTotalActiveEnemies();
    }
}
