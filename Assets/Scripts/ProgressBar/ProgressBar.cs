using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private PlayerLevelManager _player;

    private void OnDisable()
    {
        if (_player != null)
        {
            _player.ChangedProgress -= SetValue;
            _player.LevelUpFloat -= SetMaxValue;
        }
    }

    public void Init()
    {
        _player = Player.singleton.GetComponent<PlayerLevelManager>();
        _player.ChangedProgress += SetValue;
        _player.LevelUpFloat += SetMaxValue;

        _player.EnsureInitialized();
        SetMaxValue(_player.GetNextLevel());
        SetValue(0);
    }

    public void SetMaxValue(float maxValue)
    {
        _slider.maxValue = maxValue;
    }

    public void SetValue(float value)
    {
        _slider.value = Mathf.Clamp(value, 0, _slider.maxValue);
    }
}
