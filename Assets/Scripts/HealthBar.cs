using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerHealth _player;

    private void Start()
    {
        _player = GetComponent<PlayerHealth>();
        _slider.maxValue = _player.GetMaxHealth;
    }

    private void Update()
    {
        _slider.value = _player.GetHealth;
    }
}
