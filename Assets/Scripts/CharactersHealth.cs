using UnityEngine;
using System;

public class CharactersHealth : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;

    [SerializeField] protected float _health;

    public event Action Changed;
    public event Action Died;

    public float MaxCurrent => _maxHealth;
    public float Current => _health;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (damage >= 0)
        {
            _health -= damage;

            if (_health <= 0)
            {
                _health = 0;
                Died?.Invoke();
            }

            Changed?.Invoke();
        }
    }

    protected void ChangeAwake()
    {
        Changed?.Invoke();
    }
}