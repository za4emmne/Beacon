using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerProgress _player;

    private void Start()
    {
        _player.EnsureInitialized();
        SetMaxValue(_player.GetNextLevel());
        _slider.value = 0;
    }

    private void OnEnable()
    {
        _player.ChangedProgress += SetValue;
        _player.LevelUpFloat += SetMaxValue;
    }

    private void OnDisable()
    {
        _player.ChangedProgress -= SetValue;
        _player.LevelUpFloat -= SetMaxValue;
    }

    public void SetMaxValue(float maxValue)
    {
        _slider.maxValue = maxValue;
    }

    public void SetValue(float value)
    {
        _slider.value = value;
    }
}
