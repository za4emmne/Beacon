using UnityEngine;
using System;
using System.Collections;

public class EnemyAttacked : MonoBehaviour
{
    //[SerializeField] private float _attackTime;

    private float _damage;
    private EnemyHealth _health;
    //private Coroutine _coroutine;

    public event Action Attacked;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
    }

    public void Init(float damage)
    {
        _damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player) && _health.Current > 0)
        {
            player.TakeDamage(_damage);
            Attacked?.Invoke();
        }
    }
}