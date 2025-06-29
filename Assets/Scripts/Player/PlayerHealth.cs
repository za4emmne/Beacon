using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CharactersHealth
{
    public event Action CriticalHealth;
    public event Action NormalHealth;

    public void TakePills(float indexPill)
    {
        _health += indexPill;

        if (_health > _maxHealth)
            _health = _maxHealth;

        ChangeAwake();

        if (_health > _maxHealth / 1.5f)
            NormalHealth?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Pills>(out Pills pills))
        {
            TakePills(pills.Count);
            pills.PutPills();
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (_health < _maxHealth / 3)
        {
            CriticalHealth?.Invoke();
        }
    }
}
