using UnityEngine;
using System;
using System.Collections;

public class EnemyAttacked : MonoBehaviour
{
    private float _damage;
    private EnemyHealth _health;

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
            player.TakeDamage(_damage, transform.position);
            Attacked?.Invoke();
        }
    }
}