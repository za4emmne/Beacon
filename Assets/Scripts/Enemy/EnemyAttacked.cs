using UnityEngine;
using System;

public class EnemyAttacked : MonoBehaviour
{
    private float _damage;

    public event Action Attacked;

    private void Awake()
    {
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