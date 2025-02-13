using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private PlayerProgress _player;

    private void Awake()
    {
        _player = Player.singleton.GetComponent<PlayerProgress>();
    }

    private void Start()
    {
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
