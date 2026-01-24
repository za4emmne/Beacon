using UnityEngine;
using System;

public class CharactersHealth : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;

    [SerializeField] protected float _health;

    public event Action Changed;
    public event Action RaiseMe;
    public event Action Died;

    public float MaxCurrent => _maxHealth;
    public float Current => _health;

    public void Init(float health)
    {
        _maxHealth = health;
        _health = _maxHealth;
    }

    public virtual void TakeDamage(float damage, Vector2 hitSourcePosition)
    {
        if (damage >= 0 & _health > 0)
        {
            _health -= damage;
            
            if (_health <= 0)
            {
                _health = 0;
                Died?.Invoke();
            }

            Changed?.Invoke();
        }
        else if (_health <= 0)
        {
            Died?.Invoke();
        }
    }

    protected void ChangeAwake()
    {
        Changed?.Invoke();
    }

    protected void OnRaise()
    {
        RaiseMe?.Invoke();
    }
}