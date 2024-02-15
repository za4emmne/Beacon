using UnityEngine;

[RequireComponent(typeof(SmoothHealthBar))]
[RequireComponent(typeof(HealthBar))]

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _health;

    private HealthBar _healthBar;
    private SmoothHealthBar _smoothHealthBar;
    private float _maxHealth = 100f;
    private float _currentHealth;

    private void Start()
    {
        _healthBar = GetComponent<HealthBar>();
        _smoothHealthBar = GetComponent<SmoothHealthBar>();

        _health = _maxHealth;
    }

    public void TakeHealth(int pills)
    {
        _health += pills;

        if (_health >= _maxHealth)
            _health = _maxHealth;

        _currentHealth = _health / _maxHealth;
        _healthBar.ChangeValue(_currentHealth);
        StartCoroutine(_smoothHealthBar.ChangeValue(_currentHealth));
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            _health = 0;

        _currentHealth = _health / _maxHealth;
        Debug.Log(_health);
        Debug.Log(_maxHealth);
        _healthBar.ChangeValue(_currentHealth);
        StartCoroutine(_smoothHealthBar.ChangeValue(_currentHealth));
    }

    public float GetMaxHealth
    {
        get { return _maxHealth; }
    }

    public float GetHealth
    {
        get {return _health; }        
    }
}
