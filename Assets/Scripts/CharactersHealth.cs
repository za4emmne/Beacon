using UnityEngine;
using System;

public class CharactersHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private float _health;
    private bool _isDead;

    public event Action HealthChanged;
    public event Action Died;

    public float MaxHealth => _maxHealth;
    public float Health => _health;

    private void Awake()
    {
        _health = _maxHealth;
        _isDead = false;
    }

    public void TakeDamage(float damage)
    {
        if (damage >= 0)
        {
            _health -= damage;

            if (_health <= 0)
            {
                _health = 0;
                _isDead = true;
                Died?.Invoke();
            }

            HealthChanged?.Invoke();
        }
    }

    public void TakeHealth(int pills)
    {
        if(pills >= 0)
        {
            _health += pills;

            if(_health >= _maxHealth)
            {
                _health = _maxHealth;
            }

            HealthChanged?.Invoke();
        }
    }
}