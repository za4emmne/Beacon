using UnityEngine;
using System;

[RequireComponent(typeof(CharactersHealth))]

public class EnemyAttacked : MonoBehaviour
{
    [SerializeField] private float _damage = 1.5f;

    public event Action Attacked;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            player.GetComponent<CharactersHealth>().TakeDamage(_damage);
            Attacked?.Invoke();
        }
    }
}