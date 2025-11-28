using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CharactersHealth
{
    public event Action CriticalHealth;
    public event Action SuperCriticalHealth;
    public event Action NormalHealth;

    private bool _isUndead;
    private Coroutine _coroutine;

    private void Start()
    {
        _isUndead = false;
    }

    private IEnumerator UndeadWait()
    {
        int secondWait = 4;
        var waitForSeconds = new WaitForSeconds(1f);
        _isUndead = true;

        for (int i = 0; i < secondWait + 1; i++)
            yield return waitForSeconds;

        _isUndead = false;
        _coroutine = null;
    }

    public void StartUndeadProcess()
    {
        if (_coroutine == null && _isUndead == false)
        {
            _coroutine = StartCoroutine(UndeadWait());
        }
    }

    public void UpgraidMaxHealth(int count)
    {
        _maxHealth += (_maxHealth / count);
        ChangeAwake();
    }

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

    public void Raise()
    {
        _health = _maxHealth / 2;
        OnRaise();
    }

    public override void TakeDamage(float damage)
    {
        if (_isUndead == false)
            base.TakeDamage(damage);

        //GetComponent<Rigidbody2D>().AddForce(Vector2.down * 100f, ForceMode2D.Impulse);

        if (_health < _maxHealth / 2)
        {
            CriticalHealth?.Invoke();
        }

        if (_health < _maxHealth / 3)
        {
            SuperCriticalHealth?.Invoke();
        }
    }
}
