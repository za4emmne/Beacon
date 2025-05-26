using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string nameWave;
        public int totalEnemies; // Общее число врагов в волн
        public float waveTime;
        public float spawnRate;
        public EnemyData[] possibleEnemies; // Какие враги могут появляться
    }

    public Wave[] waves; // Массив волн
    private int currentWaveIndex = 0;
    private int enemiesLeftToSpawn;
    private int enemiesAlive;
    private float nextSpawnTime;

    [Header("Настройки сложности")]
    public float timeBetweenWaves = 5f; // Пауза между волнами
    public float healthMultiplierPerWave = 1.1f; // +10% здоровья за волну
    public float damageMultiplierPerWave = 1.05f; // +5% урона за волну

    [Header("Боссы")]
    public EnemyData bossEnemy;
    public int bossWaveInterval = 5; // Босс каждые 5 волн

    private void Start()
    {
        //StartNextWave();
    }
}
