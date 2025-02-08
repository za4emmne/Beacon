using UnityEngine;
using System;
using System.Collections;

public class EnemyAttacked : MonoBehaviour
{
    private float _damage;
    [SerializeField] private float _attackTime;
    private Coroutine _coroutine;

    public event Action Attacked;

    private void Start()
    {
        _coroutine = null;
        _damage = 2f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(_damage);
            Attacked?.Invoke();
        }
    }
}