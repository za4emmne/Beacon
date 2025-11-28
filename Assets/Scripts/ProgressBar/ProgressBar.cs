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
        Debug.Log("Init prog bar");
    }

    public void SetMaxValue(float maxValue)
    {
        Debug.Log("SetMaxValue: " + maxValue);
        _slider.maxValue = maxValue;
    }

    public void SetValue(float value)
    {
        Debug.Log("SetValue: " + value + " / " + _slider.maxValue);
        _slider.value = Mathf.Clamp(value, 0, _slider.maxValue);

    }
}
