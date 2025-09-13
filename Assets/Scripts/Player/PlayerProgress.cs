using System;
using System.Collections;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    [Header("Level Progression Settings")]
    [SerializeField] private float _baseEarlyGameXP = 8f;    // Множитель для уровней 1-10
    [SerializeField] private float _midGameMultiplier = 12f;  // Множитель для уровней 11-20
    [SerializeField] private float _lateGameMultiplier = 15f; // Множитель для уровней 21+
    [SerializeField] private float _exponentialFactor = 1.1f; // Экспоненциальный рост после 20 уровня

    private int _level;
    private float _currentProgress;
    private float _nextLevel;


    public event Action<float> ChangedProgress;
    public event Action<float> LevelUpFloat;
    public event Action LevelUp;

    public int Level => _level;
    public float CurrentProgress => _currentProgress;
    public float NextLevel => _nextLevel;

    private void Start()
    {
        _level = 1;
        _currentProgress = 0;
        _nextLevel = CalculateXPForLevel(_level);
    }
    private float CalculateXPForLevel(int level)
    {
        if (level <= 10)
        {
            // Легко: уровни 1-10
            return level * _baseEarlyGameXP + 2;
        }
        else if (level <= 20)
        {
            // Средне: уровни 11-20
            return level * _midGameMultiplier - 20;
        }
        else
        {
            // Сложно: уровни 21+
            return level * _lateGameMultiplier * Mathf.Pow(_exponentialFactor, level - 20);
        }
    }

    public void EnsureInitialized()
    {
        if (_nextLevel == 0)
        {
            _level = 1;
            _currentProgress = 0;
            _nextLevel = CalculateXPForLevel(_level);
        }
    }

    public float GetProgressPercent()
    {
        return _currentProgress / _nextLevel;
    }

    public float GetNextLevel()
    { return _nextLevel; }

    public void AddProgress(float score)
    {
        _currentProgress += score;
        ChangedProgress?.Invoke(_currentProgress);

        if (_currentProgress >= _nextLevel)
            UpLevel();
    }

    private void UpLevel()
    {
        _level++;
        _currentProgress = 0;
        ChangedProgress?.Invoke(_currentProgress);
        _nextLevel = CalculateXPForLevel(_level);
        LevelUpFloat?.Invoke(_nextLevel); //устанавливает следующий порог уровня
        LevelUp?.Invoke();
    }

    [ContextMenu("Detailed Balance Test")]
    private void DetailedBalanceTest()
    {
        Debug.Log("=== Detailed Balance Analysis ===");

        for (int testLevel = 1; testLevel <= 25; testLevel++)
        {
            float xpNeeded = CalculateXPForLevel(testLevel);
            float estimatedTime = xpNeeded / 10f; // Предполагаем 10 XP в секунду

            string phase = testLevel <= 10 ? "ЛЕГКО" : testLevel <= 20 ? "СРЕДНЕ" : "СЛОЖНО";

            Debug.Log($"Level {testLevel}: {xpNeeded:F0} XP | ~{estimatedTime:F1} sec | {phase}");
        }
    }

    [ContextMenu("Live Balance Test")]
    private void LiveBalanceTest()
    {
        StartCoroutine(TestLeveling());
    }

    private IEnumerator TestLeveling()
    {
        float testXPPerSecond = 10f; // Симулируем получение XP

        while (_level < 25)
        {
            AddProgress(testXPPerSecond);
            Debug.Log($"Level {_level}: {_currentProgress:F0}/{_nextLevel:F0} XP");
            yield return new WaitForSeconds(1f);
        }
    }

}
