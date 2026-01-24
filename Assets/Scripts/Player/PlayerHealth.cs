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
    private PlayerMovement _movement;
    private PlayerAnimation _animator;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        _isUndead = false;
    }

    private IEnumerator UndeadWait()
    {
        CircleCollider2D undeadRadius = Player.singleton.UndeadEffect.GetComponent<CircleCollider2D>();
        Rigidbody2D playerRb = Player.singleton.GetComponent<Rigidbody2D>();
        int secondWait = 4;
        var waitForSeconds = new WaitForSeconds(1f);

        _isUndead = true;
        undeadRadius.isTrigger = false;
        playerRb.bodyType = RigidbodyType2D.Kinematic;

        for (int i = 0; i < secondWait + 1; i++)
            yield return waitForSeconds;

        undeadRadius.isTrigger = true;
        playerRb.bodyType = RigidbodyType2D.Dynamic;
        _isUndead = false;
        _coroutine = null;
    }

    public void StartUndeadProcess()
    {
        _health = _maxHealth / 2;
        OnRaise();

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

    public override void TakeDamage(float damage, Vector2 hitSourcePosition)
    {
        if (_isUndead == false)
        {
            base.TakeDamage(damage, hitSourcePosition);

            if (_health > 0)
            {
                _animator.OnGetDamageAnimation();
                _movement.KnockbackFromPlayer(hitSourcePosition);
            }
        }

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
