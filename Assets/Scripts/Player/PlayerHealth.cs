using UnityEngine;

[RequireComponent(typeof(SmoothHealthBar))]
[RequireComponent(typeof(HealthBar))]

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _health;

    private HealthBar _healthBar;
    private SmoothHealthBar _smoothHealthBar;
    private int _maxHealth = 100;

    private void Start()
    {
        _healthBar = GetComponent<HealthBar>();
        _smoothHealthBar = GetComponent<SmoothHealthBar>();

        _health = _maxHealth;
        _healthBar.SetMaxValue(_maxHealth);
        _smoothHealthBar.SetMaxValue(_maxHealth);
    }

    public void TakeHealth(int pills)
    {
        _health += pills;

        if (_health >= _maxHealth)
            _health = _maxHealth;

        _healthBar.ChangeValue(_health);
        StartCoroutine(_smoothHealthBar.ChangeValue(_health));
        //StopCoroutine(_smoothHealthBar.ChangeValue(_health));
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            _health = 0;

        _healthBar.ChangeValue(_health);
        StartCoroutine(_smoothHealthBar.ChangeValue(_health));
        //StopCoroutine(_smoothHealthBar.ChangeValue(_health));
    }

    public int GetMaxHealth
    {
        get { return _maxHealth; }
    }

    public int GetHealth
    {
        get {return _health; }        
    }
}
