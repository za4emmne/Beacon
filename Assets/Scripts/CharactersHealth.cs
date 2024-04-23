using UnityEngine;
using System;

public class CharactersHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private float _health;
    private bool _isDead;

    public event Action Changed;
    public event Action Died;

    public float MaxCurrent => _maxHealth;
    public float Current => _health;

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

            Changed?.Invoke();
        }
    }

    public void TakePills(float pills)
    {
        if(pills >= 0)
        {
            _health += pills;

            if(_health >= _maxHealth)
            {
                _health = _maxHealth;
            }

            Changed?.Invoke();
        }
    }
}