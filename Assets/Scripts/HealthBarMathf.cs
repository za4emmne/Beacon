using UnityEngine;
using UnityEngine.UI;

public class HealthBarMathf : MonoBehaviour
{
    [SerializeField] private Image _health;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _stepHealth = 8f;

    private float _currentHealth;

    private void Start()
    {

    }

    private void Update()
    {
       
    }

    public void SetHealth(int health)
    {
        _currentHealth = health;
        
        if (_currentHealth != _slider.value)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _currentHealth, _stepHealth * Time.deltaTime);
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }
}
