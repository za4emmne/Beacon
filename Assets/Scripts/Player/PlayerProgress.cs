using System;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    [SerializeField] private float _nextLevel;

    private int _level;
    private float _currentProgress;

    public event Action<float> ChangedProgress;
    public event Action<float> LevelUpFloat;
    public event Action LevelUp;

    public int Level => _level;

    private void Start()
    {
        _level = 0;
        _currentProgress = 0;
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
        float randomCoefficient = UnityEngine.Random.Range(1, 1.3f);
        _level++;
        _currentProgress = 0;
        ChangedProgress?.Invoke(_currentProgress); //
        _nextLevel = _nextLevel * _level * randomCoefficient;
        LevelUpFloat?.Invoke(_nextLevel); //устанавливает следующий порог уровня
        LevelUp?.Invoke();
    }
}
