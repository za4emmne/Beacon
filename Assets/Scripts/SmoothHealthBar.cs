using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _stepHealth = 8f;
    [SerializeField] private PlayerHealth _player;

    private float _currentHealth;

    private void Start()
    {
        _player = GetComponent<PlayerHealth>();
        _slider.maxValue = _player.GetMaxHealth;
        _slider.maxValue = _player.GetMaxHealth;
        _slider.value = _slider.maxValue;
    }

    private void Update()
    {
        SetHealth();
    }

    public void SetHealth()
    {
        _currentHealth = _player.GetHealth;

        if (_currentHealth != _slider.value)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _currentHealth, _stepHealth * Time.deltaTime);
        }
    }
}
