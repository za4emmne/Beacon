using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeHealth : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private HealthBarMathf _healthBarMathf;

    private int _maxHealth = 100;

    private void Start()
    {
        _health = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
        _healthBarMathf.SetMaxHealth(_maxHealth);
    }

    private void Update()
    {
        _healthBar.SetHealth(_health);
        _healthBarMathf.SetHealth(_health);
    }

    public void TakeHealth(int pills)
    {
        _health += pills;

        if (_health >= _maxHealth)
            _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            _health = 0;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public int GetHealth()
    {
        return _health;
    }
}
