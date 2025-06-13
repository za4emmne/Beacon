using System.Collections;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [Header("��������� ����")]
    public WaveDataSO[] waveConfigs;
    public float waveDuration = 60f; // ������ 60 ������ �� �����
    public float healthMultiplierPerWave = 1.1f;
    public float damageMultiplierPerWave = 1.05f;

    [Header("�����")]
    public EnemyData[] bossEnemies;
    [Range(0f, 1f)] public float bossSpawnChance = 0.15f;

    [Header("�����������")]
    [SerializeField] private EnemiesGenerator _enemySpawner;
    [SerializeField] private Transform _playerTransform;

    [Header("�������")]
    public UnityEngine.Events.UnityEvent<int> OnWaveStart;
    public UnityEngine.Events.UnityEvent<int> OnWaveComplete;

    private int _currentWaveIndex;
    private bool _isWaveActive;
    private Coroutine _currentWaveCoroutine;

    // ������������ �������� ��� �����������
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
            Debug.LogError("EnemiesGenerator �� �������� � WaveSystem!");
            enabled = false;
            return;
        }

        if (_playerTransform == null)
        {
            Debug.LogError("Player Transform �� �������� � WaveSystem!");
            enabled = false;
            return;
        }

        if (waveConfigs == null || waveConfigs.Length == 0)
        {
            Debug.LogError("������������ ���� �� ���������!");
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

            Debug.Log($"?? ����� {_currentWaveIndex} ��������! ({currentWave.waveName})");
            OnWaveStart?.Invoke(_currentWaveIndex);

            // ��������� ����� ������ ����������� � ��������
            bool shouldSpawnBoss = Random.value < bossSpawnChance;

            if (shouldSpawnBoss && bossEnemies.Length > 0)
            {
                _currentWaveCoroutine = StartCoroutine(SpawnBossWave());
            }
            else
            {
                _currentWaveCoroutine = StartCoroutine(SpawnRegularWave(currentWave));
            }

            // ���� ������ 60 ������ ���������� �� ������
            yield return new WaitForSeconds(waveDuration);

            // ������������� ����� ���� �� ��� ����
            if (_currentWaveCoroutine != null)
            {
                StopCoroutine(_currentWaveCoroutine);
                _currentWaveCoroutine = null;
            }

            _isWaveActive = false;
            OnWaveComplete?.Invoke(_currentWaveIndex);
            Debug.Log($"? ����� {_currentWaveIndex} ���������! (����� �������)");

            // ����� ��������� � ��������� ����� ��� �����
        }
    }

    private IEnumerator SpawnRegularWave(WaveDataSO wave)
    {
        float spawnInterval = waveDuration / wave.totalEnemies;
        var spawnWait = new WaitForSeconds(spawnInterval);

        // ������� ������ ���������� � ������� 60 ������
        for (int i = 0; i < wave.totalEnemies; i++)
        {
            SpawnRandomEnemy(wave);
            yield return spawnWait;
        }

        // �������� �����������, �� ����� ������������ �� ��������� �������
    }

    private IEnumerator SpawnBossWave()
    {
        // ������� ����� � ������ �����
        int randomBossIndex = Random.Range(0, bossEnemies.Length);
        SpawnBoss(bossEnemies[randomBossIndex]);

        // ����� �������� �������������� ������ � �����
        WaveDataSO currentWave = GetCurrentWaveConfig();
        int additionalEnemies = currentWave.totalEnemies / 3; // 1/3 �� �������� ����������

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
        Debug.Log("?? ���� ��������!");
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

    // ����� ��� ��������� ����������� ������� ����� (��� UI)
    public float GetWaveTimeRemaining()
    {
        // ����� �������� ��������� ������ ���� ����� ���������� ���������� �����
        return _isWaveActive ? waveDuration : 0f;
    }

    // ��������� �������� ��� UI
    public int CurrentWave => _currentWaveIndex;
    public bool IsWaveActive => _isWaveActive;

    // ����� ��� ��������� ������ ���������� ������ �� �����
    public int GetTotalEnemiesOnScene()
    {
        return _enemySpawner.GetTotalActiveEnemies();
    }
}
