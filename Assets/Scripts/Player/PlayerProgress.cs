using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    [SerializeField] private ProgressBar _progressBar;

    private int _level;
    private float _nextLevel;
    private int _currentProgress;

    private void Start()
    {
        _nextLevel = 10;
        _level = 0;
        _currentProgress = 0;
        _progressBar.SetMaxValue(_nextLevel);
    }

    public void AddProgress(int score)
    {
        _currentProgress += score;
        _progressBar.SetValue(_currentProgress);
        Debug.Log("Set");

        if (_currentProgress >= _nextLevel)
        {
            UpLevel();
            _progressBar.SetMaxValue(_nextLevel);
        }
    }

    private void UpLevel()
    {
        float randomCoefficient = Random.Range(1, 1.3f);
        _level++;
        _nextLevel = _nextLevel * _level * randomCoefficient;
    }
}
