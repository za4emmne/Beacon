using System;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private int _level;
    private float _nextLevel;
    private float _currentProgress;

    public event Action<float> ChangedProgress;
    public event Action<float> LevelUpFloat;
    public event Action LevelUp;

    private void Start()
    {
        _nextLevel = 10;
        _level = 0;
        _currentProgress = 0;
        LevelUpFloat?.Invoke(_nextLevel);
    }

    public void AddProgress(float score)
    {
        _currentProgress += score;
        ChangedProgress?.Invoke(_currentProgress);

        if (_currentProgress >= _nextLevel)
        {
            UpLevel();
        }
    }

    private void UpLevel()
    {
        float randomCoefficient = UnityEngine.Random.Range(1, 1.3f);
        _level++;
        _currentProgress = 0;
        ChangedProgress?.Invoke(_currentProgress);
        _nextLevel = _nextLevel * _level * randomCoefficient;
        LevelUpFloat?.Invoke(_nextLevel);
        LevelUp?.Invoke();
    }
}
