using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string nameWave;
        public int totalEnemies; // ����� ����� ������ � ����
        public float waveTime;
        public float spawnRate;
        public EnemyData[] possibleEnemies; // ����� ����� ����� ����������
    }

    public Wave[] waves; // ������ ����
    private int currentWaveIndex = 0;
    private int enemiesLeftToSpawn;
    private int enemiesAlive;
    private float nextSpawnTime;

    [Header("��������� ���������")]
    public float timeBetweenWaves = 5f; // ����� ����� �������
    public float healthMultiplierPerWave = 1.1f; // +10% �������� �� �����
    public float damageMultiplierPerWave = 1.05f; // +5% ����� �� �����

    [Header("�����")]
    public EnemyData bossEnemy;
    public int bossWaveInterval = 5; // ���� ������ 5 ����

    private void Start()
    {
        //StartNextWave();
    }
}
