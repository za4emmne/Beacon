using UnityEngine;
using System;

[RequireComponent(typeof(CharactersHealth))]

public class EnemyAttacked : MonoBehaviour
{
    [SerializeField] private float _damage = 1.5f;
    [SerializeField] private GameObject _effect;

    public event Action Attacked;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            Instantiate(_effect, player.transform.position, Quaternion.identity);
            player.GetComponent<CharactersHealth>().TakeDamage(_damage);
            Attacked?.Invoke();
        }
    }
}