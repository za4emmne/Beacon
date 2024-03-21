using UnityEngine;
using System;

public class CharactersHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private float _health;
    private bool _isDead;
    public float MaxHealth => _maxHealth;
    public float Health => _health;
    public bool IsDead => _isDead;

    public event Action HealthChanged;
    public event Action AnimationDeadPlayed;

    private void Awake()
    {
        _health = _maxHealth;
        _isDead = false;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            _isDead = true;
            AnimationDeadPlayed?.Invoke();
            Destroy(this.gameObject, 1f);
        }

        HealthChanged?.Invoke();
    }
}